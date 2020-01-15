using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CallScheduler.Base
{
    #region 사용하지 않는 ICommand 생성자
    //public class CommandBase : ICommand
    //{
    //    private readonly Func<bool> canExecute;
    //    private readonly Action execute;
    //    private readonly Func<bool> canExecute_strParam1;
    //    private readonly Action<string> execute_strParam1;

    //    public CommandBase(Action execute) : this(execute, null)
    //    {
    //    }

    //    public CommandBase(Action<string> execute_strParam1) : this(execute_strParam1, null)
    //    {
    //    }

    //    public CommandBase(Action execute, Func<bool> canExecute)
    //    {
    //        this.execute = execute;
    //        this.canExecute = canExecute;
    //    }

    //    public CommandBase(Action<string> execute, Func<bool> canExecute)
    //    {
    //        this.execute_strParam1 = execute;
    //        this.canExecute_strParam1 = canExecute;
    //    }

    //    public event EventHandler CanExecuteChanged
    //    {
    //        add
    //        {
    //            CommandManager.RequerySuggested += value;
    //        }

    //        remove
    //        {
    //            CommandManager.RequerySuggested -= value;
    //        }
    //    }

    //    /// <summary>
    //    /// implement of icommand can execute method
    //    /// </summary>
    //    /// <param name="o">parameter by default of icommand interface</param>
    //    /// <returns>can execute or not</returns>
    //    public bool CanExecute(object o)
    //    {
    //        if (canExecute == null)
    //        {
    //            return true;
    //        }

    //        return canExecute();
    //    }

    //    public void Execute(object o)
    //    {
    //        if (o != null)
    //        {
    //            execute_strParam1(o.ToString());
    //        }
    //        else
    //        {
    //            execute();
    //        }
    //    }
    //}
    #endregion

    #region ICommand Memory Leak Solution Class
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

    /// <summary>
    /// 메모 : Predicate<T> 는 Func<T, bool>과 같은 키워드다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandBase<T> : ICommand
    {
        private List<WeakReference> _CanExecuteChangedHandler;
        private readonly Predicate<T> _CanExecute;
        private readonly Action<T> _Execute;

        public CommandBase(Action<T> execute) : this(execute, null)
        {
        }

        public CommandBase(Action<T> execute, Predicate<T> canExecute) : this(execute, canExecute, false)
        {
        }

        public CommandBase(Action<T> execute, Predicate<T> canExecute, bool isAutomaticRequeryDisabled)
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

            return _CanExecute((parameter is null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        public void Execute(object parameter)
        {
            _Execute((parameter is null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
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

    /// <summary>
    /// https://github.com/crosbymichael/mvvm-async/blob/master/MVVM-Async/Commands/DelegateCommand.cs#L44
    /// View가 소멸되고 해당 ViewModel이 사용되지 않는 Command가 메모리에 계속 상주해 있는 문제를 해결하는 클래스
    /// </summary>
    public class CommandManagerHelper
    {
        public static void CallWeakReferenceHandler(List<WeakReference> handler)
        {
            if(handler is object)
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
}
