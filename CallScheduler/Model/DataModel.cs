using CallScheduler.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallScheduler.Model
{
    public class DataModel : ModelBase
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

        private DateTime _AlarmTime = new DateTime();

        public DateTime AlarmTime
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
    }
}
