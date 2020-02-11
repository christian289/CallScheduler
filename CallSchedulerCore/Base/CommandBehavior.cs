using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace CallSchedulerCore.Base
{
    /// <summary>
    /// https://m.blog.naver.com/PostView.nhn?blogId=vactorman&logNo=220516529243&proxyReferer=https%3A%2F%2Fwww.google.com%2F
    /// 참고
    /// </summary>
    public class CommandBehavior
    {
        private class EventRaiseAttribute : Attribute
        {
        }

        #region Command 
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(CommandBehavior),
            new PropertyMetadata(OnCommandChanged)
            );

        public static ICommand GetCommand(DependencyObject d)
        {
            return d.GetValue(CommandProperty) as ICommand;
        }

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region Event
        public static readonly DependencyProperty EventProperty = DependencyProperty.RegisterAttached(
            "Event",
            typeof(string),
            typeof(CommandBehavior),
            new PropertyMetadata(OnEventChanged)
            );

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindEvent(d, e.NewValue as string);
        }

        public static string GetEvent(DependencyObject d)
        {
            return d.GetValue(EventProperty) as string;
        }

        public static void SetEvent(DependencyObject d, string value)
        {
            d.SetValue(EventProperty, value);
        }

        private static void BindEvent(DependencyObject owner, string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            var eventInfo = owner.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);

            if (eventInfo == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve event name {0}", eventName));
            }

            var types = typeof(CommandBehavior).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo method = null;

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(true);

                if (attributes.OfType<EventRaiseAttribute>().Any())
                {
                    method = type;
                    break;
                }
            }

            if (method == null)
            {
                Debug.Assert(false, string.Format("invalid method type. type = {0}", eventName));

                return;
            }

            var eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, method);
            owner.SetValue(EventHandlerProperty, eventHandler);

            //Register the handler to the Event 
            eventInfo.AddEventHandler(owner, eventHandler);
        }

        [EventRaise]
        private void OnEventRaised(object sender, EventArgs e)
        {
            var dependencyObject = sender as DependencyObject;

            if (dependencyObject == null)
            {
                return;
            }

            var command = dependencyObject.GetValue(CommandProperty) as ICommand;

            if (command == null)
            {
                return;
            }

            if (command.CanExecute(null) == false)
            {
                return;
            }

            command.Execute(e);
        }
        #endregion

        #region EventHandler 
        public static readonly DependencyProperty EventHandlerProperty = DependencyProperty.RegisterAttached(
            "EventHandler",
            typeof(Delegate),
            typeof(CommandBehavior)
            );

        public static Delegate GetEventHandler(DependencyObject d)
        {
            return d.GetValue(EventHandlerProperty) as Delegate;
        }

        public static void SetEventHandler(DependencyObject d, Delegate value)
        {
            d.SetValue(EventHandlerProperty, value);
        }
        #endregion
    }
}
