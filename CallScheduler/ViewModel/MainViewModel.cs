using CallScheduler.Base;
using CallScheduler.Global;
using CallScheduler.Helper;
using CallScheduler.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CallScheduler.ViewModel
{
    public class MainViewModel : ModelBase
    {
        #region Property

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

        #region 고객명단
        private ObservableCollection<DataModel> _Model = new ObservableCollection<DataModel>();

        public ObservableCollection<DataModel> Model
        {
            get => _Model;
            set
            {
                _Model = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 불러온 이미지
        private BitmapSource _LoadedImage;

        public BitmapSource LoadedImage
        {
            get => _LoadedImage;
            set
            {
                _LoadedImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 입력한 텍스트
        private string _InputText = string.Empty;

        public string InputText
        {
            get => _InputText;
            set
            {
                _InputText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 찾을 카카오톡 채팅방 이름 키워드
        private string _TargetKeyword = string.Empty;

        public string TargetKeyword
        {
            get => _TargetKeyword;
            set
            {
                _TargetKeyword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 팝업 오픈 플래그

        #region 알람 오픈
        private bool _PpOpen = false;

        public bool PpOpen
        {
            get => _PpOpen;
            set
            {
                _PpOpen = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 날짜 선택
        private bool _PpDTPAlarmTime = false;

        public bool PpDTPAlarmTime
        {
            get => _PpDTPAlarmTime;
            set
            {
                _PpDTPAlarmTime = value;
                OnPropertyChanged();
            }
        }

        private bool _PpTextShowing = false;

        public bool PpTextShowing
        {
            get => _PpTextShowing;
            set
            {
                if (_PpTextShowing != value)
                {
                    _PpTextShowing = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get => _SelectedDate;
            set
            {
                if (_SelectedDate != value)
                {
                    _SelectedDate = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region 알람 상세 팝업
        private bool _PpDetailInfoView = false;

        public bool PpDetailInfoView
        {
            get => _PpDetailInfoView;
            set
            {
                _PpDetailInfoView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 팝업 정보
        /// </summary>
        public PopupModel PpModel { get; set; } = new PopupModel();

        /// <summary>
        /// 고객명단 보조
        /// </summary>
        public ListViewModel LvModel { get; set; } = new ListViewModel();

        #endregion

        public MainViewModel()
        {
            // WPF에서 MVVM Pattern을 사용하면
            // 프로그램 시작 시 XAML에서 Binding Property를 사용하기 때문에 초기화를
            // 생성자가 아닌 맴버변수에 해주는게 좋다.
        }

        #region ICommand

        #region 알람 추가
        private ICommand _NewCommand;

        public ICommand NewCommand
        {
            get
            {
                //일반적인 바인딩
                //return _NewCommand ?? (_NewCommand = new CommandBase(New));

                //return _NewCommand ?? (_NewCommand = new CommandBase<object>(
                //    param => New(), param => CanExecute_New(), true));

                return _NewCommand ?? (_NewCommand = new CommandBase(New, CanExecute_New, true));
            }
        }

        private void New()
        {
            if (Mode.Equals(CRUDmode.Read))
            {
                AddButtonName = "추가 완료";
                Mode = CRUDmode.Create;

                DataModel obj = new DataModel
                {
                    AlarmTime = DateTime.Now
                };
                Model.Add(obj);
                LvModel.SelectedItem = obj;
                LvModel.SelectedIndexNumber = Model.IndexOf(obj) + 1;
                PpTextShowing = false;
            }
            else
            {
                AddButtonName = "알람 추가";
                Mode = CRUDmode.Read;

                MessageBox.Show("추가 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanExecute_New()
        {
            return true;
        }
        #endregion

        #region 알람 수정
        private ICommand _EditCommand;

        public ICommand EditCommand
        {
            get
            {
                //return _EditCommand ?? (_EditCommand = new CommandBase(Edit));

                //return _EditCommand ?? (_EditCommand = new CommandBase<object>(
                //    param => Edit(), param => CanExecute_Edit(), true));

                return _EditCommand ?? (_EditCommand = new CommandBase(Edit, CanExecute_Edit, true));
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

                MessageBox.Show("수정 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanExecute_Edit()
        {
            return true;
        }
        #endregion

        #region 알람 삭제
        private ICommand _DeleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                //return _DeleteCommand ?? (_DeleteCommand = new CommandBase(Delete));

                //return _DeleteCommand ?? (_DeleteCommand = new CommandBase<object>(
                //    param => Delete(), param => CanExecute_Delete(), true));

                return _DeleteCommand ?? (_DeleteCommand = new CommandBase(Delete, CanExecute_Delete, true));
            }
        }

        private void Delete()
        {
            // 정말 삭제하겠는지 묻는 메세지 박스
            if (LvModel.SelectedItem != null)
            {
                Model.Remove(LvModel.SelectedItem as DataModel);
            }
        }

        private bool CanExecute_Delete()
        {
            return true;
        }
        #endregion

        #region 알람 저장
        private ICommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                //return _SaveCommand ?? (_SaveCommand = new CommandBase(Save));

                return _SaveCommand ?? (_SaveCommand = new CommandBase<string>(
                    param => Save(SourceFilePath), param => CanExecute_Save(), true));
            }
        }

        private void Save(string FilePath)
        {
            if (DataXML.XmlSave(new List<DataModel>(Model), FilePath))
            {
                MessageBox.Show("저장 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("저장 실패", "알람", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecute_Save()
        {
            return true;
        }
        #endregion

        #region 이미지 불러오기
        private ICommand _LoadingImageCommand;

        public ICommand LoadingImageCommand
        {
            get
            {
                return _LoadingImageCommand ?? (_LoadingImageCommand = new CommandBase(LoadingImage, CanExecute_LoadingImage, true));
            }
        }

        private void LoadingImage()
        {
            OpenFileDialog DlgImageFile = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "이미지 파일 (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|모든 파일 (*.*)|*.*",
                InitialDirectory = Directory.GetCurrentDirectory() + @"\Image"
            };

            if (DlgImageFile.ShowDialog() == true) 
            {
                Bitmap _Image = Image.FromFile(DlgImageFile.FileName, true) as Bitmap;
                LoadedImage = BitmapToBitmapSource(_Image);
            }

            // WPF에서는 OpenFileDialog가 System.WinMouws.Forms 네임스페이스가 아닌,
            // Microsoft.Win32 이기 때문에 Windows Forms에서 사용하던 OpenFileDialog가 아니다.
            // 이 OpenFileDialog는 ShowDialog() 시 return 값이 bool이 아닌 bool? 이므로 명시적으로 == true를 해줘야 한다.
        }

        private bool CanExecute_LoadingImage()
        {
            return true;
        }
        #endregion

        #region 전송
        private ICommand _SendCommand;

        public ICommand SendCommand
        {
            get
            {
                return _SendCommand ?? (_SendCommand = new CommandBase(Send, CanExecute_Send, true));
            }
        }

        private void Send()
        {
            if (WindowInfo.GetWindowHandleInfo())
            {
                WindowInfo.FindTargetHandle(TargetKeyword);
                string strMsg = string.Format("메세지를 전송할 화면이 {0}개 입니다. 전송하시겠습니까?", WindowInfo.TargetHandle.Count);

                if (MessageBox.Show(strMsg, "알람", MessageBoxButton.OKCancel, MessageBoxImage.Question).Equals(MessageBoxResult.OK))
                {
                    Clipboard.SetImage(LoadedImage); // 클립보드에 이미지 복사

                    try
                    {
                        foreach (KeyValuePair<IntPtr, string> item in WindowInfo.TargetHandle)
                        {
                            IntPtr ChildHandle = IsKaKaoTalkOpen(item.Value);
                            ImgClipBoardPaste(ChildHandle);
                            SendText(ChildHandle, InputText);
                        }
                    }
                    catch
                    {
                        return;
                    }

                    MessageBox.Show("전송이 완료되었습니다.", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // 창 핸들 로딩 실패
            }
        }

        private bool CanExecute_Send()
        {
            return true;
        }
        #endregion

        #region 알람 팝업 닫기
        private ICommand _PopupCloseCommand;

        public ICommand PopupCloseCommand
        {
            get
            {
                return _PopupCloseCommand ?? (_PopupCloseCommand = new CommandBase(PopupClose, CanExecute_PopupClose, true));
            }
        }

        private void PopupClose()
        {
            PpModel.Init();
            PpOpen = false;
        }

        private bool CanExecute_PopupClose()
        {
            return true;
        }
        #endregion

        #region 날짜 선택
        private ICommand _DateTimePickerPopupOpenCommand;

        public ICommand DateTimePickerPopupOpenCommand
        {
            get
            {
                return _DateTimePickerPopupOpenCommand ?? (_DateTimePickerPopupOpenCommand = new CommandBase(DateTimePickerPopupOpen, CanExecute_DateTimePickerPopupOpen, true));
            }
        }

        private void DateTimePickerPopupOpen()
        {
            if (!PpDTPAlarmTime)
            {
                PpDTPAlarmTime = true;
                SelectedDate = ((DataModel)LvModel.SelectedItem).AlarmTime;
            }
            else
            {
                PpTextShowing = true;
                PpDTPAlarmTime = false;
                ((DataModel)LvModel.SelectedItem).AlarmTime = SelectedDate;
            }
        }

        private bool CanExecute_DateTimePickerPopupOpen()
        {
            return true;
        }
        #endregion

        #region 상세 정보 팝업 열기
        private ICommand _DetailInfoViewOpenCommand;

        public ICommand DetailInfoViewOpenCommand
        {
            get
            {
                return _DetailInfoViewOpenCommand ?? (_DetailInfoViewOpenCommand = new CommandBase(DetailInfoViewOpen, CanExecute_DetailInfoViewOpen, true));
            }
        }

        private void DetailInfoViewOpen()
        {
            if (!PpDetailInfoView)
            {
                PpDetailInfoView = true;
            }
            else
            {
                PpDetailInfoView = false;
            }
        }

        private bool CanExecute_DetailInfoViewOpen()
        {
            return true;
        }
        #endregion

        #region 상세 정보 팝업 닫기
        private ICommand _DetailInfoViewCloseCommand;

        public ICommand DetailInfoViewCloseCommand
        {
            get
            {
                return _DetailInfoViewCloseCommand ?? (_DetailInfoViewCloseCommand = new CommandBase(DetailInfoViewClose, CanExecute_DetailInfoViewClose, true));
            }
        }

        private void DetailInfoViewClose()
        {
            PpDetailInfoView = false;
        }

        private bool CanExecute_DetailInfoViewClose()
        {
            return true;
        }
        #endregion

        #endregion

        #region Trigger

        #region MainView Loaded
        private ICommand _LoadedCommand;

        public ICommand LoadedCommand
        {
            get
            {
                return _LoadedCommand ?? (_LoadedCommand = new CommandBase<object>(Loaded, CanExecute_Loaded, true));
            }
        }

        private void Loaded(object args)
        {
            SelectedDate = DateTime.Now;

            SourceFilePath = Directory.GetCurrentDirectory() + @"\Data.xml";
            Model = new ObservableCollection<DataModel>(DataXML.XmlLoad(SourceFilePath));
        }

        private bool CanExecute_Loaded(object args)
        {
            return true;
        }
        #endregion

        #region LvGuestList SelectionChanged
        private ICommand _LvGuestListSelectionChangedCommand;

        public ICommand LvGuestListSelectionChangedCommand
        {
            get
            {
                return _LvGuestListSelectionChangedCommand ?? (_LvGuestListSelectionChangedCommand = new CommandBase<object>(LvGuestListSelectionChanged, CanExecute_LvGuestListSelectionChanged, true));
            }
        }

        private void LvGuestListSelectionChanged(object args)
        {
            PpTextShowing = true;
        }

        private bool CanExecute_LvGuestListSelectionChanged(object args)
        {
            return true;
        }
        #endregion

        #endregion

        #region 기능 함수
        /// <summary>
        /// PC 카카오톡 창 핸들 찾기
        /// </summary>
        /// <param name="RoomName"></param>
        /// <returns></returns>
        private IntPtr IsKaKaoTalkOpen(string RoomName)
        {
            IntPtr TargetWindowNameHnd = WindowsAPI.FindWindow(null, RoomName);

            if (TargetWindowNameHnd.Equals(IntPtr.Zero))
            {
                return IntPtr.Zero;
            }

            if (!WindowsAPI.IsWindow(TargetWindowNameHnd)) // 창 핸들인지 확인한다.
            {
                return IntPtr.Zero;
            }

            // 총 2개의 하위 다이얼로그가 있으므로 핸들을 각각 가져온다.
            IntPtr kakaoTextboxHandle = WindowsAPI.FindWindowEx(TargetWindowNameHnd, IntPtr.Zero, "RichEdit20W", null);

            return kakaoTextboxHandle;
        }

        /// <summary>
        /// 클립보드에 복사한 이미지를 대상 창 핸들에 Ctrl + V 이벤트 발생
        /// </summary>
        /// <param name="hEdit">대상 창 핸들</param>
        private void ImgClipBoardPaste(IntPtr hEdit)
        {
            // 나중에 마우스 잠금 해야 함.
            WindowsAPI.SetForegroundWindow(hEdit);
            WindowsAPI.keybd_event(WindowsAPI.VK_LCONTROL, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_V, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_LCONTROL, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_V, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
        }

        public void SendText(IntPtr hEdit, string SendText)
        {
            WindowsAPI.SendMessage(hEdit, WindowsAPI.WM_SETTEXT, IntPtr.Zero, SendText);
            //WindowsAPI.PostMessage(hEdit, WindowsAPI.WM_KEYDOWN, WindowsAPI.VK_ENTER, IntPtr.Zero); // 이미지 전송과 함께 사용할 경우 동작하지 않음.
            //WindowsAPI.PostMessage(hEdit, WindowsAPI.WM_KEYUP, WindowsAPI.VK_ENTER, IntPtr.Zero); // 이미지 전송과 함께 사용할 경우 동작하지 않음.
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
        }

        public BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        #endregion
    }
}
