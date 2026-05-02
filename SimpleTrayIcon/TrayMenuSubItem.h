#pragma once

#include "TrayMenuItemBase.h"

class TrayMenuSubItem : public TrayMenuItemBase
{
private:
	std::wstring m_content;
	std::vector<std::reference_wrapper<TrayMenuItemBase>> m_items;
	HMENU m_hSubMenu = NULL;

public:
	TrayMenuSubItem() noexcept;
	~TrayMenuSubItem();
	void Attach(const HWND hWnd, const HMENU hMenu) noexcept override;
	void Detach() noexcept override;
	LPCWSTR Content() const noexcept override;
	void Content(_In_ LPCWSTR value) noexcept;
	void AddItem(TrayMenuItemBase& item) noexcept;
	void RemoveItem(TrayMenuItemBase& item) noexcept;
	void OnCommand(const WPARAM commandId) const noexcept override;
};
