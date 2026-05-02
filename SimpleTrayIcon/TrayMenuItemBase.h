#pragma once

#define BASE_ITEM_ID 40000

class TrayMenuItemBase
{
private:
	static UINT s_nextItemId;
	UINT m_itemId;

protected:
	HWND m_hWnd = NULL;
	HMENU m_hMenu = NULL;

public:
	TrayMenuItemBase();
	~TrayMenuItemBase();
	virtual void Attach(const HWND hWnd, const HMENU hMenu) noexcept;
	virtual void Detach() noexcept;
	void RefreshIfAttached() noexcept;
	virtual LPCWSTR Content() const noexcept;
	virtual void OnCommand([[maybe_unused]] const WPARAM commandId) const noexcept {};

protected:
	virtual UINT GetFlags() const noexcept;

	UINT GetId() const noexcept {
		return m_itemId;
	}

	HWND GetHWnd() const noexcept {
		return m_hWnd;
	}

	HMENU GetHMenu() const noexcept {
		return m_hMenu;
	}
};

