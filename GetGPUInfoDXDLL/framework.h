#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <DXGI.h>
#include <vector>
#include <string>
using namespace std;

#pragma comment(lib, "dxgi.lib")

struct MyAdapterDesc {
	char name[128];
	long long dedicate_memory;
	long long shared_memory;
	int luid;
	MyAdapterDesc(const char* name, SIZE_T dedicate_memory, SIZE_T shared_memory, DWORD luid) {
		strcpy_s(this->name, name);
		this->dedicate_memory = dedicate_memory;
		this->shared_memory = shared_memory;
		this->luid = luid;
	}
};

extern "C" _declspec(dllexport) int __stdcall Init();
extern "C" _declspec(dllexport) const char* __stdcall getAdapterName(int luid);
extern "C" _declspec(dllexport) int __stdcall getLUID(const char*);
extern "C" _declspec(dllexport) long long __stdcall getDedicatedMemory(int);
extern "C" _declspec(dllexport) long long __stdcall getSharedMemory(int);