#pragma once
#include "pch.h"
#include "MyTrayMenu.h"
#include "TrayIcon.h"
#include <queue>
using namespace std;
class MyTrayMenu
{
private:
	HICON m_hIcon;
	WCHAR m_tip[128];
	std::queue<std::reference_wrapper<TrayMenuItemBase>> m_addedItems;
	std::queue<std::reference_wrapper<TrayMenuItemBase>> m_items;

	// Nullable
	TrayIcon* m_trayIcon = NULL;

public:
	MyTrayMenu(const HICON hIcon, const LPWSTR tip) noexcept;
	~MyTrayMenu() noexcept;
	void Show() noexcept;
	void AddItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void RemoveItem(TrayMenuItemBase& pTrayMenuItem) noexcept;
	void Close();
	void SetIcon(const HICON hIcon) noexcept;

private:
	BOOL RemoveItem(std::queue<std::reference_wrapper<TrayMenuItemBase>>& pQueue, const TrayMenuItemBase& pItem);
};
