using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrayIcon
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ClickHandler();
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TrayMenuItemClickHandler(IntPtr sender, uint id);

    [StructLayout(LayoutKind.Sequential)]
    internal readonly unsafe struct TrayMenuHandle
    {
        private readonly int dummy;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly unsafe struct TrayMenuItemHandle
    {
        private readonly int dummy;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly unsafe struct TrayMenuSeparatorHandle
    {
        private readonly int dummy;
    }

    internal sealed unsafe class NativeMethods
    {
        private const string SimpleTrayIcon = "SimpleTrayIcon.dll";

        public static IntPtr TrayMenuCreate(IntPtr hIcon, string tip, delegate* unmanaged[Stdcall]<IntPtr, void> onDoubleClick)
        {
            Span<char> buffer = stackalloc char[128];
            tip.AsSpan().CopyTo(buffer);
            TrayMenuHandle* pInstance;
            fixed (char* pointer = buffer)
            {
                Marshal.ThrowExceptionForHR(
                    TrayMenuCreate(hIcon,
                                   pointer,
                                   (delegate* unmanaged[Stdcall]<TrayMenuHandle*, void>)onDoubleClick,
                                   &pInstance));
            }
            return (IntPtr)pInstance;
        }

        public static void TrayMenuRelease(ref IntPtr pInstance)
        {
            TrayMenuHandle** pointer = (TrayMenuHandle**)Unsafe.AsPointer(ref pInstance);
            Marshal.ThrowExceptionForHR(TrayMenuRelease(pointer));
        }

        public static void TrayMenuShow(IntPtr pInstance)
        {
            TrayMenuHandle* pointer = (TrayMenuHandle*)pInstance;
            Marshal.ThrowExceptionForHR(TrayMenuShow(pointer));
        }

        public static void TrayMenuClose(IntPtr pInstance)
        {
            TrayMenuHandle* pointer = (TrayMenuHandle*)pInstance;
            Marshal.ThrowExceptionForHR(TrayMenuClose(pointer));
        }

        public static void TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem)
        {
            TrayMenuHandle* pointer = (TrayMenuHandle*)pInstance;
            TrayMenuItemHandle* menuItemPointer = (TrayMenuItemHandle*)pTrayMenuItem;
            Marshal.ThrowExceptionForHR(TrayMenuAdd(pointer, menuItemPointer));
        }

        public static void TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem)
        {
            TrayMenuHandle* pointer = (TrayMenuHandle*)pInstance;
            TrayMenuItemHandle* menuItemPointer = (TrayMenuItemHandle*)pTrayMenuItem;
            Marshal.ThrowExceptionForHR(TrayMenuRemove(pointer, menuItemPointer));
        }

        public static void TrayMenuSetIcon(IntPtr pInstance, IntPtr hIcon)
        {
            TrayMenuHandle* pointer = (TrayMenuHandle*)pInstance;
            Marshal.ThrowExceptionForHR(TrayMenuSetIcon(pointer, hIcon));
        }

        public static IntPtr TrayMenuItemCreate(delegate* unmanaged[Stdcall]<IntPtr, uint, void> onClick)
        {
            TrayMenuItemHandle* pointer;
            Marshal.ThrowExceptionForHR(TrayMenuItemCreate((delegate* unmanaged[Stdcall]<TrayMenuItemHandle*, uint, void>)onClick, &pointer));
            return (IntPtr)pointer;
        }

        public static void TrayMenuItemRelease(ref IntPtr pInstance)
        {
            TrayMenuItemHandle** pointer = (TrayMenuItemHandle**)Unsafe.AsPointer(ref pInstance);
            Marshal.ThrowExceptionForHR(TrayMenuItemRelease(pointer));
        }

        public static void TrayMenuItemContent(IntPtr pInstance, string value)
        {
            fixed (char* chars = value)
            {
                TrayMenuItemHandle* pointer = (TrayMenuItemHandle*)pInstance;
                Marshal.ThrowExceptionForHR(TrayMenuItemContent(pointer, chars));
            }
        }

        public static void TrayMenuItemIsChecked(IntPtr pInstance, bool value)
        {
            TrayMenuItemHandle* pointer = (TrayMenuItemHandle*)pInstance;
            Marshal.ThrowExceptionForHR(TrayMenuItemIsChecked(pointer, value ? 1 : 0));
        }

        public static IntPtr TrayMenuSeparatorCreate()
        {
            TrayMenuSeparatorHandle* pointer;
            Marshal.ThrowExceptionForHR(TrayMenuSeparatorCreate(&pointer));
            return (IntPtr)pointer;
        }

        public static void TrayMenuSeparatorRelease(ref IntPtr pInstance)
        {
            TrayMenuSeparatorHandle** pointer = (TrayMenuSeparatorHandle**)Unsafe.AsPointer(ref pInstance);
            Marshal.ThrowExceptionForHR(TrayMenuSeparatorRelease(pointer));
        }

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuCreate(IntPtr hIcon, char* tip, delegate* unmanaged[Stdcall]<TrayMenuHandle*, void> onDoubleClick, TrayMenuHandle** pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuRelease(TrayMenuHandle** pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuShow(TrayMenuHandle* pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuClose(TrayMenuHandle* pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuAdd(TrayMenuHandle* pInstance, TrayMenuItemHandle* pTrayMenuItem);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuRemove(TrayMenuHandle* pInstance, TrayMenuItemHandle* pTrayMenuItem);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuSetIcon(TrayMenuHandle* pInstance, IntPtr hIcon);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuItemCreate(delegate* unmanaged[Stdcall]<TrayMenuItemHandle*, uint, void> onClick, TrayMenuItemHandle** pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuItemRelease(TrayMenuItemHandle** pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuItemContent(TrayMenuItemHandle* pInstance, char* value);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuItemIsChecked(TrayMenuItemHandle* pInstance, int value);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuSeparatorCreate(TrayMenuSeparatorHandle** pInstance);

        [DllImport(SimpleTrayIcon, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int TrayMenuSeparatorRelease(TrayMenuSeparatorHandle** pInstance);
    }
}
