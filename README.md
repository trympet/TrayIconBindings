# SimpleTrayIcon
A fast, lightweight, object oriented wrapper for the [Shell_NotifyIcon](https://learn.microsoft.com/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw) API with no framework dependencies.

[![NuGet version (SimpleTrayIcon)](https://img.shields.io/nuget/v/SimpleTrayIcon.svg?style=flat-square)](https://www.nuget.org/packages/SimpleTrayIcon/)

## Usage

```csharp
var icon = new System.Drawing.Icon(typeof(Program), "SimpleTrayIcon.Demo.tray-icon.ico"); // Load an icon for the tray
using var menu = new TrayMenu(icon, "Tooltip", true);
var item = new TrayMenuItem { Content = "Item1" };
menu.Items.Add(item);
menu.Items.Add(new TrayMenuSeparator());
menu.Items.Add(new TrayMenuItem { Content = $"some content", IsChecked = true });
item.Click += (s, e) => ((TrayMenuItem)s).IsChecked = !((TrayMenuItem)s).IsChecked; // Attach an event
NativeMethods.RunLoop(); // Runs the message pump. Winforms, WPF, etc., will do this for you.
```

## NativeAOT Linking Options

SimpleTrayIcon is fully AOT compatible.

You can build with static linking to a static library or dynamic library (with DLL).

To enable static linking to the SimpleTrayIcon DLL:
```xml
<PropertyGroup>
    <SimpleTrayIconStaticLinking>true</SimpleTrayIconStaticLinking>
</PropertyGroup>
```

To enable static linking to the SimpleTrayIcon static library:
```xml
<PropertyGroup>
    <SimpleTrayIconStaticLinking>true</SimpleTrayIconStaticLinking>
    <UseSimpleTrayIconStaticLib>true</UseSimpleTrayIconStaticLib>
</PropertyGroup>
```

## OneCore
There is a OneCore SKU of the package `SimpleTrayIcon.onecore` which links to OneCoreUAP and VCRUNTIME140_APP. It is indeed *not* OneCoreUAP compatible, but is compatible with any full-trust UWP apps. This package does not support static linking.