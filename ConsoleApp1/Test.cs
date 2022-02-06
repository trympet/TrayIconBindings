// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

public class Test
{
    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuCreate(IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string tip, out IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuRelease(ref IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuShow(IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuClose(IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuAdd(IntPtr pInstance, IntPtr pTrayMenuItem);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuRemove(IntPtr pInstance, IntPtr pTrayMenuItem);


    public delegate void OnClicked(IntPtr sender, uint id);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemCreate(OnClicked onClicked, out IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemRelease(ref IntPtr pInstance);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemContent(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport("Dll1.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int TrayMenuItemIsChecked(IntPtr instance, bool value);
}