#include "pch.h"
#include "TrayMenuSubItem.h"

TrayMenuSubItem::TrayMenuSubItem() noexcept = default;

TrayMenuSubItem::~TrayMenuSubItem()
{
	Detach();
}

void TrayMenuSubItem::Attach(const HWND hWnd, const HMENU hMenu) noexcept
{
	if (hWnd == NULL) {
		return;
	}

	if (!IsMenu(hMenu)) {
		return;
	}

	Detach();
	m_hSubMenu = CreatePopupMenu();
	if (!IsMenu(m_hSubMenu)) {
		return;
	}

	auto info = MENUITEMINFO{ .cbSize = sizeof(MENUITEMINFO) };
	info.fMask = MIIM_FTYPE | MIIM_ID | MIIM_STRING | MIIM_SUBMENU;
	info.fType = MFT_STRING;
	info.wID = GetId();
	info.hSubMenu = m_hSubMenu;
	info.dwTypeData = const_cast<LPWSTR>(Content());

	if (!InsertMenuItem(hMenu, GetMenuItemCount(hMenu), TRUE, &info)) {
		DestroyMenu(m_hSubMenu);
		m_hSubMenu = NULL;
		return;
	}

	m_hWnd = hWnd;
	m_hMenu = hMenu;

	for (const auto& item : m_items) {
		item.get().Attach(hWnd, m_hSubMenu);
	}
}

void TrayMenuSubItem::Detach() noexcept
{
	for (const auto& item : m_items) {
		item.get().Detach();
	}

	// DeleteMenu destroys the attached submenu; unattached popup menus still need explicit cleanup.
	if (IsMenu(GetHMenu())) {
		TrayMenuItemBase::Detach();
	}
	else if (IsMenu(m_hSubMenu)) {
		DestroyMenu(m_hSubMenu);
	}

	m_hSubMenu = NULL;
}

LPCWSTR TrayMenuSubItem::Content() const noexcept
{
	return m_content.c_str();
}

void TrayMenuSubItem::Content(_In_ LPCWSTR value) noexcept
{
	m_content = std::wstring(value);
	RefreshIfAttached();
}

void TrayMenuSubItem::AddItem(TrayMenuItemBase& item) noexcept
{
	m_items.push_back(item);

	if (IsMenu(m_hSubMenu) && GetHWnd()) {
		item.Attach(GetHWnd(), m_hSubMenu);
	}
}

void TrayMenuSubItem::RemoveItem(TrayMenuItemBase& item) noexcept
{
	item.Detach();

	for (auto iterator = m_items.begin(); iterator != m_items.end(); ++iterator) {
		if (&iterator->get() == &item) {
			m_items.erase(iterator);
			break;
		}
	}
}

bool TrayMenuSubItem::OnCommand(const WPARAM commandId) const noexcept
{
	for (const auto& item : m_items) {
		if (item.get().OnCommand(commandId)) {
			return true;
		}
	}

	return false;
}
