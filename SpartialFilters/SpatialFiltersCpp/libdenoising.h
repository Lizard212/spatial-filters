
#ifndef _LIBDENOISING_H_
#define _LIBDENOISING_H_

#include "libauxiliar.h"

/**
 * @file   libdenoising.cpp
 * @brief  Denoising functions
 */
void nlmeans_ipol(int iDWin,                    // Half size of comparison window
                  int iDBloc,           // Half size of research window
                  float fSigma,         // Noise parameter
                  float fFiltPar,       // Filtering parameter
                  float **fpI,          // Input
                  float **fpO,          // Output
                  int iChannels, int iWidth,int iHeight);

#endif
