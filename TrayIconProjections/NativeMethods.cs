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

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuCreate(IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string tip, out IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuRelease(ref IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuShow(IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuClose(IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem);


        public delegate void OnClicked(IntPtr sender, uint id);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuItemCreate(OnClicked onClicked, out IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuItemRelease(ref IntPtr pInstance);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport(TrayIcon, ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int TrayMenuItemIsChecked(IntPtr instance, bool value);
    }

    internal sealed class NativeMethods2
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
    }
}
