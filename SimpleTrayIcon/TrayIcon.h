#pragma once
#include "TrayMenuItem.h"
#include "TrayMenuPopup.h"
#include "TrayIconManager.h"
class TrayIcon
{
private:
	static const inline wchar_t* TrayIconWndClass = L"TrayIconWnd";
	UINT m_iconNotifyWm;
	UINT m_taskbarRestartWm{};
	BOOL m_trayIconCreated = false;
	TrayIconDataWrapper& m_trayIconData = TrayIconManager::Create();
	TrayMenuPopup* m_trayMenuPopup = NULL;
	HWND m_trayIconHwnd = NULL;
	std::vector<std::reference_wrapper<TrayMenuItemBase>> m_items;
public:
	TrayIcon(const HICON hIcon, const LPWSTR tip);
	~TrayIcon();
	void AddItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void RemoveItem(TrayMenuItemBase& pTrayMenuItem);
	void SetIcon(const HICON hIcon) noexcept;
private:
	static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) noexcept;
	inline static TrayIcon* GetInstance(const HWND hWnd);
	LRESULT TrayIconWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
};
