using CallScheduler.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallScheduler.Model
{
    public class MainModel : ModelBase
    {
        private enum CRUDmode
        {
            Create,
            Read,
            Update,
            Delete
        }

        #region 수정 모드 Flag
        private bool _IsEditorMode = false;

        public bool IsEditorMode
        {
            get => _IsEditorMode;
            set
            {
                _IsEditorMode = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region XML 데이터 파일 경로
        private string _SourceFilePath = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string SourceFilePath
        {
            get => _SourceFilePath;
            set
            {
                _SourceFilePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 알람 수정 버튼명
        private string _ButtonName = "알람 수정";

        public string ButtonName
        {
            get => _ButtonName;
            set
            {
                _ButtonName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private ObservableCollection<DataModel> _Model = new ObservableCollection<DataModel>();

        public ObservableCollection<DataModel> Model
        {
            get
            {
                return _Model;
            }
        }

        public MainModel()
        {
            // WPF에서 MVVM Pattern을 사용하면
            // 프로그램 시작 시 XAML에서 Binding Property를 사용하기 때문에 초기화를
            // 생성자가 아닌 맴버변수에 해주는게 좋다.
        }

        #region 알람 추가
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
            Model.Add(new DataModel());
        }
        #endregion

        #region 알람 수정
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
            if (IsEditorMode) // 수정 모드
            {
                ButtonName = "알람 수정";
                IsEditorMode = false;
            }
            else // 비 수정 모드
            {
                ButtonName = "수정 완료";
                IsEditorMode = true;
            }
        }
        #endregion

        #region 알람 삭제
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
        #endregion

        #region 알람 저장
        private ICommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new CommandBase(Save));
            }
        }

        private void Save(string FilePath)
        {
            DataXML.XmlSave(new List<DataModel>(Model), FilePath);
        }
        #endregion
    }
}
