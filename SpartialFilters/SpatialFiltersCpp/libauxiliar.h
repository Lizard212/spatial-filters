#ifndef _LIBAUXILIAR_H_
#define _LIBAUXILIAR_H_


///// LUT tables
#define LUTMAX 30.0
#define LUTMAXM1 29.0
#define LUTPRECISION 1000.0

const char *pick_option(int *c, char **v, const char *o, const char *d);

void wxFillExpLut(float *lut, int size);        // Fill exp(-x) lut

float wxSLUT(float dif, float *lut);                     // look at LUT

void fpClear(float *fpI, float fValue, int iLength);


float fiL2FloatDist(float *u0,
                    float *u1,
                    int i0,
                    int j0,
                    int i1,
                    int j1,
                    int radius,
                    int width0,
                    int width1);

float fiL2FloatDist(float **u0,
                    float **u1,
                    int i0,
                    int j0,
                    int i1,
                    int j1,
                    int radius,
                    int channels,
                    int width0,
                    int width1);

#endif
