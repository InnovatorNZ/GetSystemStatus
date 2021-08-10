//#define CUDA_METHOD
#ifdef CUDA_METHOD
#include <cuda.h>
#include <cuda_runtime.h>
#include <device_launch_parameters.h>
#endif
#include <stdio.h>
#include <Windows.h>  
#include <iostream>  
#include <DXGI.h>  
#include <vector>  
using namespace std;

#pragma comment(lib, "dxgi.lib")

#ifdef CUDA_METHOD
#pragma comment(lib, "cuda.lib")
#pragma comment(lib, "cudart.lib")
int CUDA() {
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
#endif

std::string WStringToString(const std::wstring& wstr) {
	std::string str(wstr.length(), ' ');
	std::copy(wstr.begin(), wstr.end(), str.begin());
	return str;
}
int DX()
{
	IDXGIFactory* pFactory;
	IDXGIAdapter* pAdapter;
	std::vector <IDXGIAdapter*> vAdapters;
	int iAdapterNum = 0;	//显卡数量
	HRESULT hr = CreateDXGIFactory(__uuidof(IDXGIFactory), (void**)(&pFactory));

	if (FAILED(hr)) return -1;

	// 枚举适配器
	while (pFactory->EnumAdapters(iAdapterNum, &pAdapter) != DXGI_ERROR_NOT_FOUND) {
		vAdapters.push_back(pAdapter);
		++iAdapterNum;
	}

	cout << "===============获取到" << iAdapterNum << "块显卡===============" << endl;
	for (size_t i = 0; i < vAdapters.size(); i++)
	{
		DXGI_ADAPTER_DESC adapterDesc;
		vAdapters[i]->GetDesc(&adapterDesc);
		wstring aa(adapterDesc.Description);
		std::string bb = WStringToString(aa);
		cout << dec;
		cout << "系统视频内存: " << adapterDesc.DedicatedSystemMemory / 1024 / 1024 << "M" << endl;
		cout << "专用视频内存: " << adapterDesc.DedicatedVideoMemory / 1024 / 1024 << "M" << endl;
		cout << "共享系统内存: " << adapterDesc.SharedSystemMemory / 1024 / 1024 << "M" << endl;
		cout << "设备描述: " << bb.c_str() << endl;
		cout << "设备ID: " << adapterDesc.DeviceId << endl;
		cout << "PCI ID修正版本: " << adapterDesc.Revision << endl;
		cout << "子系统PIC ID: " << adapterDesc.SubSysId << endl;
		cout << "厂商编号: " << adapterDesc.VendorId << endl;
		cout << "LUID: " << hex << adapterDesc.AdapterLuid.LowPart << endl;

		// 输出显示设备
		IDXGIOutput* pOutput;
		std::vector<IDXGIOutput*> vOutputs;
		int iOutputNum = 0;
		while (vAdapters[i]->EnumOutputs(iOutputNum, &pOutput) != DXGI_ERROR_NOT_FOUND) {
			vOutputs.push_back(pOutput);
			iOutputNum++;
		}

		cout << "-----------------------------------------" << endl << dec;
		cout << "获取到" << iOutputNum << "个显示设备:" << endl << endl;
		for (size_t n = 0; n < vOutputs.size(); n++) {
			DXGI_OUTPUT_DESC outputDesc;
			vOutputs[n]->GetDesc(&outputDesc);
			UINT uModeNum = 0;
			DXGI_FORMAT format = DXGI_FORMAT_R8G8B8A8_UNORM;
			UINT flags = DXGI_ENUM_MODES_INTERLACED;
			vOutputs[n]->GetDisplayModeList(format, flags, &uModeNum, 0);
			DXGI_MODE_DESC* pModeDescs = new DXGI_MODE_DESC[uModeNum];
			vOutputs[n]->GetDisplayModeList(format, flags, &uModeNum, pModeDescs);

			cout << "显示设备名称:" << outputDesc.DeviceName << endl;
			cout << "显示设备当前分辨率:" << outputDesc.DesktopCoordinates.right - outputDesc.DesktopCoordinates.left << "*" << outputDesc.DesktopCoordinates.bottom - outputDesc.DesktopCoordinates.top << endl;
			cout << endl;
			cout << "分辨率信息:" << endl;
			for (UINT m = 0; m < uModeNum; m++) {
				cout << "== 分辨率:" << pModeDescs[m].Width << "*" << pModeDescs[m].Height << "     刷新率" << (pModeDescs[m].RefreshRate.Numerator) / (pModeDescs[m].RefreshRate.Denominator) << endl;
			}
		}
		vOutputs.clear();
		cout << endl << "=============================================================" << endl << endl;
	}
	vAdapters.clear();
	return 0;
}

int main() {
	return DX();
}