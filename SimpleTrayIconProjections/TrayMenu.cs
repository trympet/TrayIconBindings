using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static SimpleTrayIcon.NativeMethods;

namespace SimpleTrayIcon
{
    public partial class TrayMenu : IDisposable
    {
        private readonly Action<TrayMenuItemBase> _onAddedDelegate;
        private readonly Action<TrayMenuItemBase> _onRemovedDelegate;
        private readonly bool _ownsItems;
        private Icon? _icon;
        private IntPtr _hInstance;
        private ICollection<TrayMenuItemBase> _items = new ObservableCollection<TrayMenuItemBase>();
        private ItemSubscription<TrayMenuItemBase>? _itemSubscription;

        public TrayMenu(Icon icon, string tip, bool ownsItems = true)
        {
            _onAddedDelegate = OnItemAdded;
            _onRemovedDelegate = OnItemRemoved;
            _ownsItems = ownsItems;
            _icon = icon ?? throw new ArgumentNullException(nameof(icon));
            _hInstance = TrayMenuCreate(_icon.Handle, tip);
            _itemSubscription = ItemSubscription.Create(_items, _onAddedDelegate, _onRemovedDelegate);
            Show();
        }

        ~TrayMenu()
        {
            Dispose(disposing: false);
        }

#if NET6_0_OR_GREATER
        [MemberNotNullWhen(false, nameof(_icon))]
#endif
        private bool DisposedValue { get; set; }

        public ICollection<TrayMenuItemBase> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    foreach (var item in _items)
                    {
                        OnItemRemoved(item);
                    }

                    _items = value ?? throw new ArgumentNullException(nameof(value));
                    _itemSubscription?.Dispose();

                    foreach (var item in value)
                    {
                        OnItemAdded(item);
                    }

                    _itemSubscription = ItemSubscription.Create(value, _onAddedDelegate, _onRemovedDelegate);
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Icon Icon
        {
            get
            {
                if (DisposedValue)
                {
                    throw new ObjectDisposedException(nameof(TrayMenu));
                }

                return _icon;
            }
            set
            {
                if (DisposedValue)
                {
                    throw new ObjectDisposedException(nameof(TrayMenu));
                }

                if (_icon != value)
                {
                    _icon.Dispose();
                    _icon = value;
                    TrayMenuSetIcon(_hInstance, value.Handle);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    TrayMenuClose(_hInstance);
                    if (_ownsItems)
                    {
                        foreach (var item in _items)
                        {
                            (item as IDisposable)?.Dispose();
                        }
                    }

                    _icon.Dispose();
                    _itemSubscription?.Dispose();
                }

                if (_hInstance != IntPtr.Zero)
                {
                    TrayMenuRelease(ref _hInstance);
                    _hInstance = IntPtr.Zero;
                }

                _icon = null;
                _itemSubscription = null;

                DisposedValue = true;
            }
        }

        public void Show()
        {
            if (!DisposedValue)
            {
                TrayMenuShow(_hInstance);
            }
        }

        public void Hide()
        {
            if (!DisposedValue)
            {
                TrayMenuClose(_hInstance);
            }
        }

        protected virtual void OnItemAdded(TrayMenuItemBase item)
        {
            TrayMenuAdd(_hInstance, item.HInstance);
        }

        protected virtual void OnItemRemoved(TrayMenuItemBase item)
        {
            TrayMenuRemove(_hInstance, item.HInstance);
            if (_ownsItems)
            {
                (item as IDisposable)?.Dispose();
            }
        }
    }
}