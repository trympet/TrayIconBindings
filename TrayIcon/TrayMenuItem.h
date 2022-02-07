#pragma once
#define BASE_COMMAND_ID 40000

class TrayMenuItem;

typedef void(CALLBACK* TrayMenuItemClicked)(const TrayMenuItem*, UINT);

class TrayMenuItem
{
private:
	static UINT s_nextCommandId;
	UINT m_commandId;
	HWND m_hWnd = NULL;
	HMENU m_hMenu = NULL;
	TrayMenuItemClicked m_onClicked;
	wchar_t m_content[128]{};
	BOOL m_isChecked = false;

public:
	TrayMenuItem(const TrayMenuItemClicked onClicked) noexcept;
	~TrayMenuItem() noexcept;
	void Content(const LPWSTR& value) noexcept;
	void IsChecked(const BOOL value) noexcept;
	void Attach(const HWND hWnd, const HMENU hMenu) noexcept;
	void Detach() noexcept;
	void OnCommand(const WPARAM commandId) const noexcept;

private:
	constexpr UINT GetFlags() const noexcept;
	void RefreshIfAttached();
};

