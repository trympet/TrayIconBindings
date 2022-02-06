#pragma once
#include "TrayMenuItem.h"

class TrayMenuPopup
{
private:
	HWND m_hWnd;
	std::vector<std::reference_wrapper<TrayMenuItem>> m_items;
	HMENU m_hMenu;
	HMENU m_hSubMenu;

public:
	TrayMenuPopup(const HWND hWnd, const std::vector<std::reference_wrapper<TrayMenuItem>>& items);
	~TrayMenuPopup();
	void Attach(TrayMenuItem& item) noexcept;
	void Track() const noexcept;
};

