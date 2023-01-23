#include "pch.h"
#include "TrayIcon.h"

#define WM_TRAYNOTIFY L"WM_IconNotify"
#define WM_TRAYMOUSEMESSAGE (WM_USER + 420)
static const UINT WM_TASKBARCREATED = RegisterWindowMessageW(L"TaskbarCreated");
static const wchar_t TrayIconWndClass[] = L"TrayIconWnd";

EXTERN_C static IMAGE_DOS_HEADER __ImageBase;

static volatile UINT s_nextId = 0;

TrayIcon::TrayIcon(const HICON hIcon, const LPWSTR tip, const std::function<void()> onDoubleClick)
	: m_trayIconData(TrayIconManager::Create()), m_hIcon(CopyIcon(hIcon))
{
	m_onDoubleClick = onDoubleClick;

	const WNDCLASS wc = {
		.style = CS_HREDRAW | CS_VREDRAW,
		.lpfnWndProc = WndProc,
		.hInstance = reinterpret_cast<HINSTANCE>(&__ImageBase),
		.hCursor = LoadCursor(NULL, IDC_ARROW),
		.hbrBackground = 0,
		.lpszClassName = TrayIconWndClass,
	};

	if (!RegisterClass(&wc)) {
		throw Win32Exception("Failed to register tray menu class.");
	}

	auto hWnd = m_trayIconHwnd = CreateWindowEx(0L,
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

	if (hWnd == NULL) {
		throw Win32Exception("Failed to create tray menu window.");
	}

	NOTIFYICONDATA trayIconData{};
	trayIconData.cbSize = sizeof(NOTIFYICONDATA);
	trayIconData.hIcon = m_hIcon;
	trayIconData.hWnd = hWnd;
	trayIconData.uID = InterlockedIncrement(&s_nextId);
	trayIconData.uCallbackMessage = WM_TRAYMOUSEMESSAGE;
	wcscpy_s(trayIconData.szTip, sizeof(trayIconData.szTip) / sizeof(WCHAR), tip);
	trayIconData.uFlags = NIF_ICON | NIF_TIP | NIF_MESSAGE;
	ChangeWindowMessageFilterEx(hWnd, WM_COMMAND, MSGFLT_ALLOW, NULL);

	if (!Shell_NotifyIconW(NIM_ADD, &trayIconData)) {
		throw Win32Exception("Failed to notify shell about icon.");
	}

	m_trayIconData->emplace(trayIconData);
}

TrayIcon::~TrayIcon()
{
	if (m_trayIconHwnd) {
		SendMessageTimeout(m_trayIconHwnd, WM_CLOSE, 0, 0, SMTO_ABORTIFHUNG | SMTO_BLOCK, 3999, NULL);
		DestroyWindow(m_trayIconHwnd);
	}
	DestroyIcon(m_hIcon);
}

void TrayIcon::AddItem(TrayMenuItemBase& item) noexcept
{
	if (m_trayMenuPopup) {
		m_trayMenuPopup->get()->Attach(item);
	}

	m_items.push_back(item);
}

void TrayIcon::RemoveItem(TrayMenuItemBase& item)
{
	if (m_trayMenuPopup) {
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

void TrayIcon::SetIcon(const HICON hIcon) noexcept
{
	if (hIcon != m_hIcon) {
		DestroyIcon(m_hIcon);
		m_hIcon = CopyIcon(hIcon);
	}

	if (!m_trayIconData->has_value()) {
		return;
	}

	auto& trayIconData = m_trayIconData->value();
	trayIconData.hIcon = m_hIcon;

	Shell_NotifyIcon(NIM_MODIFY, &trayIconData);
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
	case WM_DESTROY:
		if (m_trayIconData->has_value()) {
			m_trayIconData->reset();
		}

		m_trayIconData->reset();
		PostQuitMessage(0);
		break;

	case WM_CLOSE:
		DestroyWindow(hWnd);
		break;

	case WM_COMMAND:
		for (size_t i = 0; i < m_items.size(); i++)
		{
			m_items.at(i).get().OnCommand(wParam);
		}
		break;
	case WM_TRAYMOUSEMESSAGE:
		switch (lParam) {
		case WM_RBUTTONUP:
		case WM_CONTEXTMENU:
			if (!m_trayMenuPopup) {
				m_trayMenuPopup.emplace(std::make_unique<TrayMenuPopup>(hWnd, m_items));
			}
			m_trayMenuPopup->get()->Track();
			break;
		case WM_LBUTTONDBLCLK:
			if (m_onDoubleClick != NULL) {
				m_onDoubleClick();
			}
			break;
		}
		break;
	case WM_INITMENUPOPUP:
		if (!m_trayMenuPopup) {
			m_trayMenuPopup.emplace(std::make_unique<TrayMenuPopup>(hWnd, m_items));
		}
		break;
	default:
		if (message == WM_TASKBARCREATED && m_trayIconData->has_value()) {
			Shell_NotifyIcon(NIM_ADD, &m_trayIconData->value());
		}
		break;
	}

	return DefWindowProc(hWnd, message, wParam, lParam);
}
