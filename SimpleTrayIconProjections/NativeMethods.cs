using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrayIcon
{
    internal sealed class NativeMethods
    {
        private const string SimpleTrayIcon = "SimpleTrayIcon.dll";

        public delegate void ClickHandler();
        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuCreate(IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string tip, ClickHandler onDoubleClick);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuRelease(ref IntPtr pInstance);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuShow(IntPtr pInstance);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuClose(IntPtr pInstance);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuSetIcon(IntPtr pInstance, IntPtr hIcon);


        public delegate void TrayMenuItemClickHandler(IntPtr sender, uint id);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuItemCreate(TrayMenuItemClickHandler onClick);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemRelease(ref IntPtr pInstance);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemIsChecked(IntPtr instance, bool value);

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuSeparatorCreate();

        [DllImport(SimpleTrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuSeparatorRelease(ref IntPtr pInstance);
    }
}
