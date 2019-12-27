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
    /// <summary>
    /// DateTimePickerControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DateTimePickerControl : UserControl
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

        public DateTimePickerControl()
        {
            InitializeComponent();
        }
    }
}
