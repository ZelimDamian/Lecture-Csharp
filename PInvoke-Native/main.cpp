#ifndef _NATIVELIB_H_
#define _NATIVELIB_H_

#include "Tester.h"

#ifndef MYAPI
#define MYAPI __declspec(dllexport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

	MYAPI double piNative(int iters)
	{
		return Tester::pi(iters);
	}

	MYAPI void matmultNative(double* a, double* b, int aRows, int aCols, int bCols, double* x)
	{
		Tester::matmult(a, b, aRows, aCols, bCols, x);
	}

#ifdef __cplusplus
}
#endif

#endif // _NATIVELIB_H_