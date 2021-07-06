#pragma once

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <cuda.h>
#include <cuda_runtime.h>
#include <device_launch_parameters.h>

#pragma comment(lib, "cuda.lib")
#pragma comment(lib, "cudart.lib")

extern "C" _declspec(dllexport) int __stdcall Init(int deviceId);
extern "C" _declspec(dllexport) long long __stdcall InitAndGetNVGPUMemory(int deviceId);
extern "C" _declspec(dllexport) long long __stdcall GetNVGPUMemory(int deviceId);