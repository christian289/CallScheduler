using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// UserControl은 독립적으로 동작하는 것으로 만들기 위해 NameSpace와 CommandBase를 따로 구성함.
/// </summary>
namespace UC
{
    /// <summary>
    /// 1. 의존 프로퍼티에서 get, set이 디버깅이 안 잡히는 이유
    /// 의존 프로퍼티의 바인딩은 CLR wrapper (get, set)을 무시하고 기본 의존 프로퍼티를 직접 업데이트하여 성능을 향상시킨다.
    /// 따라서 의존 프로퍼티의 set 에서 처리할 작업은 Callback 함수를 정의하여 작업한다.
    /// 
    /// 2. DependencyProperty 정의 시, ownerType에서 typeof(this.GetType())이 불가능한 이유
    /// - static Method에서 this 키워드 사용 불가(가능 했다면 this.GetType()으로 해도 되었을 듯.)
    /// - 위 방식이 불가하기 때문에 타입을 직접 넘겨야 함. PropertyFromName에 등록할 때 Type명을 DependencyProperty의 key값으로 사용. (해당 타입에 대한 특정한 속성은 유일해야 하기 때문)
    /// - 컨트롤 하나에 동일한 속성이 중복될 수 없는 이유가 위와 같을 것이다.
    /// 
    /// 3. DependencyProperty를 바인딩하기 위해서는, xaml에서 ElementName을 반드시 지정하여, Path로 바인딩 경로를 지정해야 한다.
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

        #region Dependency Properties

        #region SelectedDate
        public DateTime SelectedDate
        {
            get => (DateTime)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        private static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(DateTimePickerControl),
            new FrameworkPropertyMetadata(
                new DateTime(2020, 1, 1, 0, 0 ,0),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(SelectedDatePropertyChanged)
                )
            );

        private static void SelectedDatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTimePickerControl obj = sender as DateTimePickerControl;
            DateTime NewValue = (DateTime)e.NewValue;

            obj.Hour = NewValue.Hour;
            obj.Minute = NewValue.Minute;
        }
        #endregion

        #region Hour
        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        private static readonly DependencyProperty HourProperty = DependencyProperty.Register(
            nameof(Hour),
            typeof(int),
            typeof(DateTimePickerControl),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(HourPropertyChanged),
                new CoerceValueCallback(HourCoerceValue)
                )
            );

        private static void HourPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTimePickerControl obj = sender as DateTimePickerControl;
            obj.SelectedDate = ChangeTime(obj.SelectedDate, (int)e.NewValue, obj.SelectedDate.Minute);
        }

        private static object HourCoerceValue(DependencyObject sender, object data)
        {
            if ((int)data >= 24)
            {
                data = 0;
            }
            else if ((int)data < 0)
            {
                data = 23;
            }

            return data;
        }
        #endregion

        #region Minute
        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        private static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
            nameof(Minute),
            typeof(int),
            typeof(DateTimePickerControl),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(MinutePropertyChanged),
                new CoerceValueCallback(MinuteCoerceValue)
                )
            );

        private static void MinutePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTimePickerControl obj = sender as DateTimePickerControl;
            obj.SelectedDate = ChangeTime(obj.SelectedDate, obj.SelectedDate.Hour, (int)e.NewValue);
        }

        private static object MinuteCoerceValue(DependencyObject sender, object data)
        {
            if ((int)data >= 60)
            {
                data = 0;
            }
            else if ((int)data < 0)
            {
                data = 59;
            }

            return data;
        }
        #endregion

        #region FontSize
        public new int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        private new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            nameof(FontSize),
            typeof(int),
            typeof(DateTimePickerControl),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(FontSizePropertyChanged),
                new CoerceValueCallback(FontSizeCoerceValue)
                )
            );

        private static void FontSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object FontSizeCoerceValue(DependencyObject sender, object data)
        {
            if ((int)data <= 7)
            {
                data = 11;
            }

            return data;
        }
        #endregion

        #endregion

        private static DateTime ChangeTime(DateTime dateTime, int hours, int minutes, int seconds = default, int milliseconds = default)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds, milliseconds, dateTime.Kind);
        }

        public DateTimePickerControl()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
