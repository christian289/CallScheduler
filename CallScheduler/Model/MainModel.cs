using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallScheduler.Model
{
    public class MainModel : ModelBase
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

        private string _AlarmTime = string.Empty;

        public string AlarmTime
        {
            get => _AlarmTime;
            set
            {
                _AlarmTime = value;
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

        private ICommand _NewCommand;

        public ICommand NewCommand
        {
            get
            {
                return _NewCommand ?? (_NewCommand = new CommandBase(New));
            }
        }

        private void New()
        {
            
        }


        private ICommand _EditCommand;

        public ICommand EditCommand
        {
            get
            {
                return _EditCommand ?? (_EditCommand = new CommandBase(Edit));
            }
        }

        private void Edit()
        {

        }

        private ICommand _DeleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new CommandBase(Delete));
            }
        }

        private void Delete()
        {

        }

        private ICommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new CommandBase(Save));
            }
        }

        private void Save()
        {

        }
    }
}
