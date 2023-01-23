using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static SimpleTrayIcon.NativeMethods;

namespace SimpleTrayIcon
{
    public class TrayMenuItemClickedEventArgs : EventArgs
    {
        /// <summary>
        /// The win32 command ID of the menu item.
        /// </summary>
        public uint CommandId { get; set; }
    }

    public class TrayMenuItem : TrayMenuItemBase, IDisposable
    {
        private static readonly Dictionary<IntPtr, WeakReference<Action<uint>>> s_clickHandlers = new();
#if !NET6_0_OR_GREATER
        private readonly TrayMenuItemClickHandler _onClickedDelegate; // store to keep delegate pinned.
#endif
        private bool _disposedValue;
        private string _content = string.Empty;
        private bool _isChecked;

        public TrayMenuItem()
        {
            unsafe
            {
#if NET6_0_OR_GREATER
                var pointer = HInstanceRef = TrayMenuItemCreate(&ClickCallback);
                s_clickHandlers[pointer] = new WeakReference<Action<uint>>(OnClick);
#else
                _onClickedDelegate = ClickCallback;
                HInstanceRef = TrayMenuItemCreate((delegate* unmanaged[Stdcall]<IntPtr, uint, void>)Marshal.GetFunctionPointerForDelegate(_onClickedDelegate));
#endif
            }
        }

        ~TrayMenuItem()
        {
            Dispose(disposing: false);
        }

        public event EventHandler<TrayMenuItemClickedEventArgs>? Click;

        public virtual ICommand? Command { get; set; }

        public virtual string Content
        {
            get => _content;
            set
            {
                GuardNotDisposed();
                if (_content != value)
                {
                    _content = value;
                    TrayMenuItemContent(HInstance, value ?? string.Empty);
                }
            }
        }

        public virtual bool IsChecked
        {
            get => _isChecked;
            set
            {
                GuardNotDisposed();
                if (_isChecked != value)
                {
                    _isChecked = value;
                    TrayMenuItemIsChecked(HInstance, value);
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
                    _ = disposing;
                }

                if (HInstanceRef != IntPtr.Zero)
                {
                    TrayMenuItemRelease(ref HInstanceRef);
                    HInstanceRef = IntPtr.Zero;
                }

                _disposedValue = true;
            }
        }

        protected virtual void OnClick(TrayMenuItemClickedEventArgs e)
        {
            Click?.Invoke(this, e);

            if (Command is not null && Command.CanExecute(this))
            {
                Command.Execute(this);
            }
        }

        private void OnClick(uint id)
        {
            if (!_disposedValue)
            {
                OnClick(new TrayMenuItemClickedEventArgs { CommandId = id });
            }
        }

        private void GuardNotDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(TrayMenuItem));
            }
        }
#if NET6_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static void ClickCallback(IntPtr hInstance, uint id)
        {
            if (!s_clickHandlers.TryGetValue(hInstance, out var reference))
            {
                Debug.Fail("A registered callback was removed.");
                return;
            }

            if (!reference.TryGetTarget(out var callback))
            {
                s_clickHandlers.Remove(hInstance);
                return;
            }

            callback(id);
        }
#else
        private void ClickCallback(IntPtr hInstance, uint id)
        {
            OnClick(id);
        }
#endif
    }
}