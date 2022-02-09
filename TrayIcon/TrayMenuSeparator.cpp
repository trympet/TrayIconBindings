#include "pch.h"
#include "TrayMenuSeparator.h"

UINT TrayMenuSeparator::GetFlags() const noexcept
{
    return MF_SEPARATOR;
}
