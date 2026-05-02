// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

public delegate void DoubleClickHandler(IntPtr obj);
public class SimpleTrayIconApi
{
    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuCreate(IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string tip, DoubleClickHandler onDoubleClick, out IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuRelease(ref IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuShow(IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuClose(IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem);


    public delegate void OnClicked(IntPtr sender, uint id);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemCreate(OnClicked onClicked, out IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemRelease(ref IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemIsChecked(IntPtr instance, bool value);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuSubItemCreate(out IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuSubItemRelease(ref IntPtr pInstance);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuSubItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuSubItemAdd(IntPtr instance, IntPtr pTrayMenuItem);

    [DllImport("SimpleTrayIcon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuSubItemRemove(IntPtr instance, IntPtr pTrayMenuItem);
}