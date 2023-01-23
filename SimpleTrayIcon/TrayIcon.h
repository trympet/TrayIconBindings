#pragma once
#include "TrayMenuItem.h"
#include "TrayMenuPopup.h"
#include "TrayIconManager.h"
#include <functional>
class TrayIcon
{
private:
	std::shared_ptr<TrayIconDataWrapper> m_trayIconData;
	std::optional<std::unique_ptr<TrayMenuPopup>> m_trayMenuPopup;
	HWND m_trayIconHwnd = NULL;
	std::vector<std::reference_wrapper<TrayMenuItemBase>> m_items;
	std::function<void()> m_onDoubleClick;
	HICON m_hIcon;
public:
	TrayIcon(const HICON hIcon, const LPWSTR tip, const std::function<void()> onDoubleClick);
	~TrayIcon();
	void AddItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void RemoveItem(TrayMenuItemBase& pTrayMenuItem);
	void SetIcon(const HICON hIcon) noexcept;
private:
	static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) noexcept;
	inline static TrayIcon* GetInstance(const HWND hWnd);
	LRESULT TrayIconWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
};
