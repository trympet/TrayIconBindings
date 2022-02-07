// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Runtime.InteropServices;
using static Windows.Win32.PInvoke;
#pragma warning disable CA1416 // Validate platform compatibility

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Console.WriteLine("Hello, World!");

        //var resourceStream = typeof(Test).Assembly.GetManifestResourceStream("TrayIcon.Demo.tray-icon.ico");
        var icon = new Icon(typeof(Test), "TrayIcon.Demo.tray-icon.ico");
        var hIcon = icon.Handle;

        Test.TrayMenuCreate(hIcon, "tip", out var hMenu);

        Test.TrayMenuItemCreate((s, e) =>
        {
            Console.WriteLine("Clicked1");
        }, out var hItem1);

        Test.TrayMenuItemCreate((s, e) =>
        {
            Console.WriteLine("Clicked2");
        }, out var hItem2);

        var item3Checked = false;
        string item3Content = "a";
        Test.TrayMenuItemCreate((s, e) =>
        {
            Test.TrayMenuItemIsChecked(s, item3Checked = !item3Checked);
            item3Content += (char)(item3Content.Last() + 1);
            Test.TrayMenuItemContent(s, item3Content);
            Console.WriteLine("Clicked3");
        }, out var hItem3);

        Test.TrayMenuItemContent(hItem1, "item1");
        Test.TrayMenuItemContent(hItem2, "item2");
        Test.TrayMenuItemContent(hItem3, item3Content);

        Test.TrayMenuAdd(hMenu, hItem1);
        Test.TrayMenuAdd(hMenu, hItem2);
        Test.TrayMenuAdd(hMenu, hItem3);

        Test.TrayMenuShow(hMenu);

        while (GetMessage(out var msg, default, 0, 0))
        {
            TranslateMessage(in msg);
            DispatchMessage(in msg);
        }


        Thread.Sleep(1111);
    }
}
