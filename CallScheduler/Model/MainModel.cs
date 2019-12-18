﻿using CallScheduler.Global;
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
        #region 버튼 컨트롤러
        private bool _NewButtonController = true;

        public bool NewButtonController
        {
            get => _NewButtonController;
            set
            {
                _NewButtonController = value;
                OnPropertyChanged();
            }
        }

        private bool _EditButtonController = true;

        public bool EditButtonController
        {
            get => _EditButtonController;
            set
            {
                _EditButtonController = value;
                OnPropertyChanged();
            }
        }

        private bool _DeleteButtonController = true;

        public bool DeleteButtonController
        {
            get => _DeleteButtonController;
            set
            {
                _DeleteButtonController = value;
                OnPropertyChanged();
            }
        }

        private bool _SaveButtonController = true;

        public bool SaveButtonController
        {
            get => _SaveButtonController;
            set
            {
                _SaveButtonController = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 텍스트 박스 컨트롤러
        private bool _NameTextboxController = false;

        public bool NameTextboxController
        {
            get => _NameTextboxController;
            set
            {
                _NameTextboxController = value;
                OnPropertyChanged();
            }
        }

        private bool _PhoneNumberTextboxController = false;

        public bool PhoneNumberTextboxController
        {
            get => _PhoneNumberTextboxController;
            set
            {
                _PhoneNumberTextboxController = value;
                OnPropertyChanged();
            }
        }

        private bool _AlarmTimeTextboxController = false;

        public bool AlarmTimeTextboxController
        {
            get => _AlarmTimeTextboxController;
            set
            {
                _AlarmTimeTextboxController = value;
                OnPropertyChanged();
            }
        }

        private bool _MemoTextboxController = false;

        public bool MemoTextboxController
        {
            get => _MemoTextboxController;
            set
            {
                _MemoTextboxController = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 수정 모드 Flag
        public enum CRUDmode
        {
            Create,
            Read,
            Update,
            Delete
        }

        private CRUDmode _Mode = CRUDmode.Read;

        public CRUDmode Mode
        {
            get => _Mode;
            set
            {
                _Mode = value;

                switch (_Mode)
                {
                    case CRUDmode.Create:
                        NewButtonController = true;
                        EditButtonController = false;
                        DeleteButtonController = false;
                        SaveButtonController = false;
                        NameTextboxController = true;
                        PhoneNumberTextboxController = true;
                        AlarmTimeTextboxController = true;
                        MemoTextboxController = true;
                        break;
                    case CRUDmode.Read:
                        NewButtonController = true;
                        EditButtonController = true;
                        DeleteButtonController = true;
                        SaveButtonController = true;
                        NameTextboxController = false;
                        PhoneNumberTextboxController = false;
                        AlarmTimeTextboxController = false;
                        MemoTextboxController = false;
                        break;
                    case CRUDmode.Update:
                        NewButtonController = false;
                        EditButtonController = true;
                        DeleteButtonController = false;
                        SaveButtonController = false;
                        NameTextboxController = true;
                        PhoneNumberTextboxController = true;
                        AlarmTimeTextboxController = true;
                        MemoTextboxController = true;
                        break;
                    case CRUDmode.Delete:
                        // 작업 없음.
                        break;
                }

                OnPropertyChanged();
            }
        }
        #endregion

        #region XML 데이터 파일 경로
        private string _SourceFilePath = string.Empty;

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

        #region 알람 추가 버튼명
        private string _AddButtonName = "알람 추가";

        public string AddButtonName
        {
            get => _AddButtonName;
            set
            {
                _AddButtonName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 알람 수정 버튼명
        private string _EditButtonName = "알람 수정";

        public string EditButtonName
        {
            get => _EditButtonName;
            set
            {
                _EditButtonName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// 고객명단
        /// </summary>
        public ObservableCollection<DataModel> Model { get; } = new ObservableCollection<DataModel>();

        /// <summary>
        /// 고객명단 보조
        /// </summary>
        public ListViewModel LvModel { get; set; } = new ListViewModel();

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
            if (Mode.Equals(CRUDmode.Read))
            {
                AddButtonName = "추가 완료";
                Mode = CRUDmode.Create;

                DataModel obj = new DataModel();
                Model.Add(obj);
                LvModel.SelectedItem = obj;
                LvModel.SelectedIndexNumber = Model.IndexOf(obj) + 1;
            }
            else
            {
                AddButtonName = "알람 추가";
                Mode = CRUDmode.Read;
            }
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
            if (Mode.Equals(CRUDmode.Read))
            {
                EditButtonName = "수정 완료";
                Mode = CRUDmode.Update;
            }
            else
            {
                EditButtonName = "알람 수정";
                Mode = CRUDmode.Read;
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
            if (LvModel.SelectedItem != null)
            {
                Model.Remove(LvModel.SelectedItem as DataModel);
            }
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
