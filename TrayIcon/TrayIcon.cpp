#include "pch.h"
#include "TrayIcon.h"

#define WM_TRAYNOTIFY L"WM_IconNotify"

EXTERN_C static IMAGE_DOS_HEADER __ImageBase;

// {ECEAEC05-4D8B-4744-94A2-54859B6C3B70}
static constexpr GUID s_trayIconGuid =
{ 0xeceaec05, 0x4d8b, 0x4744, { 0x94, 0xa2, 0x54, 0x85, 0x9b, 0x6c, 0x3b, 0x70 } };

TrayIcon::TrayIcon(const HICON hIcon, const LPWSTR tip) noexcept
{
	m_iconNotifyWm = RegisterWindowMessage(WM_TRAYNOTIFY);

	const WNDCLASS wc = {
		.style = CS_HREDRAW | CS_VREDRAW,
		.lpfnWndProc = WndProc,
		.hInstance = reinterpret_cast<HINSTANCE>(&__ImageBase),
		.hIcon = hIcon,
		.hCursor = LoadCursor(NULL, IDC_ARROW),
		.hbrBackground = 0,
		.lpszClassName = TrayIconWndClass,
	};

	RegisterClass(&wc);
	auto hWnd = CreateWindowEx(0L,
		wc.lpszClassName,
		TrayIconWndClass,
		WS_OVERLAPPEDWINDOW | WS_POPUP,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		NULL,
		NULL,
		wc.hInstance,
		this);

	m_trayIconData.cbSize = sizeof(NOTIFYICONDATA);
	m_trayIconData.hIcon = hIcon;
	m_trayIconData.hWnd = hWnd;
	m_trayIconData.guidItem = s_trayIconGuid;
	m_trayIconData.uVersion = NOTIFYICON_VERSION_4;
	//m_trayIconData.uID = m_iconNotifyWm;
	m_trayIconData.uCallbackMessage = m_iconNotifyWm;
	wcscpy_s(m_trayIconData.szTip, sizeof(m_trayIconData.szTip) / sizeof(WCHAR), tip);
	m_trayIconData.uFlags = NIF_ICON | NIF_SHOWTIP | NIF_MESSAGE | NIF_GUID;
	ChangeWindowMessageFilterEx(hWnd, WM_COMMAND, MSGFLT_ALLOW, NULL);

	if (!m_trayIconCreated) {
		m_trayIconCreated = Shell_NotifyIcon(NIM_ADD, &m_trayIconData);
	}
}

TrayIcon::~TrayIcon()
{
	if (m_trayIconHwnd) {
		SendMessageTimeout(m_trayIconHwnd, WM_CLOSE, 0, 0, SMTO_ABORTIFHUNG | SMTO_BLOCK, 3999, NULL);
	}

	delete m_trayMenuPopup;
}

void TrayIcon::AddItem(TrayMenuItem& item) noexcept
{
	if (m_trayMenuPopup != NULL) {
		m_trayMenuPopup->Attach(item);
	}

	m_items.push_back(item);
}

void TrayIcon::RemoveItem(TrayMenuItem& item)
{
	if (m_trayMenuPopup != NULL) {
		item.Detach();
	}

	size_t i = 0;
	for (; i < m_items.size(); i++)
	{
		if (&m_items[i].get() == &item) {
			break;
		}
	}

	if (i != m_items.size()) {
		m_items.erase(m_items.begin() + i);
	}
}

LRESULT CALLBACK TrayIcon::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) noexcept
{
	if (message != WM_NCCREATE) {
		const auto instance = GetInstance(hWnd);
		if (instance) {
			return instance->TrayIconWndProc(hWnd, message, wParam, lParam);
		}
	}
	else {
		auto pCs = (CREATESTRUCT*)lParam;
		auto pInstance = (TrayIcon*)pCs->lpCreateParams;
		pInstance->m_trayIconHwnd = hWnd;
		SetWindowLongPtr(hWnd, GWLP_USERDATA, (LONG_PTR)pInstance);
	}

	return DefWindowProc(hWnd, message, wParam, lParam);
}

inline TrayIcon* TrayIcon::GetInstance(const HWND hWnd)
{
	return (TrayIcon*)GetWindowLongPtr(hWnd, GWLP_USERDATA);
}

LRESULT TrayIcon::TrayIconWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
	switch (message) {
	case WM_CREATE:
		if (m_taskbarRestartWm == 0) {
			m_trayIconHwnd = hWnd;
			m_taskbarRestartWm = RegisterWindowMessage(L"TaskbarCreated");
		}
		break;

	case WM_DESTROY:
		if (m_trayIconCreated) {
			Shell_NotifyIcon(NIM_DELETE, &m_trayIconData);
			m_trayIconCreated = false;
		}

		PostQuitMessage(0);
		break;

	case WM_CLOSE:
		DestroyWindow(hWnd);
		break;

	case WM_COMMAND:
		for (const auto x : m_items) {
			x.get().OnCommand(wParam);
		}
		break;

	case WM_WINDOWPOSCHANGING:
		if (!m_trayIconCreated) {
			m_trayIconCreated = Shell_NotifyIcon(NIM_ADD, &m_trayIconData);
		}
		break;
	case WM_PAINT:
		break;
	case WM_ENTERMENULOOP:
		break;
	default:
		if (message == m_iconNotifyWm) {
			switch (lParam) {
			case WM_LBUTTONUP:
				break;
			case WM_RBUTTONUP:
			case WM_CONTEXTMENU:
				if (m_trayMenuPopup == NULL) {
					m_trayMenuPopup = new TrayMenuPopup(hWnd, m_items);
				}

				m_trayMenuPopup->Track();
				break;
			}
		}
		else if (message == m_taskbarRestartWm) {
			m_trayIconCreated = Shell_NotifyIcon(NIM_ADD, &m_trayIconData);
		}

		break;
	}

	return DefWindowProc(hWnd, message, wParam, lParam);
}
