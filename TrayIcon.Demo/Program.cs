// See https://aka.ms/new-console-template for more information
using System.Drawing;

#pragma warning disable CA1416 // Validate platform compatibility
namespace TrayIcon.Demo
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            char? c = default;
            while (c == default)
            {
                Console.WriteLine("1) Managed demo");
                Console.WriteLine("2) PInvoke demo");
                var input = Console.ReadLine();
                switch (c = input?.FirstOrDefault())
                {
                    case '1':
                        ManagedDemo();
                        break;
                    case '2':
                        PInvokeDemo();
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        c = default;
                        continue;
                }
            }
        }

        private static void ManagedDemo()
        {
            var icon = new Icon(typeof(TrayIconApi), "TrayIcon.Demo.tray-icon.ico");
            var menu = new TrayMenu(icon, "Tooltip", true);
            var item1 = new TrayMenuItem { Content = "Item1" };

            int itemNumber = 1;
            void OnClicked(object? sender, EventArgs e)
            {
                if (sender is TrayMenuItem item)
                {
                    item.IsChecked = !item.IsChecked;
                    Console.WriteLine($"{item.Content} clicked.");
                    var newItem = new TrayMenuItem { Content = $"Item{++itemNumber}" };
                    newItem.Click += OnClicked;
                    menu.Items.Add(newItem);
                }
            }

            item1.Click += OnClicked;
            menu.Items.Add(item1);

            NativeMethods.RunLoop();
        }

        private static void PInvokeDemo()
        {
            var icon = new Icon(typeof(TrayIconApi), "TrayIcon.Demo.tray-icon.ico");
            var hIcon = icon.Handle;

            TrayIconApi.TrayMenuCreate(hIcon, "tip", out var hMenu);

            TrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                Console.WriteLine("Clicked1");
            }, out var hItem1);

            TrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                Console.WriteLine("Clicked2");
            }, out var hItem2);

            var item3Checked = false;
            string item3Content = "a";
            TrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                TrayIconApi.TrayMenuItemIsChecked(s, item3Checked = !item3Checked);
                item3Content += (char)(item3Content.Last() + 1);
                TrayIconApi.TrayMenuItemContent(s, item3Content);
                Console.WriteLine("Clicked3");
            }, out var hItem3);

            TrayIconApi.TrayMenuItemContent(hItem1, "item1");
            TrayIconApi.TrayMenuItemContent(hItem2, "item2");
            TrayIconApi.TrayMenuItemContent(hItem3, item3Content);

            TrayIconApi.TrayMenuAdd(hMenu, hItem1);
            TrayIconApi.TrayMenuAdd(hMenu, hItem2);
            TrayIconApi.TrayMenuAdd(hMenu, hItem3);

            TrayIconApi.TrayMenuShow(hMenu);

            NativeMethods.RunLoop();
        }
    }
}
