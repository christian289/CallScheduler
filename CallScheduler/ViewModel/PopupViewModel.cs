using CallScheduler.Base;
using CallScheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallScheduler.ViewModel
{
    public class PopupViewModel : ModelBase
    {
        private string _Name = string.Empty;

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _PhoneNumber = string.Empty;

        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _Memo = string.Empty;

        public string Memo
        {
            get => _Memo;
            set
            {
                _Memo = value;
                OnPropertyChanged();
            }
        }

        public PopupViewModel(DataModel obj)
        {
            Name = obj.Name;
            PhoneNumber = obj.PhoneNumber;
            Memo = obj.Memo;
        }

        private ICommand _OKCommand;

        public ICommand OKCommand
        {
            get
            {
                return _OKCommand ?? (_OKCommand = new CommandBase(OK, CanExecute_OK, true));
            }
        }

        private void OK()
        {

        }

        private bool CanExecute_OK()
        {
            return true;
        }
    }
}
