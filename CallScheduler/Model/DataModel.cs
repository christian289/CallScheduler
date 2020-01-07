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
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _PhoneNumber = string.Empty;

        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                if (_PhoneNumber != value)
                {
                    _PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _AlarmTime = new DateTime();

        public DateTime AlarmTime
        {
            get => _AlarmTime;
            set
            {
                if (_AlarmTime != value)
                {
                    _AlarmTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _Memo = string.Empty;

        public string Memo
        {
            get => _Memo;
            set
            {
                if (_Memo != value)
                {
                    _Memo = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
