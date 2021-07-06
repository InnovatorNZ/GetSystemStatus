#include <GL/GL.h>
#include <Windows.h>
#include <gl/GLU.h>
#include <gl/glut.h>

// 基于扩展NVX_gpu_memory_info extension
UINT      QueryNVidiaCardMemory()
{
    __try
    {
        int iVal = 0;
        glGetIntegerv(GPU_MEMORY_INFO_DEDICATED_VIDMEM_NVX, &iVal);
        return (UINT)iVal / 1024;
    }
    __except (1)
    {
    }
    return 0;
}
// 基于扩展WGL_AMD_gpu_association
UINT      QueryAmdCardMemory()
{
    __try
    {
        typedef UINT(WINAPI* PFNWGLGETGPUIDSAMDPROC) (UINT maxCount, UINT* ids);
        typedef INT(WINAPI* PFNWGLGETGPUINFOAMDPROC) (UINT id, INT property, GLenum dataType, UINT size, void* data);
        PFNWGLGETGPUIDSAMDPROC wglGetGPUIDsAMD = (PFNWGLGETGPUIDSAMDPROC)wglGetProcAddress("wglGetGPUIDsAMD");
        PFNWGLGETGPUINFOAMDPROC wglGetGPUInfoAMD = (PFNWGLGETGPUINFOAMDPROC)wglGetProcAddress("wglGetGPUInfoAMD");
        if (!wglGetGPUIDsAMD || !wglGetGPUInfoAMD)
        {
            return 0;
        }
        UINT n = wglGetGPUIDsAMD(0, 0);
        UINT ids[24];
#define WGL_GPU_RAM_AMD 0x21A3
        size_t TotalMemSize = 0;
        wglGetGPUIDsAMD(n, ids);
        wglGetGPUInfoAMD(
            ids[0],
            WGL_GPU_RAM_AMD,
            GL_UNSIGNED_INT,
            sizeof(size_t),
            &TotalMemSize
        );
        return (UINT)TotalMemSize;
    }
    __except (1)
    {
    }
    return 0;
}
UINT      QueryDedicatedVideoCardMemory(bool bNvidiaCard, bool bAmdAtiCard)
{
    __try
    {
        HWND hWnd = (HWND)你可以用的HWND;
        HDC hdc = ::GetDC(hWnd);
        if (hdc == NULL)
        {
            return 0;
        }
        PIXELFORMATDESCRIPTOR FormatDesc;
        memset(&FormatDesc, 0, sizeof(PIXELFORMATDESCRIPTOR));
        FormatDesc.nSize = sizeof(PIXELFORMATDESCRIPTOR);
        FormatDesc.nVersion = 1;
        FormatDesc.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | /*PFD_SUPPORT_GDI|*/ PFD_DOUBLEBUFFER;
        FormatDesc.iPixelType = PFD_TYPE_RGBA;
        FormatDesc.cColorBits = 32;
        FormatDesc.cDepthBits = 32;
        // FormatDesc.cStencilBits = 0;
        FormatDesc.iLayerType = PFD_MAIN_PLANE;
        GLuint PixelFormat = ChoosePixelFormat(hdc, &FormatDesc);
        if (PixelFormat == NULL)
        {
            return 0;
        }
        SetPixelFormat(hdc, PixelFormat, &FormatDesc);
        HGLRC hContext = wglCreateContext(hdc);
        if (hContext == NULL)
        {
            return 0;
        }
        __try
        {
            if (wglMakeCurrent(hdc, hContext))
            {
                if (bNvidiaCard)
                {
                    return QueryNVidiaCardMemory();
                }
                else if (bAmdAtiCard)
                {
                    return QueryAmdCardMemory();
                }
            }
            else
            {
                return 0;
            }
        }
        __finally
        {
            wglMakeCurrent(NULL, NULL);
            ::DeleteDC(hdc);
            wglDeleteContext(hContext);
        }
    }
    __except (1)
    {
    }
    return 0;
}