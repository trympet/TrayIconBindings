#include "pch.h"
#include "TrayMenuItem.h"

TrayMenuItem::TrayMenuItem(const TrayMenuItemClickHandler onClicked) noexcept
{
	m_onClicked = onClicked;
}

LPCWSTR TrayMenuItem::Content() const noexcept
{
	return m_content.c_str();
}

void TrayMenuItem::Content(_In_ LPCWSTR value) noexcept
{
	m_content = std::wstring(value);
	RefreshIfAttached();
}

void TrayMenuItem::IsChecked(const BOOL value) noexcept
{
	m_isChecked = value;
	RefreshIfAttached();
}

void TrayMenuItem::IsCheckable(const BOOL value) noexcept
{
	m_isCheckable = value;
	RefreshIfAttached();
}

bool TrayMenuItem::OnCommand(const WPARAM commandId) const noexcept
{
	if (commandId == GetId()) {
		if (m_isCheckable) {
			m_onClicked(this, GetId());
		}
		return true;
	}

	return false;
}

UINT TrayMenuItem::GetFlags() const noexcept
{
	UINT result = MF_STRING;
	if (m_isChecked) {
		result |= MF_CHECKED;
	}
	if (!m_isCheckable) {
		result |= MF_GRAYED | MF_DISABLED;
	}
	return result;
}
