#include "pch.h"
#include "TrayIconManager.h"

TrayIconDataWrapper& TrayIconManager::Create() noexcept
{
	m_data.push_back(TrayIconDataWrapper());
	return m_data.back();
}

void TrayIconManager::Free(TrayIconDataWrapper& data) noexcept
{
	data.TryFree();
}

void TrayIconManager::Cleanup() noexcept
{
	for (auto& item : m_data) {
		if (item.Exists()) {
			Shell_NotifyIcon(NIM_DELETE, &item.get());
		}
	}
}
