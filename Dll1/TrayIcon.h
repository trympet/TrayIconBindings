#pragma once
#include "TrayMenuItem.h"
#include "TrayMenuPopup.h"
class TrayIcon
{
private:
	static const inline wchar_t* TrayIconWndClass = L"TrayIconWnd";
	UINT m_iconNotifyWm;
	UINT m_taskbarRestartWm{};
	BOOL m_trayIconCreated = false;
	NOTIFYICONDATA m_trayIconData{};
	TrayMenuPopup* m_trayMenuPopup = NULL;
	HWND m_trayIconHwnd = NULL;
	std::vector<std::reference_wrapper<TrayMenuItem>> m_items;
public:
	TrayIcon(const HICON hIcon, const LPWSTR tip) noexcept;
	~TrayIcon();
	void AddItem(TrayMenuItem& pTrayMenuItem) noexcept;
	void RemoveItem(TrayMenuItem& pTrayMenuItem);
private:
	static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) noexcept;
	inline static TrayIcon* GetInstance(const HWND hWnd);
	LRESULT TrayIconWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
};
