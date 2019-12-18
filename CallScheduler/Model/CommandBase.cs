using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CallScheduler.Model
{
    /// <summary>
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
    /// 
    /// </summary>
    public class CommandBase : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;
        private readonly Action<string> execute_strParam1;

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">indicate an execute function</param>
        public CommandBase(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">execute function </param>
        /// <param name="canExecute">can execute function</param>
        public CommandBase(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public CommandBase(Action<string> execute_strParam1)
        {
            this.execute_strParam1 = execute_strParam1;
        }

        /// <summary>
        /// can executes event handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// implement of icommand can execute method
        /// </summary>
        /// <param name="o">parameter by default of icommand interface</param>
        /// <returns>can execute or not</returns>
        public bool CanExecute(object o)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute();
        }

        /// <summary>
        /// implement of icommand interface execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        public void Execute(object o)
        {
            execute();
        }

        public void Execute_strParam1(string str)
        {
            execute_strParam1(str);
        }
        /*
        /// <summary>
        /// raise ca excute changed when property changed
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if(this.CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }*/
    }
}
