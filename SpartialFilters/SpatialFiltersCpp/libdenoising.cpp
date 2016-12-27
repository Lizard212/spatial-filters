#include "libdenoising.h"
#include <cmath>
#include <algorithm>

using std::max;
using std::min;

#define fTiny 0.00000001f

void nlmeans_ipol(int iDWin,            // Half size of patch
                  int iDBloc,           // Half size of research window
                  float fSigma,         // Noise parameter
                  float fFiltPar,       // Filtering parameter
                  float **fpI,          // Input
                  float **fpO,          // Output
                  int iChannels, int iWidth, int iHeight) {
  // length of each channel
  int iwxh = iWidth * iHeight;

  //  length of comparison window
  int ihwl = (2 * iDWin + 1);
  int iwl = (2 * iDWin + 1) * (2 * iDWin + 1);
  int icwl = iChannels * iwl;

  // filtering parameter
  float fSigma2 = fSigma * fSigma;
  float fH = fFiltPar * fSigma;
  float fH2 = fH * fH;

  // multiply by size of patch, since distances are not normalized
  fH2 *= (float) icwl;

  // tabulate exp(-x), faster than using directly function expf
  int iLutLength = (int) rintf((float) LUTMAX * (float) LUTPRECISION);
  float *fpLut = new float[iLutLength];
  wxFillExpLut(fpLut, iLutLength);

  // auxiliary variable
  // number of denoised values per pixel
  float *fpCount = new float[iwxh];
  fpClear(fpCount, 0.0f, iwxh);

  // clear output
  for (int ii = 0; ii < iChannels; ii++) fpClear(fpO[ii], 0.0f, iwxh);

  // PROCESS STARTS
  // for each pixel (x,y)
#pragma omp parallel shared(fpI, fpO)
  {
#pragma omp for schedule(dynamic) nowait
    for (int y = 0; y < iHeight; y++) {
      // auxiliary variable
      // denoised patch centered at a certain pixel
      float **fpODenoised = new float *[iChannels];
      for (int ii = 0; ii < iChannels; ii++) fpODenoised[ii] = new float[iwl];
      for (int x = 0; x < iWidth; x++) {
        // reduce the size of the comparison window if we are near the boundary
        int iDWin0 =
            min(iDWin, min(iWidth - 1 - x, min(iHeight - 1 - y, min(x, y))));
        // research zone depending on the boundary and the size of the window
        int imin = max(x - iDBloc, iDWin0);
        int jmin = max(y - iDBloc, iDWin0);
        int imax = min(x + iDBloc, iWidth - 1 - iDWin0);
        int jmax = min(y + iDBloc, iHeight - 1 - iDWin0);

        //  clear current denoised patch
        for (int ii = 0; ii < iChannels; ii++)
          fpClear(fpODenoised[ii],
                  0.0f,
                  iwl);

        // maximum of weights. Used for reference patch
        float fMaxWeight = 0.0f;

        // sum of weights
        float fTotalWeight = 0.0f;

        for (int j = jmin; j <= jmax; j++)
          for (int i = imin; i <= imax; i++)
            if (i != x || j != y) {
              float fDif = fiL2FloatDist(fpI,
                                         fpI,
                                         x,
                                         y,
                                         i,
                                         j,
                                         iDWin0,
                                         iChannels,
                                         iWidth,
                                         iWidth);

              // dif^2 - 2 * fSigma^2 * N      dif is not normalized
              fDif = max(fDif - 2.0f * (float) icwl * fSigma2, 0.0f);
              fDif = fDif / fH2;
              float fWeight = wxSLUT(fDif, fpLut);
              if (fWeight > fMaxWeight) fMaxWeight = fWeight;
              fTotalWeight += fWeight;

              for (int is = -iDWin0; is <= iDWin0; is++) {
                int aiindex = (iDWin + is) * ihwl + iDWin;
                int ail = (j + is) * iWidth + i;

                for (int ir = -iDWin0; ir <= iDWin0; ir++) {

                  int iindex = aiindex + ir;
                  int il = ail + ir;

                  for (int ii = 0; ii < iChannels; ii++)
                    fpODenoised[ii][iindex] += fWeight * fpI[ii][il];

                }
              }
            }

        // current patch with fMaxWeight
        for (int is = -iDWin0; is <= iDWin0; is++) {
          int aiindex = (iDWin + is) * ihwl + iDWin;
          int ail = (y + is) * iWidth + x;
          for (int ir = -iDWin0; ir <= iDWin0; ir++) {
            int iindex = aiindex + ir;
            int il = ail + ir;
            for (int ii = 0; ii < iChannels; ii++)
              fpODenoised[ii][iindex] += fMaxWeight * fpI[ii][il];
          }
        }
        fTotalWeight += fMaxWeight;

        // normalize average value when fTotalweight is not near zero
        if (fTotalWeight > fTiny) {
          for (int is = -iDWin0; is <= iDWin0; is++) {
            int aiindex = (iDWin + is) * ihwl + iDWin;
            int ail = (y + is) * iWidth + x;
            for (int ir = -iDWin0; ir <= iDWin0; ir++) {
              int iindex = aiindex + ir;
              int il = ail + ir;
              fpCount[il]++;
              for (int ii = 0; ii < iChannels; ii++) {
                fpO[ii][il] += fpODenoised[ii][iindex] / fTotalWeight;
              }
            }
          }
        }
      }

      for (int ii = 0; ii < iChannels; ii++) delete[] fpODenoised[ii];
      delete[] fpODenoised;
    }
  }

  for (int ii = 0; ii < iwxh; ii++)
    if (fpCount[ii] > 0.0) {
      for (int jj = 0; jj < iChannels; jj++) fpO[jj][ii] /= fpCount[ii];
    } else {
      for (int jj = 0; jj < iChannels; jj++) fpO[jj][ii] = fpI[jj][ii];
    }
  // delete memory
  delete[] fpLut;
  delete[] fpCount;
}
