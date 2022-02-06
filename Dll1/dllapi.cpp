#include "pch.h"
#include "dllapi.h"

#define GUARD_NOT_NULL(x) if (!x) return E_POINTER

#define API_TRY try {
#define API_CATCH(x) } \
	catch (const Win32Exception& e) { \
			x \
			return e.GetHR(); \
	} \
	return S_OK;



TRAYAPI TrayMenuCreate(const HICON hIcon, const _In_ LPWSTR tip, _Outptr_result_nullonfailure_ MyTrayMenu** pInstance) noexcept
{
	*pInstance = NULL;
	GUARD_NOT_NULL(hIcon);

	API_TRY
		* pInstance = new MyTrayMenu(hIcon, tip);
	API_CATCH(
		if (pInstance) {
			if (*pInstance) {
				delete* pInstance;
			}

			*pInstance = NULL;
		}
	)
}

TRAYAPI TrayMenuRelease(const _Inout_ MyTrayMenu** pInstance) noexcept
{
	if (pInstance) {
		if (*pInstance) {
			delete* pInstance;
		}

		*pInstance = NULL;
	}

	return S_OK;
}

TRAYAPI TrayMenuShow(_In_ MyTrayMenu* pInstance) noexcept
{
	GUARD_NOT_NULL(pInstance);
	pInstance->Show();
	return S_OK;
}

TRAYAPI TrayMenuClose(_In_ MyTrayMenu* pInstance) noexcept
{
	GUARD_NOT_NULL(pInstance);
	pInstance->Close();
	return S_OK;
}

TRAYAPI TrayMenuAdd(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItem* pTrayMenuItem) noexcept
{
	GUARD_NOT_NULL(pInstance);
	GUARD_NOT_NULL(pTrayMenuItem);
	pInstance->AddItem(*pTrayMenuItem);
	return S_OK;
}

TRAYAPI TrayMenuRemove(_In_ MyTrayMenu* pInstance, _In_ TrayMenuItem* pTrayMenuItem) noexcept
{
	GUARD_NOT_NULL(pInstance);
	GUARD_NOT_NULL(pTrayMenuItem);
	pInstance->RemoveItem(*pTrayMenuItem);
	return S_OK;
}

TRAYAPI TrayMenuItemCreate(const _In_ TrayMenuItemClicked onClicked, _Outptr_result_nullonfailure_ TrayMenuItem** pInstance) noexcept
{
	GUARD_NOT_NULL(onClicked);
	GUARD_NOT_NULL(pInstance);
	*pInstance = NULL;

	API_TRY
		* pInstance = new TrayMenuItem(onClicked);
	API_CATCH(
		if (pInstance) {
			if (*pInstance) {
				delete* pInstance;
			}

			*pInstance = NULL;
		}
	)
}

TRAYAPI TrayMenuItemRelease(const _Inout_ TrayMenuItem** pInstance) noexcept
{
	if (pInstance) {
		if (*pInstance) {
			delete* pInstance;
		}

		*pInstance = NULL;
	}

	return S_OK;
}

TRAYAPI TrayMenuItemContent(_In_ TrayMenuItem* pInstance, const _In_ LPWSTR value) noexcept
{
	GUARD_NOT_NULL(pInstance);
	pInstance->Content(value);
	return S_OK;
}

TRAYAPI TrayMenuItemIsChecked(_In_ TrayMenuItem* pInstance, const BOOL value) noexcept
{
	GUARD_NOT_NULL(pInstance);
	pInstance->IsChecked(value);
	return S_OK;
}

