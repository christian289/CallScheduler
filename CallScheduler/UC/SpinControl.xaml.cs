using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// UserControl은 독립적으로 동작하는 것으로 만들기 위해 NameSpace와 CommandBase를 따로 구성함.
/// </summary>
namespace UC
{
    public partial class SpinControl : UserControl
    {
        #region Base

        #region OnPropertyChanged 정의
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region CommandBase
        public class CommandBase : ICommand
        {
            private List<WeakReference> _CanExecuteChangedHandler;
            private readonly Func<bool> _CanExecute;
            private readonly Action _Execute;

            public CommandBase(Action execute) : this(execute, null)
            {
            }

            public CommandBase(Action execute, Func<bool> canExecute) : this(execute, canExecute, false)
            {
            }

            public CommandBase(Action execute, Func<bool> canExecute, bool isAutomaticRequeryDisabled)
            {
                if (execute is null)
                {
                    throw new ArgumentException("executeMethod null");
                }

                _Execute = execute;
                _CanExecute = canExecute;
                _IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
            }

            #region IsAutomaticRequeryDisabled Property
            private bool _IsAutomaticRequeryDisabled = false;

            public bool IsAutomaticRequeryDisabled
            {
                get => _IsAutomaticRequeryDisabled;
                set
                {
                    _IsAutomaticRequeryDisabled = value;

                    if (_IsAutomaticRequeryDisabled)
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_CanExecuteChangedHandler);
                    }
                    else
                    {
                        CommandManagerHelper.AddHandlersToRequerySuggested(_CanExecuteChangedHandler);
                    }
                }
            }
            #endregion

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    if (!_IsAutomaticRequeryDisabled)
                    {
                        CommandManager.RequerySuggested += value;
                    }
                    else
                    {
                        CommandManagerHelper.AddWeakReferenceHandler(ref _CanExecuteChangedHandler, value, -1);
                    }
                }
                remove
                {
                    if (!_IsAutomaticRequeryDisabled)
                    {
                        CommandManager.RequerySuggested -= value;
                    }
                    else
                    {
                        CommandManagerHelper.RemoveWeakReferenceHandler(_CanExecuteChangedHandler, value);
                    }
                }
            }

            public bool CanExecute(object parameter)
            {
                if (_CanExecute is null)
                {
                    return true;
                }

                return _CanExecute();
            }

            public void Execute(object parameter)
            {
                _Execute();
            }

            public void RaiseCanExecuteChanged()
            {
                OnCanExecuteChanged();
            }

            protected virtual void OnCanExecuteChanged()
            {
                CommandManagerHelper.CallWeakReferenceHandler(_CanExecuteChangedHandler);
            }
        }

        public class CommandManagerHelper
        {
            public static void CallWeakReferenceHandler(List<WeakReference> handler)
            {
                if (handler is object)
                {
                    // Take a snapshot of the handlers before we call out to them since the handlers
                    // could cause the array to me modified while we are reading it.
                    EventHandler[] call = new EventHandler[handler.Count];
                    int count = 0;

                    for (int i = handler.Count - 1; i >= 0; i--)
                    {
                        WeakReference WkReference = handler[i];

                        if (!(WkReference.Target is EventHandler _handler))
                        {
                            // Clean up old handlers that have been collected
                            handler.RemoveAt(i);
                        }
                        else
                        {
                            call[count] = _handler;
                            count++;
                        }
                    }

                    // Call the handlers that we snapshotted
                    for (int i = 0; i < count; i++)
                    {
                        call[i](null, EventArgs.Empty);
                    }
                }
            }

            public static void AddHandlersToRequerySuggested(List<WeakReference> handler)
            {
                if (handler is object)
                {
                    foreach (WeakReference handlerRef in handler)
                    {
                        if (handlerRef.Target is EventHandler _handler)
                        {
                            CommandManager.RequerySuggested += _handler;
                        }
                    }
                }
            }

            public static void RemoveHandlersFromRequerySuggested(List<WeakReference> handler)
            {
                if (handler is object)
                {
                    foreach (WeakReference handlerRef in handler)
                    {
                        if (handlerRef.Target is EventHandler _handler)
                        {
                            CommandManager.RequerySuggested -= _handler;
                        }
                    }
                }
            }

            public static void AddWeakReferenceHandler(ref List<WeakReference> handler, EventHandler _handler)
            {
                AddWeakReferenceHandler(ref handler, _handler, -1);
            }

            public static void AddWeakReferenceHandler(ref List<WeakReference> handler, EventHandler _handler, int defaultListSize)
            {
                if (handler is null)
                {
                    handler = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
                }

                handler.Add(new WeakReference(_handler));
            }

            public static void RemoveWeakReferenceHandler(List<WeakReference> handler, EventHandler _handler)
            {
                if (handler is object)
                {
                    for (int i = handler.Count - 1; i >= 0; i--)
                    {
                        WeakReference WkReference = handler[i];

                        if (!(WkReference.Target is EventHandler existingHandler) || existingHandler.Equals(_handler))
                        {
                            // Clean up old handlers that have been collected
                            // in addition to the handler that is to be removed.
                            handler.RemoveAt(i);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Properties
        private int _Hour = 0;

        public int Hour
        {
            get => _Hour;
            set
            {
                _Hour = value;
                OnPropertyChanged();
            }
        }

        private int _Minute = 0;

        public int Minute
        {
            get => _Minute;
            set
            {
                _Minute = value;
                OnPropertyChanged();
            }
        }

        #region HourUp 삼각형 Point 좌표
        private PointCollection _HourUpPoint = new PointCollection(new Point[] { new Point(10, 15), new Point(15, 10), new Point(20, 15) });

        public PointCollection HourUpPoint
        {
            get => _HourUpPoint;
            set
            {
                _HourUpPoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region HourDown 삼각형 Point 좌표
        private PointCollection _HourDownPoint = new PointCollection(new Point[] { new Point(10, 15), new Point(15, 10), new Point(20, 15) });

        public PointCollection HourDownPoint
        {
            get => _HourDownPoint;
            set
            {
                _HourDownPoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region MinuteUp 삼각형 Point 좌표
        private PointCollection _MinuteUpPoint = new PointCollection(new Point[] { new Point(10, 15), new Point(15, 10), new Point(20, 15) });

        public PointCollection MinuteUpPoint
        {
            get => _MinuteUpPoint;
            set
            {
                _MinuteUpPoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region MinuteDown 삼각형 Point 좌표
        private PointCollection _MinuteDownPoint = new PointCollection(new Point[] { new Point(10, 15), new Point(15, 10), new Point(20, 15) });

        public PointCollection MinuteDownPoint
        {
            get => _MinuteDownPoint;
            set
            {
                _MinuteDownPoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Commands

        #region HourUp
        private ICommand _HourUpCommand;

        public ICommand HourUpCommand
        {
            get
            {
                return _HourUpCommand ?? (_HourUpCommand = new CommandBase(HourUp, CanExecute_HourUp, true));
            }
        }

        private void HourUp()
        {
            if (Hour >= 0 && Hour <= 22)
            {
                Hour++;
            }
            else
            {
                Hour = 0;
            }
        }

        private bool CanExecute_HourUp()
        {
            return true;
        }
        #endregion

        #region HourDown
        private ICommand _HourDownCommand;

        public ICommand HourDownCommand
        {
            get
            {
                return _HourDownCommand ?? (_HourDownCommand = new CommandBase(HourDown, CanExecute_HourDown, true));
            }
        }

        private void HourDown()
        {
            if (Hour >= 1 && Hour <= 23)
            {
                Hour--;
            }
            else
            {
                Hour = 0;
            }
        }

        private bool CanExecute_HourDown()
        {
            return true;
        }
        #endregion

        #region MinuteUp
        private ICommand _MinuteUpCommand;

        public ICommand MinuteUpCommand
        {
            get
            {
                return _MinuteUpCommand ?? (_MinuteUpCommand = new CommandBase(MinuteUp, CanExecute_MinuteUp, true));
            }
        }

        private void MinuteUp()
        {
            if (Minute >= 0 && Minute <= 58)
            {
                Minute++;
            }
            else
            {
                Minute = 0;
            }
        }

        private bool CanExecute_MinuteUp()
        {
            return true;
        }
        #endregion

        #region MinuteDown
        private ICommand _MinuteDownCommand;

        public ICommand MinuteDownCommand
        {
            get
            {
                return _MinuteDownCommand ?? (_MinuteDownCommand = new CommandBase(MinuteDown, CanExecute_MinuteDown, true));
            }
        }

        private void MinuteDown()
        {
            if (Minute >= 1 && Minute <= 59)
            {
                Minute--;
            }
            else
            {
                Minute = 0;
            }
        }

        private bool CanExecute_MinuteDown()
        {
            return true;
        }
        #endregion

        #endregion

        public SpinControl()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
