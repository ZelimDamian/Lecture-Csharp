#include "Tester.h"

#include <iostream>

// perform the actual test
double Tester::pi(uint64_t iters)
{
	double n = iters, i;         // Number of iterations and control variable
	double pi = 4;

	for (i = 3; i <= (n + 2); i += 2)
		pi = pi * ((i - 1) / i) * ((i + 1) / i);

	return pi;
}

void Tester::matmult(double* a, double* b, int aRows, int aCols, int bCols, double* x)
{
	// int m = aRows, n = bCols, p = aCols; 
	// Note: as is conventional in C#/C/C++, a and b are in row-major order.
	// Note: bRows (the number of rows in b) must equal aCols.
	int bRows = aCols;
	//double* x = new double[aRows * bCols]; // result
	double* c = new double[bRows * bCols];

	for (int i = 0; i < aCols; ++i) // transpose (large-matrix optimization)
	for (int j = 0; j < bCols; ++j)
		c[j*bRows + i] = b[i*bCols + j];

	for (int i = 0; i < aRows; ++i) {
		double* a_i = &a[i*aCols];
		for (int j = 0; j < bCols; ++j)
		{
			double* c_j = &c[j*bRows];
			double s = 0.0;
			for (int k = 0; k < aCols; ++k)
				s += a_i[k] * c_j[k];
			x[i*bCols + j] = s;
		}
	}
	delete[] c;
}