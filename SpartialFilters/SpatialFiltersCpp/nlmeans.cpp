#include <iostream>
#include <cstdlib>
#include "libdenoising.h"

extern "C" {
#include "iio.h"
}

using std::cerr;
using std::endl;


int main(int argc, char **argv) {
  if (argc != 4) {
    cerr << "usage: " << argv[0] << " input sigma output" << endl;
    return EXIT_FAILURE;
  }

  // read input
  int d_w, d_h, d_c;
  float *noisy = iio_read_image_float_split(argv[1], &d_w, &d_h, &d_c);
  float fSigma = atof(argv[2]);

  if (d_c == 2) {
    d_c = 1;    // we do not use the alpha channel
  }
  if (d_c > 3) {
    d_c = 3;    // we do not use the alpha channel
  }

  int d_wh = d_w * d_h;
  int d_whc = d_c * d_w * d_h;

  // denoise
  float **fpI = new float *[d_c];
  float **fpO = new float *[d_c];
  float *denoised = new float[d_whc];

  for (int ii = 0; ii < d_c; ii++) {
    fpI[ii] = &noisy[ii * d_wh];
    fpO[ii] = &denoised[ii * d_wh];
  }

  int bloc, win;
  float fFiltPar;

  if (d_c == 1) {
    if (fSigma <= 0.0f) {
      cerr << "error: sigma must be > 0" << endl;
      return EXIT_FAILURE;
    } else if (fSigma <= 15.0f) {
      win = 1;
      bloc = 10;
      fFiltPar = 0.4f;
    } else if (fSigma <= 30.0f) {
      win = 2;
      bloc = 10;
      fFiltPar = 0.4f;
    } else if (fSigma <= 45.0f) {
      win = 3;
      bloc = 17;
      fFiltPar = 0.35f;
    } else if (fSigma <= 75.0f) {
      win = 4;
      bloc = 17;
      fFiltPar = 0.35f;
    } else if (fSigma <= 100.0f) {
      win = 5;
      bloc = 17;
      fFiltPar = 0.30f;
    } else {
      cerr << "error: algorithm parametrized only for values of sigma less than 100.0" << endl;
      return EXIT_FAILURE;
    }
  } else {
    if (fSigma <= 0.0f) {
      cerr << "error: sigma must be > 0" << endl;
      return EXIT_FAILURE;
    } else if (fSigma <= 25.0f) {
      win = 1;
      bloc = 10;
      fFiltPar = 0.55f;
    } else if (fSigma <= 55.0f) {
      win = 2;
      bloc = 17;
      fFiltPar = 0.4f;
    } else if (fSigma <= 100.0f) {
      win = 3;
      bloc = 17;
      fFiltPar = 0.35f;
    } else {
      cerr << "error: algorithm parametrized only for values of sigma less than 100.0" << endl;
      return EXIT_FAILURE;
    }
  }

  nlmeans_ipol(win, bloc, fSigma, fFiltPar, fpI, fpO, d_c, d_w, d_h);

  delete[] fpI;
  delete[] fpO;
  // save denoised images
  iio_save_image_float_split(argv[3], denoised, d_w, d_h, d_c);
  delete[] denoised;

  return EXIT_SUCCESS;
}



