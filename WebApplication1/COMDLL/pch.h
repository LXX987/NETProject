// pch.h

#pragma once
#include <combaseapi.h>

// 新添加的代码
#pragma comment(lib,"COMDll1.lib")

extern "C" _declspec(dllimport) VARIANT CreateObject(const WCHAR * __comname, const WCHAR * __funcname, int __count, ...);
