#pragma once
#include <memory>
#include <optional>

class TrayIconDataWrapper {
private:
	std::optional<NOTIFYICONDATA> m_item;
public:
	TrayIconDataWrapper() = default;
	~TrayIconDataWrapper() {
		reset();
	}

	constexpr NOTIFYICONDATA& emplace(NOTIFYICONDATA& value) {
		return m_item.emplace(value);
	}

	constexpr void reset() {
		if (m_item) {
			try
			{
				Shell_NotifyIcon(NIM_DELETE, &m_item.value());
				m_item.reset();
			}
			catch (const std::bad_optional_access&)
			{
				// Not thread safe, but don't propagate error if we fail.
			}
		}
	}

	constexpr bool has_value() {
		return m_item.has_value();
	}

	_NODISCARD constexpr NOTIFYICONDATA& value() noexcept {
		return m_item.value();
	}
};


class TrayIconManager
{
public:
	// Returns a new empty struct.
	static std::shared_ptr<TrayIconDataWrapper> Create() noexcept;
	// Notifies the shell that we're finished; prevents lingering icons.
	static void Cleanup() noexcept;
};
