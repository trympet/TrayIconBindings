#pragma once

class TrayIconDataWrapper {
private:
	NOTIFYICONDATA* m_item = NULL;
public:
	constexpr operator NOTIFYICONDATA& () const noexcept {
		return *m_item;
	}

	NOTIFYICONDATA& Create() noexcept {
		return *(m_item = new NOTIFYICONDATA{});
	}

	_NODISCARD constexpr NOTIFYICONDATA& get() const noexcept {
		return *m_item;
	}

	constexpr bool Exists() const noexcept { return m_item != NULL; }

	void TryFree() noexcept {
		if (Exists()) {
			delete m_item;
			m_item = NULL;
		}
	}
};


class TrayIconManager
{
private:
	inline static std::vector<TrayIconDataWrapper> m_data{};

public:
	// Returns a new empty struct.
	static TrayIconDataWrapper& Create() noexcept;
	static void Free(TrayIconDataWrapper& data) noexcept;
	// Notifies the shell that we're finished; prevents lingering icons.
	static void Cleanup() noexcept;
};
