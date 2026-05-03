using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static SimpleTrayIcon.NativeMethods;

namespace SimpleTrayIcon
{
    public class TrayMenuSubItem : TrayMenuItemBase, IDisposable
    {
        private readonly Action<TrayMenuItemBase> _onAddedDelegate;
        private readonly Action<TrayMenuItemBase> _onRemovedDelegate;
        private bool _disposedValue;
        private string _content = string.Empty;
        private ObservableCollection<TrayMenuItemBase> _items = new ObservableCollection<TrayMenuItemBase>();
        private TrayMenu.ItemSubscription<TrayMenuItemBase>? _itemSubscription;

        public TrayMenuSubItem()
        {
            _onAddedDelegate = OnItemAdded;
            _onRemovedDelegate = OnItemRemoved;
            HInstanceRef = TrayMenuSubItemCreate();
            _itemSubscription = TrayMenu.ItemSubscription.Create(_items, _onAddedDelegate, _onRemovedDelegate);
        }

        ~TrayMenuSubItem()
        {
            Dispose(disposing: false);
        }

        public virtual string Content
        {
            get => _content;
            set
            {
                GuardNotDisposed();
                if (_content != value)
                {
                    _content = value;
                    TrayMenuSubItemContent(HInstance, value ?? string.Empty);
                }
            }
        }

        public ObservableCollection<TrayMenuItemBase> Items
        {
            get => _items;
            set
            {
                GuardNotDisposed();
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

                    _itemSubscription = TrayMenu.ItemSubscription.Create(value, _onAddedDelegate, _onRemovedDelegate);
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _itemSubscription?.Dispose();
                }

                if (HInstanceRef != IntPtr.Zero)
                {
                    TrayMenuSubItemRelease(ref HInstanceRef);
                    HInstanceRef = IntPtr.Zero;
                }

                if (disposing)
                {
                    foreach (var item in _items)
                    {
                        (item as IDisposable)?.Dispose();
                    }
                }

                _itemSubscription = null;
                _disposedValue = true;
            }
        }

        protected virtual void OnItemAdded(TrayMenuItemBase item)
        {
            TrayMenuSubItemAdd(HInstance, item.HInstance);
        }

        protected virtual void OnItemRemoved(TrayMenuItemBase item)
        {
            TrayMenuSubItemRemove(HInstance, item.HInstance);
            (item as IDisposable)?.Dispose();
        }

        private void GuardNotDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(TrayMenuSubItem));
            }
        }
    }
}