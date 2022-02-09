#pragma once
#include "MyTrayMenu.h"
#include "TrayMenuSeparator.h"
#define TRAYAPI EXTERN_C __declspec(dllexport) HRESULT WINAPI

TRAYAPI TrayMenuCreate(const HICON hIcon, const _In_ LPWSTR tip, _Outptr_result_nullonfailure_ MyTrayMenu** pInstance) noexcept;
TRAYAPI TrayMenuRelease(const _Inout_ MyTrayMenu** pInstance) noexcept;
TRAYAPI TrayMenuShow(_In_ MyTrayMenu* pInstance) noexcept;
TRAYAPI TrayMenuClose(_In_ MyTrayMenu* pInstance) noexcept;
TRAYAPI TrayMenuAdd(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItemBase* pTrayMenuItem) noexcept;
TRAYAPI TrayMenuRemove(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItemBase* pTrayMenuItem) noexcept;

TRAYAPI TrayMenuItemCreate(const _In_ TrayMenuItemClicked onClicked, _Outptr_result_nullonfailure_ TrayMenuItem** pInstance) noexcept;
TRAYAPI TrayMenuItemRelease(const _Inout_ TrayMenuItem** pInstance) noexcept;
TRAYAPI TrayMenuItemContent(_In_ TrayMenuItem* pInstance, const _In_ LPWSTR value) noexcept;
TRAYAPI TrayMenuItemIsChecked(_In_ TrayMenuItem* pInstance, const BOOL value) noexcept;

TRAYAPI TrayMenuSeparatorCreate(_Outptr_result_nullonfailure_ TrayMenuSeparator** pInstance) noexcept;
TRAYAPI TrayMenuSeparatorRelease(const _Inout_ TrayMenuSeparator** pInstance) noexcept;