#include "pch.h"
#include "TrayMenuItem.h"

TrayMenuItem::TrayMenuItem(const TrayMenuItemClicked onClicked) noexcept
{
	m_onClicked = onClicked;
	m_commandId = InterlockedIncrement(&s_nextCommandId);
}

TrayMenuItem::~TrayMenuItem()
{
	Detach();
}

void TrayMenuItem::Content(const LPWSTR& value) noexcept
{
	wcscpy_s(m_content, value);
	RefreshIfAttached();
}

void TrayMenuItem::Attach(const HWND hWnd, const HMENU hMenu) noexcept
{
	if (hWnd == NULL) {
		return;
	}

	if (hMenu == NULL) {
		return;
	}

	Detach();
	AppendMenu(hMenu, GetFlags(), m_commandId, m_content);

	m_hWnd = hWnd;
	m_hMenu = hMenu;
}

void TrayMenuItem::Detach() noexcept
{
	if (m_hMenu) {
		DeleteMenu(m_hMenu, m_commandId, MF_BYCOMMAND);
		m_hMenu = NULL;
	}

	m_hWnd = NULL;
}

void TrayMenuItem::IsChecked(const BOOL value) noexcept
{
	m_isChecked = value;
	RefreshIfAttached();
}

void TrayMenuItem::OnCommand(const WPARAM commandId) const noexcept
{
	if (commandId == m_commandId) {
		m_onClicked(this, m_commandId);
	}
}

constexpr UINT TrayMenuItem::GetFlags() const noexcept
{
	UINT result = MF_STRING;
	if (m_isChecked) {
		result |= MF_CHECKED;
	}
	return result;
}

void TrayMenuItem::RefreshIfAttached() {
	if (m_hMenu) {
		Attach(m_hWnd, m_hMenu);
		DrawMenuBar(m_hWnd);
	}
}

UINT TrayMenuItem::s_nextCommandId{ BASE_COMMAND_ID };
