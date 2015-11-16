#pragma once

#include <cinttypes>

class Tester
{
public:
	Tester();
	~Tester();

	// perform the actual test
	static double pi(uint64_t iters);

	static void matmult(double* a, double* b, int aRows, int aCols, int bCols, double* x);
};

