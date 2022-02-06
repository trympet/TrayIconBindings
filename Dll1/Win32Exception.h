#pragma once
class Win32Exception : public std::runtime_error
{
private:
	HRESULT m_hr;

public:
	Win32Exception(const std::string& message) : std::runtime_error(message) {
		m_hr = HRESULT_FROM_WIN32(GetLastError());
	}
	Win32Exception() : Win32Exception("") {}

	constexpr HRESULT GetHR() const noexcept { return m_hr; }
};

