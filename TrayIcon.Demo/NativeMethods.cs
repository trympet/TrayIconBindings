using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436
namespace TrayIcon.Demo
{
    internal class NativeMethods
    {
        internal static unsafe void RunLoop()
        {
            MSG msg = default;
            var lpMsg = &msg;
            while (GetMessage(lpMsg, default, 0, 0))
            {
                TranslateMessage(lpMsg);
                DispatchMessage(lpMsg);
            }
        }

        [DllImport("User32", ExactSpelling = true, EntryPoint = "GetMessageW", SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern unsafe bool GetMessage(MSG* lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("User32", ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern unsafe int TranslateMessage(MSG* lpMsg);

        [DllImport("User32", ExactSpelling = true, EntryPoint = "DispatchMessageW")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern unsafe nint DispatchMessage(MSG* lpMsg);

        [StructLayout(LayoutKind.Sequential)]
        internal ref struct MSG
        {
            internal IntPtr hwnd;
            internal uint message;
            internal uint wParam;
            internal uint lParam;
            internal uint time;
            internal int ptX;
            internal int ptY;
        }
    }
}
