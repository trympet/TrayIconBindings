using System;
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
        private readonly OnClicked _onClickedDelegate; // store to keep delegate pinned.
        private bool _disposedValue;
        private string _content = string.Empty;
        private bool _isChecked;

        public TrayMenuItem()
        {
            _onClickedDelegate = OnClick;
            HInstanceRef = TrayMenuItemCreate(_onClickedDelegate);
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

        private void OnClick(IntPtr sender, uint e)
        {
            if (!_disposedValue)
            {
                OnClick(new TrayMenuItemClickedEventArgs { CommandId = e });
            }
        }

        private void GuardNotDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(TrayMenuItem));
            }
        }
    }
}