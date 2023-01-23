#pragma once
#include "pch.h"
#include "MyTrayMenu.h"
#include "TrayIcon.h"
#include <queue>
#include <optional>
using namespace std;
class MyTrayMenu;
typedef void(CALLBACK* TrayMenuClickHandler)(const MyTrayMenu*);
class MyTrayMenu
{
private:
	HICON m_hIcon;
	WCHAR m_tip[128];
	std::queue<std::reference_wrapper<TrayMenuItemBase>> m_addedItems;
	std::queue<std::reference_wrapper<TrayMenuItemBase>> m_items;
	TrayMenuClickHandler m_onDoubleClick;
	std::optional<std::unique_ptr<TrayIcon>> m_trayIcon;

public:
	MyTrayMenu(const HICON hIcon, const LPWSTR tip, const TrayMenuClickHandler onDoubleClick) noexcept;
	void Show() noexcept;
	void AddItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void RemoveItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void Close();
	void SetIcon(const HICON hIcon) noexcept;

private:
	BOOL RemoveItem(std::queue<std::reference_wrapper<TrayMenuItemBase>>& pQueue, const TrayMenuItemBase& pItem);
};
