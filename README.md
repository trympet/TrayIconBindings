# TrayIconBindings
A fast, lightweight, object oriented wrapper for Shell_NotifyIcon API.

```csharp
var icon = new System.Drawing.Icon(typeof(Program), "SimpleTrayIcon.Demo.tray-icon.ico");
using var menu = new TrayMenu(icon, "Tooltip", true);
var item = new TrayMenuItem { Content = "Item1" };
menu.Items.Add(item);
menu.Items.Add(new TrayMenuSeparator());
menu.Items.Add(new TrayMenuItem { Content = $"some content", IsChecked = true });
item.Click += (s, e) => ((TrayMenuItem)s).IsChecked = !((TrayMenuItem)s).IsChecked;
NativeMethods.RunLoop(); // Runs the message pump. Winforms, WPF, etc., will do this for you.
```
