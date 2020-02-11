using CallSchedulerCore.Base;
using CallSchedulerCore.Global;
using CallSchedulerCore.Helper;
using CallSchedulerCore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CallSchedulerCore.ViewModels
{
    public partial class MainViewModel : ModelBase
    {
        #region Property

        #region 텍스트 박스 컨트롤러
        private bool _NameTextboxController = false;

        public bool NameTextboxController
        {
            get => _NameTextboxController;
            set => SetProperty(ref _NameTextboxController, value);
        }

        private bool _PhoneNumberTextboxController = false;

        public bool PhoneNumberTextboxController
        {
            get => _PhoneNumberTextboxController;
            set => SetProperty(ref _PhoneNumberTextboxController, value);
        }

        private bool _AlarmTimeTextboxController = false;

        public bool AlarmTimeTextboxController
        {
            get => _AlarmTimeTextboxController;
            set => SetProperty(ref _AlarmTimeTextboxController, value);
        }

        private bool _MemoTextboxController = false;

        public bool MemoTextboxController
        {
            get => _MemoTextboxController;
            set => SetProperty(ref _MemoTextboxController, value);
        }
        #endregion

        #region 수정 모드 Flag
        public enum CRUDmode
        {
            Create,
            Read,
            Update,
            Delete,
            Running
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
                        NameTextboxController = true;
                        PhoneNumberTextboxController = true;
                        AlarmTimeTextboxController = true;
                        MemoTextboxController = true;
                        break;
                    case CRUDmode.Read:
                        NameTextboxController = false;
                        PhoneNumberTextboxController = false;
                        AlarmTimeTextboxController = false;
                        MemoTextboxController = false;
                        break;
                    case CRUDmode.Update:
                        NameTextboxController = true;
                        PhoneNumberTextboxController = true;
                        AlarmTimeTextboxController = true;
                        MemoTextboxController = true;
                        break;
                    case CRUDmode.Delete:
                        // 작업 없음.
                        break;
                    case CRUDmode.Running:
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
            set => SetProperty(ref _SourceFilePath, value);
        }
        #endregion

        #region 알람 추가 버튼명
        private string _AddButtonName = "알람 추가";

        public string AddButtonName
        {
            get => _AddButtonName;
            set => SetProperty(ref _AddButtonName, value);
        }
        #endregion

        #region 알람 수정 버튼명
        private string _EditButtonName = "알람 수정";

        public string EditButtonName
        {
            get => _EditButtonName;
            set => SetProperty(ref _EditButtonName, value);
        }
        #endregion

        #region 고객명단
        private ObservableCollection<DataModel> _Model = new ObservableCollection<DataModel>();

        public ObservableCollection<DataModel> Model
        {
            get => _Model;
            set => SetProperty(ref _Model, value);
        }
        #endregion

        #region 불러온 이미지
        private BitmapSource _LoadedImage;

        public BitmapSource LoadedImage
        {
            get => _LoadedImage;
            set => SetProperty(ref _LoadedImage, value);
        }
        #endregion

        #region 입력한 텍스트
        private string _InputText = string.Empty;

        public string InputText
        {
            get => _InputText;
            set => SetProperty(ref _InputText, value);
        }
        #endregion

        #region 찾을 카카오톡 채팅방 이름 키워드
        private string _TargetKeyword = string.Empty;

        public string TargetKeyword
        {
            get => _TargetKeyword;
            set => SetProperty(ref _TargetKeyword, value);
        }
        #endregion

        #region 팝업 오픈 플래그

        #region 알람 오픈
        private bool _PpOpen = false;

        public bool PpOpen
        {
            get => _PpOpen;
            set => SetProperty(ref _PpOpen, value);
        }
        #endregion

        #region 날짜 선택
        private bool _PpDTPAlarmTime = false;

        public bool PpDTPAlarmTime
        {
            get => _PpDTPAlarmTime;
            set => SetProperty(ref _PpDTPAlarmTime, value);
        }

        private bool _PpTextShowing = false;

        public bool PpTextShowing
        {
            get => _PpTextShowing;
            set => SetProperty(ref _PpTextShowing, value);
        }

        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get => _SelectedDate;
            set => SetProperty(ref _SelectedDate, value);
        }
        #endregion

        #region 알람 상세 팝업
        private bool _PpDetailInfoView = false;

        public bool PpDetailInfoView
        {
            get => _PpDetailInfoView;
            set => SetProperty(ref _PpDetailInfoView, value);
        }
        #endregion

        #endregion

        #region 알람 발생 시 팝업 프로퍼티
        private string _PpAlarmName = string.Empty;

        public string PpAlarmName
        {
            get => _PpAlarmName;
            set => SetProperty(ref _PpAlarmName, value);
        }

        private string _PpAlarmPhoneNumber = string.Empty;

        public string PpAlarmPhoneNumber
        {
            get => _PpAlarmPhoneNumber;
            set => SetProperty(ref _PpAlarmPhoneNumber, value);
        }

        private DateTime _PpAlarmDateTime = new DateTime();

        public DateTime PpAlarmDateTime
        {
            get => _PpAlarmDateTime;
            set => SetProperty(ref _PpAlarmDateTime, value);
        }

        private string _PpAlarmMemo = string.Empty;

        public string PpAlarmMemo
        {
            get => _PpAlarmMemo;
            set => SetProperty(ref _PpAlarmMemo, value);
        }
        #endregion

        /// <summary>
        /// 고객명단 보조
        /// </summary>
        public ListViewModel LvModel { get; set; } = new ListViewModel();

        #region 알람 시작버튼 문구변경
        private string _AlarmStateString = "알람시작";

        public string AlarmStateString
        {
            get => _AlarmStateString;
            set => SetProperty(ref _AlarmStateString, value);
        }
        #endregion

        #region 고객 명단 사용가능 여부
        private bool _LvGuestListEnable = true;

        public bool LvGuestListEnable
        {
            get => _LvGuestListEnable;
            set => SetProperty(ref _LvGuestListEnable, value);
        }
        #endregion

        #endregion

        #region Field

        private CancellationTokenSource tokenSource;
        private List<Task<bool>> LiAlarmList;

        #endregion

        public MainViewModel()
        {
            // WPF에서 MVVM Pattern을 사용하면
            // 프로그램 시작 시 XAML에서 Binding Property를 사용하기 때문에 초기화를
            // 생성자가 아닌 맴버변수에 해주는게 좋다.

            tokenSource = new CancellationTokenSource();
            LiAlarmList = new List<Task<bool>>();

            Loaded();
        }

        #region 기능 함수
        public void Loaded()
        {
            SelectedDate = DateTime.Now;

            SourceFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Data.xml";
            Model = new ObservableCollection<DataModel>(DataXML.XmlLoad(SourceFilePath));

            foreach (DataModel obj in Model)
            {
                OlderAlarmCheck(obj);
            }
        }

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

        private void SendText(IntPtr hEdit, string SendText)
        {
            WindowsAPI.SendMessage(hEdit, WindowsAPI.WM_SETTEXT, IntPtr.Zero, SendText);
            //WindowsAPI.PostMessage(hEdit, WindowsAPI.WM_KEYDOWN, WindowsAPI.VK_ENTER, IntPtr.Zero); // 이미지 전송과 함께 사용할 경우 동작하지 않음.
            //WindowsAPI.PostMessage(hEdit, WindowsAPI.WM_KEYUP, WindowsAPI.VK_ENTER, IntPtr.Zero); // 이미지 전송과 함께 사용할 경우 동작하지 않음.
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
            WindowsAPI.keybd_event(WindowsAPI.VK_ENTER, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY | WindowsAPI.KEYEVENTF_KEYUP, 0);
        }

        private BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private bool OlderAlarmCheck(DataModel obj)
        {
            DateTime dtCurTime = DateTime.Now;

            if ((dtCurTime - obj.AlarmTime).TotalSeconds >= 0)
            {
                obj.ItemColor = System.Windows.Media.Brushes.Red;
                return true;
            }
            else
            {
                obj.ItemColor = System.Windows.Media.Brushes.DarkSeaGreen;
                return false;
            }
        }
        #endregion
    }
}
