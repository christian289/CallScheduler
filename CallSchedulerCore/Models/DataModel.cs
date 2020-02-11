using CallSchedulerCore.Base;
using System;
using System.Windows.Media;

namespace CallSchedulerCore.Models
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



        #region Item 색상
        private Brush _ItemColor = Brushes.DarkSeaGreen;

        public Brush ItemColor
        {
            get => _ItemColor;
            set
            {
                if (_ItemColor != value)
                {
                    _ItemColor = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
