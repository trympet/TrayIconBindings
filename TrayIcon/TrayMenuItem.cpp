#include "pch.h"
#include "TrayMenuItem.h"

TrayMenuItem::TrayMenuItem(const TrayMenuItemClicked onClicked) noexcept
{
	m_onClicked = onClicked;
}

LPCWSTR TrayMenuItem::Content() const noexcept
{
	return m_content;
}

void TrayMenuItem::Content(const LPWSTR& value) noexcept
{
	wcscpy_s(m_content, value);
	RefreshIfAttached();
}

void TrayMenuItem::IsChecked(const BOOL value) noexcept
{
	m_isChecked = value;
	RefreshIfAttached();
}

void TrayMenuItem::OnCommand(const WPARAM commandId) const noexcept
{
	if (commandId == GetId()) {
		m_onClicked(this, GetId());
	}
}

UINT TrayMenuItem::GetFlags() const noexcept
{
	UINT result = MF_STRING;
	if (m_isChecked) {
		result |= MF_CHECKED;
	}
	return result;
}
