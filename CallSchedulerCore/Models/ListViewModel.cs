using CallSchedulerCore.Base;

namespace CallSchedulerCore.Models
{
    public class ListViewModel : ModelBase
    {
        private int _SelectedIndexNumber = -1;

        public int SelectedIndexNumber
        {
            get => _SelectedIndexNumber;
            set
            {
                if (_SelectedIndexNumber != value)
                {
                    _SelectedIndexNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _SelectedItem;

        public object SelectedItem
        {
            get => _SelectedItem;
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
