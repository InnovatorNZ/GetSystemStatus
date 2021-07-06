#include <cuda.h>
#include <cuda_runtime.h>
#include <device_launch_parameters.h>
#include <stdio.h>
using namespace std;

#pragma comment(lib, "cuda.lib")
#pragma comment(lib, "cudart.lib")

int main() {
	cudaError_t cerr = cudaSetDevice(0);
	if (cerr != cudaSuccess(0)) return -1;
	int* dn;
	cudaMalloc((void**)&dn, 4 * sizeof(int));
	CUresult uRet = cuInit(0);
	size_t free;
	size_t total;
	uRet = cuMemGetInfo(&free, &total);
	if (uRet == CUDA_SUCCESS)
		printf("free = %dM\ntotal = %dM", free / 1024 / 1024, total / 1024 / 1024);
	return 0;
}