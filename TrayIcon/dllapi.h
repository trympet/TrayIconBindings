#pragma once
#include "MyTrayMenu.h"
#define TRAYAPI EXTERN_C __declspec(dllexport) HRESULT WINAPI

TRAYAPI TrayMenuCreate(const HICON hIcon, const _In_ LPWSTR tip, _Outptr_result_nullonfailure_ MyTrayMenu** pInstance) noexcept;
TRAYAPI TrayMenuRelease(const _Inout_ MyTrayMenu** pInstance) noexcept;
TRAYAPI TrayMenuShow(_In_ MyTrayMenu* pInstance) noexcept;
TRAYAPI TrayMenuClose(_In_ MyTrayMenu* pInstance) noexcept;
TRAYAPI TrayMenuAdd(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItem* pTrayMenuItem) noexcept;
TRAYAPI TrayMenuRemove(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItem* pTrayMenuItem) noexcept;

TRAYAPI TrayMenuItemCreate(const _In_ TrayMenuItemClicked onClicked, _Outptr_result_nullonfailure_ TrayMenuItem** pInstance) noexcept;
TRAYAPI TrayMenuItemRelease(const _Inout_ TrayMenuItem** pInstance) noexcept;
TRAYAPI TrayMenuItemContent(_In_ TrayMenuItem* pInstance, const _In_ LPWSTR value) noexcept;
TRAYAPI TrayMenuItemIsChecked(_In_ TrayMenuItem* pInstance, const BOOL value) noexcept;
