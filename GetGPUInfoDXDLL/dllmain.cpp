// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

int iAdapterNum = 0;	//显卡数量
MyAdapterDesc* myAdapterDesc[128];

std::string WStringToString(const std::wstring& wstr) {
	std::string str(wstr.length(), ' ');
	std::copy(wstr.begin(), wstr.end(), str.begin());
	return str;
}
int __stdcall Init()
{
	IDXGIFactory* pFactory;
	IDXGIAdapter* pAdapter;
	std::vector <IDXGIAdapter*> vAdapters;
	HRESULT hr = CreateDXGIFactory(__uuidof(IDXGIFactory), (void**)(&pFactory));
	if (FAILED(hr)) return -1;

	// 枚举适配器
	while (pFactory->EnumAdapters(iAdapterNum, &pAdapter) != DXGI_ERROR_NOT_FOUND) {
		vAdapters.push_back(pAdapter);
		++iAdapterNum;
	}

	for (size_t i = 0; i < vAdapters.size(); i++)
	{
		DXGI_ADAPTER_DESC adapterDesc;
		vAdapters[i]->GetDesc(&adapterDesc);
		wstring aa(adapterDesc.Description);
		std::string bb = WStringToString(aa);
		/*
		cout << "系统视频内存: " << adapterDesc.DedicatedSystemMemory / 1024 / 1024 << "M" << endl;
		cout << "专用视频内存: " << adapterDesc.DedicatedVideoMemory / 1024 / 1024 << "M" << endl;
		cout << "共享系统内存: " << adapterDesc.SharedSystemMemory / 1024 / 1024 << "M" << endl;
		cout << "设备描述: " << bb.c_str() << endl;
		cout << "设备ID: " << adapterDesc.DeviceId << endl;
		cout << "PCI ID修正版本: " << adapterDesc.Revision << endl;
		cout << "子系统PIC ID: " << adapterDesc.SubSysId << endl;
		cout << "厂商编号: " << adapterDesc.VendorId << endl;
		cout << "LUID: " << hex << adapterDesc.AdapterLuid.LowPart << endl;
		*/

		myAdapterDesc[i] = new MyAdapterDesc(bb.c_str(), adapterDesc.DedicatedVideoMemory, adapterDesc.SharedSystemMemory, adapterDesc.AdapterLuid.LowPart);
	}
	vAdapters.clear();
	return 0;
}

const char* __stdcall getAdapterName(int luid) {
	for (int i = 0; i < iAdapterNum; i++) {
		if (myAdapterDesc[i]->luid == luid) {
			return myAdapterDesc[i]->name;
		}
	}
	return NULL;
}

int __stdcall getLUID(const char* name) {
	for (int i = 0; i < iAdapterNum; i++) {
		if (strcmp(myAdapterDesc[i]->name, name) == 0) {
			return myAdapterDesc[i]->luid;
		}
	}
	return -1;
}

long long __stdcall getDedicatedMemory(int luid) {
	for (int i = 0; i < iAdapterNum; i++) {
		if (myAdapterDesc[i]->luid == luid) {
			return myAdapterDesc[i]->dedicate_memory;
		}
	}
	return 0;
}

long long __stdcall getSharedMemory(int luid) {
	for (int i = 0; i < iAdapterNum; i++) {
		if (myAdapterDesc[i]->luid == luid) {
			return myAdapterDesc[i]->shared_memory;
		}
	}
	return 0;
}