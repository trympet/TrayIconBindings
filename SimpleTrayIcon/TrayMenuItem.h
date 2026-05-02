#pragma once
#include "TrayMenuItemBase.h"

#define BASE_COMMAND_ID 40000

class TrayMenuItem;

typedef void(CALLBACK* TrayMenuItemClickHandler)(const TrayMenuItem*, UINT);

class TrayMenuItem : public TrayMenuItemBase
{
private:
	TrayMenuItemClickHandler m_onClicked;
	std::wstring m_content;
	BOOL m_isChecked = false;

public:
	TrayMenuItem(const TrayMenuItemClickHandler onClicked) noexcept;
	LPCWSTR Content() const noexcept;
	void Content(_In_ LPCWSTR value) noexcept;
	void IsChecked(const BOOL value) noexcept;
	bool OnCommand(const WPARAM commandId) const noexcept override;

protected:
	UINT GetFlags() const noexcept;
};

