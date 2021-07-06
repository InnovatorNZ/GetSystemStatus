// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"

long long mTotalMemory[100];

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        memset(mTotalMemory, 0, sizeof(mTotalMemory));
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

int __stdcall Init(int deviceId) {
    cudaError_t cerr = cudaSetDevice(deviceId);
    if (cerr != cudaSuccess(0)) return FALSE;
    int* dn;
    cudaMalloc((void**)&dn, 4 * sizeof(int));
    CUresult uRet = cuInit(0);
    size_t free;
    size_t total;
    uRet = cuMemGetInfo(&free, &total);
    if (uRet == CUDA_SUCCESS) {
        mTotalMemory[deviceId] = total;
        //printf("free = %dM\ntotal = %dM", free / 1024 / 1024, total / 1024 / 1024);
    }
    return TRUE;
}

long long __stdcall InitAndGetNVGPUMemory(int deviceId) {
    cudaError_t cerr = cudaSetDevice(deviceId);
    if (cerr != cudaSuccess(0)) return 0;
    int* dn;
    cudaMalloc((void**)&dn, 4 * sizeof(int));
    CUresult uRet = cuInit(0);
    size_t free;
    size_t total;
    uRet = cuMemGetInfo(&free, &total);
    if (uRet == CUDA_SUCCESS) {
        long long ret = total;
        return ret;
    }
    return 0;
}

long long __stdcall GetNVGPUMemory(int deviceId) {
    return mTotalMemory[deviceId];
}