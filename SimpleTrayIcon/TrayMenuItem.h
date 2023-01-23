#pragma once
#include "TrayMenuItemBase.h"

#define BASE_COMMAND_ID 40000

class TrayMenuItem;

typedef void(CALLBACK* TrayMenuItemClickHandler)(const TrayMenuItem*, UINT);

class TrayMenuItem : public TrayMenuItemBase
{
private:
	TrayMenuItemClickHandler m_onClicked;
	wchar_t m_content[128]{};
	BOOL m_isChecked = false;

public:
	TrayMenuItem(const TrayMenuItemClickHandler onClicked) noexcept;
	LPCWSTR Content() const noexcept;
	void Content(const LPWSTR& value) noexcept;
	void IsChecked(const BOOL value) noexcept;
	void OnCommand(const WPARAM commandId) const noexcept;

protected:
	UINT GetFlags() const noexcept;
};

