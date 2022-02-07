using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayIcon
{
    public partial class TrayMenu
    {
        private static class ItemSubscription
        {
            public static ItemSubscription<T>? Create<T>(ICollection<T> items, Action<T> onAdded, Action<T> onRemoved)
            {
                if (items is INotifyCollectionChanged notifyingCollection)
                {
                    return new ItemSubscription<T>(items, notifyingCollection, onAdded, onRemoved);
                }

                return null;
            }
        }

        private sealed class ItemSubscription<T> : IDisposable
        {
            private readonly ICollection<T> _items;
            private readonly INotifyCollectionChanged _notifyingCollection;
            private readonly Action<T> _onAdded;
            private readonly Action<T> _onRemoved;

            internal ItemSubscription(ICollection<T> items, INotifyCollectionChanged notifyingCollection, Action<T> onAdded, Action<T> onRemoved)
            {
                notifyingCollection.CollectionChanged += NotifyingCollection_CollectionChanged;
                this._items = items;
                _notifyingCollection = notifyingCollection;
                this._onAdded = onAdded;
                this._onRemoved = onRemoved;
            }

            public void Dispose()
            {
                foreach (var item in _items)
                {

                }

                _notifyingCollection.CollectionChanged -= NotifyingCollection_CollectionChanged;
            }

            private void NotifyingCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.OldItems is not null)
                {
                    foreach (var item in e.OldItems)
                    {
                        _onRemoved((T)item);
                    }
                }

                if (e.NewItems is not null)
                {
                    foreach (var item in e.NewItems)
                    {
                        _onAdded((T)item);
                    }
                }
            }
        }
    }
}
