using System;

namespace SimpleTrayIcon
{
    public abstract class TrayMenuItemBase
    {
        private IntPtr _hInstance;
        internal IntPtr HInstance => _hInstance;
        protected ref IntPtr HInstanceRef => ref _hInstance;
    }
}
