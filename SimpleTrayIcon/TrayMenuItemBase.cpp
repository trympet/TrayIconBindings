#include "pch.h"
#include "TrayMenuItemBase.h"

TrayMenuItemBase::TrayMenuItemBase()
{
	m_itemId = InterlockedIncrement(&s_nextItemId);
}

TrayMenuItemBase::~TrayMenuItemBase()
{
	Detach();
}

void TrayMenuItemBase::Attach(const HWND hWnd, const HMENU hMenu) noexcept
{
	if (hWnd == NULL) {
		return;
	}

	if (hMenu == NULL) {
		return;
	}

	Detach();
	AppendMenu(hMenu, GetFlags(), m_itemId, Content());

	m_hWnd = hWnd;
	m_hMenu = hMenu;
}

void TrayMenuItemBase::Detach() noexcept
{
	if (m_hMenu) {
		DeleteMenu(m_hMenu, m_itemId, MF_BYCOMMAND);
		m_hMenu = NULL;
	}

	m_hWnd = NULL;
}

void TrayMenuItemBase::RefreshIfAttached() noexcept
{
	if (m_hMenu) {
		auto info = MENUITEMINFO{ .cbSize = sizeof(MENUITEMINFO) };
		GetMenuItemInfo(m_hMenu, m_itemId, false, &info);
		info.dwTypeData = (LPWSTR)Content();
		info.fMask |= MIIM_FTYPE | MIIM_STATE;
		info.fState = GetFlags();
		SetMenuItemInfo(m_hMenu, m_itemId, false, &info);
		DrawMenuBar(m_hWnd);
	}
}

UINT TrayMenuItemBase::GetFlags() const noexcept
{
	return 0;
}

LPCWSTR TrayMenuItemBase::Content() const noexcept
{
	return NULL;
}

UINT TrayMenuItemBase::s_nextItemId{ BASE_ITEM_ID };
