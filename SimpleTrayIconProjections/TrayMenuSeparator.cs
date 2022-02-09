using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SimpleTrayIcon.NativeMethods;

namespace SimpleTrayIcon
{
    public class TrayMenuSeparator : TrayMenuItemBase, IDisposable
    {
        private bool disposedValue;

        public TrayMenuSeparator()
        {
            HInstanceRef = TrayMenuSeparatorCreate();
        }

        ~TrayMenuSeparator()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _ = disposing;

                if (HInstanceRef != IntPtr.Zero)
                {
                    TrayMenuSeparatorRelease(ref HInstanceRef);
                    HInstanceRef = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }
    }
}
