using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TrayIcon
{
    internal sealed class NativeMethods
    {
        private const string TrayIcon = "TrayIcon.dll";

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuCreate(IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string tip);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuRelease(ref IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuShow(IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuClose(IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem);


        public delegate void OnClicked(IntPtr sender, uint id);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuItemCreate(OnClicked onClicked);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemRelease(ref IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuItemIsChecked(IntPtr instance, bool value);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IntPtr TrayMenuSeparatorCreate();

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void TrayMenuSeparatorRelease(ref IntPtr pInstance);
    }
}
