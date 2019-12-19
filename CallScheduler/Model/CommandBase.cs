using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CallScheduler.Model
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
    /// <summary>
    /// 1. ICommand 동작 방식
    /// XAML 에서 Command에 바인딩할 때 ICommand 객체를 바인딩할 경우,
    /// ICommand 객체에서 ICommand 요소인 CanExecute를 실행시켜 버튼을 활성화 가능한지 정한다.
    /// CanExecute는 프로그램 실행 시 최초 1회만 실행하여 버튼을 사용할 수 있는지 결정한다.
    /// 이후 CanExecute를 통해 버튼을 사용가능한지 체크하려면, CanExecuteChanged 요소를 사용하여
    /// 사용가능한지 알려야한다.
    /// 버튼이 ICommand 객체를 최초 할당했을 때 CanExecuteChanged 이벤트를 구독한다.
    /// 이후 버튼에서 아래 코드의 CanExecuteChanged.add를 호출하게 되고, 
    /// 생성자를 통해 전달된 메소드는 CommandManager.RequerySuggested 이벤트에 전달된다.
    /// 이후 버튼을 클릭할 때마다 RequerySuggested 이벤트를 발생시킨다.
    /// 그러면 버튼은 CanExecuteChanged 알림을 받게 되고
    /// CanExecute를 호출해 객체의 상태를 조회하게 된다.
    /// 이후 Execute를 호출해서 Command를 실행한다.
    /// 
    /// ※ 검색하다보면 ICommand 객체를 CanExecute를 발생시키는 메소드를 만들어서 계속 메소드를 호출하게 하는데,
    /// 그런 메소드를 만들지 않고 CommandManager.RequerySuggested 에 등록하면 Windows가 알아서 CanExecuteChanged를 호출해준다.
    /// 
    /// 2. ICommand 메모리 누수
    /// View가 종료된 뒤 ViewModel의 ICommand 객체는 해제되지 않고 지속적으로 이미 종료된 View의 CanExecute를 호출하고 있다.
    /// 이는 메모리 누수로 연결될 여지가 충분하다.
    /// 따라서 View가 종료되는 시점에 이벤트를 걸어 DataContext = null; 코드로 하여금 View의 DataContext를 빼버리거나,
    /// ICommand Memory Leak Solution Class 를 사용하여 처음부터 CanExecute가 지속적으로 호출되지 않도록
    /// CommandManager.RequerySuggested 에 Command Method를 등록하지 않고,
    /// 각 Command마다 생성된 _CanExecuteChangedHandler 를 참조하여 1개 밖에 등록되어 있지 없는 Command를 실행한다.
    /// CommandManager.RequerySuggested 에 Command Method가 등록되지 않았기 때문에 Windows에서 CanExecute를 지속적으로 실행하지 않는다.
    /// 따라서 메모리 누수도 발생하지 않는다.
    /// </summary>
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
            if(!ReferenceEquals(handler, null))
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                EventHandler[] call = new EventHandler[handler.Count];
                int count = 0;

                for (int i = handler.Count - 1; i >= 0; i--)
                {
                    WeakReference WkReference = handler[i];
                    EventHandler _handler = WkReference.Target as EventHandler;

                    if (_handler is null)
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
            if (!ReferenceEquals(handler, null))
            {
                foreach (WeakReference handlerRef in handler)
                {
                    EventHandler _handler = handlerRef.Target as EventHandler;

                    if (!ReferenceEquals(_handler, null))
                    {
                        CommandManager.RequerySuggested += _handler;
                    }
                }
            }
        }

        public static void RemoveHandlersFromRequerySuggested(List<WeakReference> handler)
        {
            if (!ReferenceEquals(handler, null))
            {
                foreach (WeakReference handlerRef in handler)
                {
                    EventHandler _handler = handlerRef.Target as EventHandler;

                    if (!ReferenceEquals(_handler, null))
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
            if (!ReferenceEquals(handler, null))
            {
                for (int i = handler.Count - 1; i >= 0; i--)
                {
                    WeakReference WkReference = handler[i];
                    EventHandler existingHandler = WkReference.Target as EventHandler;

                    if (existingHandler is null || existingHandler.Equals(_handler))
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
