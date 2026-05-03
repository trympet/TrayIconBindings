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
                Console.WriteLine("3) dispose demo");
                var input = Console.ReadLine();
                switch (c = input?.FirstOrDefault())
                {
                    case '1':
                        ManagedDemo();
                        break;
                    case '2':
                        PInvokeDemo();
                        break;
                    case '3':
                        DisposeDemo();
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
            var subMenu = new TrayMenuSubItem { Content = "Sub menu" };
            var nestedItem = new TrayMenuItem { Content = "Nested item" };
            var addNestedItem = new TrayMenuItem { Content = "Add nested item" };
            var disabledCheckedItem = new TrayMenuItem { Content = "Checked disabled item", IsChecked = true, IsCheckable = false };

            int itemNumber = 1;
            int nestedItemNumber = 1;
            void OnClicked(object? sender, EventArgs e)
            {
                if (sender is TrayMenuItem item)
                {
                    menu.Icon = GetNextIcon();
                    item.IsChecked = !item.IsChecked;
                    Console.WriteLine($"{item.Content} clicked.");
                }
            }

            void OnTopLevelClicked(object? sender, EventArgs e)
            {
                OnClicked(sender, e);
                var newItem = new TrayMenuItem { Content = $"Item{++itemNumber}" };
                newItem.Click += OnClicked;
                menu.Items.Add(newItem);
                menu.Items.Add(new TrayMenuSeparator());
            }

            void OnAddNestedClicked(object? sender, EventArgs e)
            {
                OnClicked(sender, e);
                var newNestedItem = new TrayMenuItem { Content = $"Nested item {++nestedItemNumber}" };
                newNestedItem.Click += OnClicked;
                subMenu.Items.Add(newNestedItem);
            }

            item1.Click += OnTopLevelClicked;
            nestedItem.Click += OnClicked;
            addNestedItem.Click += OnAddNestedClicked;
            menu.Items.Add(item1);
            menu.Items.Add(new TrayMenuSeparator());
            menu.Items.Add(disabledCheckedItem);
            menu.Items.Add(new TrayMenuSeparator());
            subMenu.Items.Add(nestedItem);
            subMenu.Items.Add(new TrayMenuSeparator());
            subMenu.Items.Add(addNestedItem);
            menu.Items.Add(subMenu);

            menu.DoubleClick += (_, _) => Console.WriteLine("Double click.");

            NativeMethods.RunLoop();
        }

        private static void DisposeDemo()
        {
            {
                using var menu = new TrayMenu(GetNextIcon(), "Dispose verification", true);
                var subMenu = new TrayMenuSubItem { Content = "Sub menu" };
                var nestedItem = new TrayMenuItem { Content = "Nested item" };
                var separator = new TrayMenuSeparator();
                var anotherNestedItem = new TrayMenuItem { Content = "Another nested item" };

                subMenu.Items.Add(nestedItem);
                subMenu.Items.Add(separator);
                subMenu.Items.Add(anotherNestedItem);
                menu.Items.Add(subMenu);
            }

            Console.WriteLine("Dispose verification completed.");
        }

        private static Icon GetNextIcon()
        {
            iconId = iconId % 2 + 1;
            return new Icon(typeof(SimpleTrayIconApi), $"SimpleTrayIcon.Demo.tray-icon-{iconId}.ico");
        }

        private static void PInvokeDemo()
        {
            var icon = new Icon(typeof(SimpleTrayIconApi), "SimpleTrayIcon.Demo.tray-icon-1.ico");
            var hIcon = icon.Handle;
            DoubleClickHandler onDoubleClick = _ => Console.WriteLine("Double click!");
            SimpleTrayIconApi.TrayMenuCreate(hIcon, "tip", onDoubleClick, out var hMenu);

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
            SimpleTrayIconApi.TrayMenuItemIsChecked(hItem2, true);
            SimpleTrayIconApi.TrayMenuItemIsCheckable(hItem2, false);

            SimpleTrayIconApi.TrayMenuSubItemCreate(out var hSubMenu);
            SimpleTrayIconApi.TrayMenuSubItemContent(hSubMenu, "sub menu");

            SimpleTrayIconApi.TrayMenuItemCreate((s, e) =>
            {
                Console.WriteLine("Nested clicked");
            }, out var hNestedItem);
            SimpleTrayIconApi.TrayMenuItemContent(hNestedItem, "nested item");
            SimpleTrayIconApi.TrayMenuSubItemAdd(hSubMenu, hNestedItem);

            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem1);
            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem2);
            SimpleTrayIconApi.TrayMenuAdd(hMenu, hItem3);
            SimpleTrayIconApi.TrayMenuAdd(hMenu, hSubMenu);

            SimpleTrayIconApi.TrayMenuShow(hMenu);

            NativeMethods.RunLoop();
        }
    }
}
