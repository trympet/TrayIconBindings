#pragma once

#include "TrayMenuItemBase.h"

class TrayMenuSeparator : public TrayMenuItemBase
{
protected:
	UINT GetFlags() const noexcept;
};

