using CallScheduler.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallScheduler.Models
{
    /// <summary>
    /// CommandBase<T> 테스트 모델 객체
    /// 보고 따라만하고 사용하지는 않는다.
    /// </summary>
    class TestModel : ModelBase
    {
        private string _TestProperty = string.Empty;

        public string TestProperty
        {
            get => _TestProperty;
            set
            {
                _TestProperty = value;
                OnPropertyChanged();

                if (!ReferenceEquals(_TestCommand, null))
                {
                    _TestCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private CommandBase<object> _TestCommand;

        public ICommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new CommandBase<object>(
                    param => ExecuteTestCommand(), 
                    param => CanExecuteTestCommand(), 
                    true));
            }
        }

        private void ExecuteTestCommand()
        {

        }

        private bool CanExecuteTestCommand()
        {
            return !string.IsNullOrWhiteSpace(TestProperty);
        }
    }
}
