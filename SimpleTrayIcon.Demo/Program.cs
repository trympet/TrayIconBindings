// See https://aka.ms/new-console-template for more information
using System.Drawing;

#pragma warning disable CA1416 // Validate platform compatibility
namespace SimpleTrayIcon.Demo
{
    public static class Program
    {
        private static int iconId = 1;

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
            var menu = new TrayMenu(GetNextIcon(), "Tooltip", true);
            var item1 = new TrayMenuItem { Content = "Item1" };

            int itemNumber = 1;
            void OnClicked(object? sender, EventArgs e)
            {
                if (sender is TrayMenuItem item)
                {
                    menu.Icon = GetNextIcon();
                    item.IsChecked = !item.IsChecked;
                    Console.WriteLine($"{item.Content} clicked.");
                    var newItem = new TrayMenuItem { Content = $"Item{++itemNumber}" };
                    newItem.Click += OnClicked;
                    menu.Items.Add(newItem);
                    menu.Items.Add(new TrayMenuSeparator());
                }
            }

            item1.Click += OnClicked;
            menu.Items.Add(item1);

            NativeMethods.RunLoop();
        }

        private static Icon GetNextIcon()
        {
            iconId = iconId % 2 + 1;
            return new Icon(typeof(SimpleTrayIconApi), $"SimpleTrayIcon.Demo.tray-icon-{iconId}.ico");
        }

        private static void PInvokeDemo()
        {
            var icon = new Icon(typeof(SimpleTrayIconApi), "SimpleTrayIcon.Demo.tray-icon.ico");
            var hIcon = icon.Handle;

            SimpleTrayIconApi.TrayMenuCreate(hIcon, "tip", out var hMenu);

            SimpleTrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                Console.WriteLine("Clicked1");
            }, out var hItem1);

            SimpleTrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                Console.WriteLine("Clicked2");
            }, out var hItem2);

            var item3Checked = false;
            string item3Content = "a";
            SimpleTrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                SimpleTrayIconApi.TrayMenuItemIsChecked(s, item3Checked = !item3Checked);
                item3Content += (char)(item3Content.Last() + 1);
                SimpleTrayIconApi.TrayMenuItemContent(s, item3Content);
                Console.WriteLine("Clicked3");
            }, out var hItem3);

            SimpleTrayIconApi.TrayMenuItemContent(hItem1, "item1");
            SimpleTrayIconApi.TrayMenuItemContent(hItem2, "item2");
            SimpleTrayIconApi.TrayMenuItemContent(hItem3, item3Content);

            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem1);
            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem2);
            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem3);

            SimpleTrayIconApi.TrayMenuShow(hMenu);

            NativeMethods.RunLoop();
        }
    }
}
