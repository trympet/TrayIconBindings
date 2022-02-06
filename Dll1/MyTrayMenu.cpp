
#include "pch.h"
#include "TrayMenuItem.h"
#include "TrayIcon.h"
#include "MyTrayMenu.h"

using namespace std;

MyTrayMenu::MyTrayMenu(const HICON hIcon, const LPWSTR tip) noexcept
{
	m_hIcon = hIcon;
	wcscpy_s(m_tip, tip);
}

MyTrayMenu::~MyTrayMenu() noexcept
{
	Close();
}

void MyTrayMenu::Show() noexcept
{
	if (!m_trayIcon) {
		m_trayIcon = new TrayIcon(m_hIcon, m_tip);
	}

	while (!m_items.empty()) {
		const auto& item = m_items.front();
		m_items.pop();
		AddItem(item);
	}
}

void MyTrayMenu::AddItem(TrayMenuItem& pTrayMenuItem) noexcept
{
	if (m_trayIcon) {
		m_trayIcon->AddItem(pTrayMenuItem);
		m_addedItems.push(pTrayMenuItem);
	}
	else {
		m_items.push(pTrayMenuItem);
	}
}

void MyTrayMenu::RemoveItem(TrayMenuItem& pTrayMenuItem) noexcept
{
	if (m_trayIcon) {
		m_trayIcon->RemoveItem(pTrayMenuItem);
		RemoveItem(m_addedItems, pTrayMenuItem);
	}
	else {
		RemoveItem(m_items, pTrayMenuItem);
	}
}

void MyTrayMenu::Close()
{
	if (m_trayIcon) {
		delete m_trayIcon;
		m_trayIcon = NULL;
	}
}

BOOL MyTrayMenu::RemoveItem(std::queue<std::reference_wrapper<TrayMenuItem>>& pQueue, const TrayMenuItem& pItem) {
	const auto max = pQueue.size();
	int i = 0;
	while (!pQueue.empty()) {
		const auto& current = pQueue.front();
		if (&current.get() == &pItem) {
			pQueue.pop();
			break;
		}

		pQueue.pop();
		pQueue.push(current);

		if (i == max) {
			return FALSE;
		}
	}

	return TRUE;
}