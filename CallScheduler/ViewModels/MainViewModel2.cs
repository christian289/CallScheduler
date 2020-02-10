using CallScheduler.Base;
using CallScheduler.Global;
using CallScheduler.Helper;
using CallScheduler.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CallScheduler.ViewModel
{
    public class MainViewModel2 : ModelBase
    {
        private Property _This = new Property();

        public Property This
        {
            get => _This;
            set
            {
                _This = value;
                OnPropertyChanged();
            }
        }

        public struct Property
        {
            [NotifyParentProperty(true)]
            public bool NameTextboxController { get; set; }
            [NotifyParentProperty(true)]
            public bool PhoneNumberTextboxController { get; set; }
            [NotifyParentProperty(true)]
            public bool AlarmTimeTextboxController { get; set; }
            [NotifyParentProperty(true)]
            public bool MemoTextboxController { get; set; }
            [NotifyParentProperty(true)]
            public string SourceFilePath { get; set; } // XML 데이터 파일 경로
            [NotifyParentProperty(true)]
            public string AddButtonName { get; set; } // 알람 추가 버튼명
            [NotifyParentProperty(true)]
            public string EditButtonName { get; set; } // 알람 수정 버튼명
            [NotifyParentProperty(true)]
            public ObservableCollection<DataModel> Model { get; set; } // 고객명단
            [NotifyParentProperty(true)]
            public BitmapSource LoadedImage { get; set; } // 불러온 이미지
            [NotifyParentProperty(true)]
            public string InputText { get; set; } // 입력한 텍스트
            [NotifyParentProperty(true)]
            public string TargetKeyword { get; set; } // 찾을 카카오톡 채팅방 이름 키워드
            [NotifyParentProperty(true)]
            public bool PpOpen { get; set; } // 알람 팝업 오픈
            [NotifyParentProperty(true)]
            public bool PpDTPAlarmTime { get; set; } // 날짜 선택 팝업 오픈
            [NotifyParentProperty(true)]
            public bool PpTextShowing { get; set; } // 날짜 선택 버튼 텍스트
            [NotifyParentProperty(true)]
            public DateTime SelectedDate { get; set; } // 선택 날짜
            [NotifyParentProperty(true)]
            public bool PpDetailInfoView { get; set; } // 알람 상세 팝업
            [NotifyParentProperty(true)]
            public string PpAlarmName { get; set; } // 알람 정보
            [NotifyParentProperty(true)]
            public string PpAlarmPhoneNumber { get; set; } // 알람 정보
            [NotifyParentProperty(true)]
            public DateTime PpAlarmDateTime { get; set; } // 알람 정보
            [NotifyParentProperty(true)]
            public string PpAlarmMemo { get; set; } // 알람 정보
            [NotifyParentProperty(true)]
            public string AlarmStateString { get; set; } // 알람 시작버튼 문구변경
            [NotifyParentProperty(true)]
            public bool LvGuestListEnable { get; set; } // 고객 명단 사용가능 여부
        }

        public void SetProperty(Property _property, string propertyName, object setterValue)
        {
            FieldInfo fi = typeof(Property).GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            TypedReference tr = __makeref(_property);
            fi.SetValueDirect(tr, setterValue);
        }

        public void Init()
        {
            SetProperty(This, nameof(Property.NameTextboxController), false);
            SetProperty(This, nameof(Property.PhoneNumberTextboxController), false);
            SetProperty(This, nameof(Property.AlarmTimeTextboxController), false);
            SetProperty(This, nameof(Property.MemoTextboxController), false);
            SetProperty(This, nameof(Property.SourceFilePath), string.Empty);
            SetProperty(This, nameof(Property.AddButtonName), "알람 추가");
            SetProperty(This, nameof(Property.EditButtonName), "알람 수정");
            SetProperty(This, nameof(Property.Model), new ObservableCollection<DataModel>());
            SetProperty(This, nameof(Property.LoadedImage), null);
            SetProperty(This, nameof(Property.InputText), string.Empty);
            SetProperty(This, nameof(Property.TargetKeyword), string.Empty);
            SetProperty(This, nameof(Property.PpOpen), false);
            SetProperty(This, nameof(Property.PpDTPAlarmTime), false);
            SetProperty(This, nameof(Property.PpTextShowing), false);
            SetProperty(This, nameof(Property.SelectedDate), null);
            SetProperty(This, nameof(Property.PpDetailInfoView), false);
            SetProperty(This, nameof(Property.PpAlarmName), string.Empty);
            SetProperty(This, nameof(Property.PpAlarmPhoneNumber), string.Empty);
            SetProperty(This, nameof(Property.PpAlarmDateTime), new DateTime());
            SetProperty(This, nameof(Property.PpAlarmMemo), string.Empty);
            SetProperty(This, nameof(Property.AlarmStateString), "알람시작");
            SetProperty(This, nameof(Property.LvGuestListEnable), true);
        }

        #region Property

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
                        SetProperty(This, nameof(Property.NameTextboxController), true);
                        SetProperty(This, nameof(Property.PhoneNumberTextboxController), true);
                        SetProperty(This, nameof(Property.AlarmTimeTextboxController), true);
                        SetProperty(This, nameof(Property.MemoTextboxController), true);
                        break;
                    case CRUDmode.Read:
                        SetProperty(This, nameof(Property.NameTextboxController), false);
                        SetProperty(This, nameof(Property.PhoneNumberTextboxController), false);
                        SetProperty(This, nameof(Property.AlarmTimeTextboxController), false);
                        SetProperty(This, nameof(Property.MemoTextboxController), false);
                        break;
                    case CRUDmode.Update:
                        SetProperty(This, nameof(Property.NameTextboxController), true);
                        SetProperty(This, nameof(Property.PhoneNumberTextboxController), true);
                        SetProperty(This, nameof(Property.AlarmTimeTextboxController), true);
                        SetProperty(This, nameof(Property.MemoTextboxController), true);
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

        /// <summary>
        /// 고객명단 보조
        /// </summary>
        public ListViewModel LvModel { get; set; } = new ListViewModel();

        #endregion

        #region Field

        private CancellationTokenSource tokenSource;
        private List<Task<bool>> LiAlarmList;

        #endregion

        public MainViewModel2()
        {
            // WPF에서 MVVM Pattern을 사용하면
            // 프로그램 시작 시 XAML에서 Binding Property를 사용하기 때문에 초기화를
            // 생성자가 아닌 맴버변수에 해주는게 좋다.

            tokenSource = new CancellationTokenSource();
            LiAlarmList = new List<Task<bool>>();
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

                return _NewCommand ?? (_NewCommand = new CommandBase(New, CanExecute_New, false));
            }
        }

        private void New()
        {
            if (Mode.Equals(CRUDmode.Read))
            {
                SetProperty(This, nameof(Property.AddButtonName), "추가 완료");
                Mode = CRUDmode.Create;

                DataModel obj = new DataModel
                {
                    AlarmTime = DateTime.Now
                };
                This.Model.Add(obj);
                LvModel.SelectedItem = obj;
                LvModel.SelectedIndexNumber = This.Model.IndexOf(obj) + 1;
                SetProperty(This, nameof(Property.PpTextShowing), false);
                SetProperty(This, nameof(Property.LvGuestListEnable), false);
            }
            else
            {
                SetProperty(This, nameof(Property.AddButtonName), "알람 추가");
                Mode = CRUDmode.Read;
                SetProperty(This, nameof(Property.LvGuestListEnable), true);

                MessageBox.Show("추가 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanExecute_New()
        {
            if (Mode == CRUDmode.Running || Mode == CRUDmode.Update)
            {
                return false;
            }
            else
            {
                return true;
            }
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

                return _EditCommand ?? (_EditCommand = new CommandBase(Edit, CanExecute_Edit, false));
            }
        }

        private void Edit()
        {
            if (Mode.Equals(CRUDmode.Read))
            {
                SetProperty(This, nameof(Property.EditButtonName), "수정 완료");
                Mode = CRUDmode.Update;
            }
            else
            {
                SetProperty(This, nameof(Property.EditButtonName), "알람 수정");
                Mode = CRUDmode.Read;

                MessageBox.Show("수정 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanExecute_Edit()
        {
            if (Mode == CRUDmode.Running || Mode == CRUDmode.Create || LvModel.SelectedItem is null || This.Model.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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

                return _DeleteCommand ?? (_DeleteCommand = new CommandBase(Delete, CanExecute_Delete, false));
            }
        }

        private void Delete()
        {
            // 정말 삭제하겠는지 묻는 메세지 박스
            if (!(LvModel.SelectedItem is null))
            {
                This.Model.Remove(LvModel.SelectedItem as DataModel);
            }
        }

        private bool CanExecute_Delete()
        {
            if (Mode == CRUDmode.Running || Mode == CRUDmode.Create || Mode == CRUDmode.Update || LvModel.SelectedItem is null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
                    param => Save(This.SourceFilePath), param => CanExecute_Save(), false));
            }
        }

        private void Save(string FilePath)
        {
            if (DataXML.XmlSave(new List<DataModel>(This.Model), FilePath))
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
            if (Mode == CRUDmode.Running || Mode == CRUDmode.Create || Mode == CRUDmode.Update)
            {
                return false;
            }
            else
            {
                return true;
            }
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
                SetProperty(This, nameof(Property.LoadedImage), BitmapToBitmapSource(_Image));
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
                WindowInfo.FindTargetHandle(This.TargetKeyword);
                string strMsg = string.Format("메세지를 전송할 화면이 {0}개 입니다. 전송하시겠습니까?", WindowInfo.TargetHandle.Count);

                if (MessageBox.Show(strMsg, "알람", MessageBoxButton.OKCancel, MessageBoxImage.Question).Equals(MessageBoxResult.OK))
                {
                    Clipboard.SetImage(This.LoadedImage); // 클립보드에 이미지 복사

                    try
                    {
                        foreach (KeyValuePair<IntPtr, string> item in WindowInfo.TargetHandle)
                        {
                            IntPtr ChildHandle = IsKaKaoTalkOpen(item.Value);
                            ImgClipBoardPaste(ChildHandle);
                            SendText(ChildHandle, This.InputText);
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
            SetProperty(This, nameof(Property.PpAlarmName), string.Empty);
            SetProperty(This, nameof(Property.PpAlarmDateTime), new DateTime());
            SetProperty(This, nameof(Property.PpAlarmPhoneNumber), string.Empty);
            SetProperty(This, nameof(Property.PpAlarmMemo), string.Empty);
            SetProperty(This, nameof(Property.PpOpen), false);
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
            if (!This.PpDTPAlarmTime) // 팝업 열기
            {
                SetProperty(This, nameof(Property.PpDTPAlarmTime), true);
                SetProperty(This, nameof(Property.SelectedDate), ((DataModel)LvModel.SelectedItem).AlarmTime);
            }
            else // 팝업 닫기
            {
                SetProperty(This, nameof(Property.PpTextShowing), true);
                SetProperty(This, nameof(Property.PpDTPAlarmTime), false);
                ((DataModel)LvModel.SelectedItem).AlarmTime = This.SelectedDate;
                OlderAlarmCheck((DataModel)LvModel.SelectedItem);
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
            if (!This.PpDetailInfoView)
            {
                SetProperty(This, nameof(Property.PpDetailInfoView), true);
            }
            else
            {
                SetProperty(This, nameof(Property.PpDetailInfoView), false);
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
            SetProperty(This, nameof(Property.PpDetailInfoView), false);
        }

        private bool CanExecute_DetailInfoViewClose()
        {
            return true;
        }
        #endregion

        #region 알람 시작
        private ICommand _BtnAlarmStateCommand;

        public ICommand BtnAlarmStateCommand
        {
            get
            {
                return _BtnAlarmStateCommand ?? (_BtnAlarmStateCommand = new CommandBase(BtnAlarmState, CanExecute_BtnAlarmState, false));
            }
        }

        private void BtnAlarmState()
        {
            if (Mode == CRUDmode.Read)
            {
                SetProperty(This, nameof(Property.AlarmStateString), "알람중지");
                tokenSource = new CancellationTokenSource();

                foreach (DataModel obj in This.Model)
                {
                    if (OlderAlarmCheck(obj))
                    {
                        continue;
                    }

                    Task<bool> task = new Task<bool>(() => AlarmTaskStart(obj, tokenSource.Token), tokenSource.Token);
                    
                    task.ContinueWith((returnValue) => {
                        if (returnValue.Result)
                        {
                            LiAlarmList.Remove(task);
                            SetProperty(This, nameof(Property.PpOpen), true);
                        }

                        if (LiAlarmList.Count == 0)
                        {
                            SetProperty(This, nameof(Property.AlarmStateString), "알람시작");
                            Mode = CRUDmode.Read;
                        }
                    });

                    LiAlarmList.Add(task);
                    task.Start();
                }

                Mode = CRUDmode.Running;
            }
            else
            {
                SetProperty(This, nameof(Property.AlarmStateString), "알람시작");
                Mode = CRUDmode.Read;
                tokenSource.Cancel();
                LiAlarmList = new List<Task<bool>>();
            }
        }

        private bool CanExecute_BtnAlarmState()
        {
            if ((Mode == CRUDmode.Read && This.Model.Count > 0) || Mode == CRUDmode.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AlarmTaskStart(DataModel pobj, CancellationToken token)
        {
            while (true)
            {
                // DateTime.Now와 비교할 때, DateTime.Now는 계속 변하는 값이므로 고정값 DateTime 객체와 동등비교 시 원하는 결과를 얻지 못한다.
                // 따라서 아래와 같이 처리하거나, DateTime을 String으로 바꿔서 비교하거나, DateTime의 시간 관련 프로퍼티를 하나씩 비교해야한다.
                if (Math.Abs((DateTime.Now - pobj.AlarmTime).TotalSeconds) < 1)
                {
                    SetProperty(This, nameof(Property.PpAlarmName), pobj.Name);
                    SetProperty(This, nameof(Property.PpAlarmPhoneNumber), pobj.PhoneNumber);
                    SetProperty(This, nameof(Property.PpAlarmDateTime), pobj.AlarmTime);
                    SetProperty(This, nameof(Property.PpAlarmMemo), pobj.Memo);

                    pobj.ItemColor = System.Windows.Media.Brushes.Red;

                    return true;
                }

                if (token.IsCancellationRequested)
                {
                    return false;
                }

                Task.Delay(100);
            }
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
            SetProperty(This, nameof(Property.SelectedDate), DateTime.Now);
            SetProperty(This, nameof(Property.SourceFilePath), Directory.GetCurrentDirectory() + @"\Data.xml");
            SetProperty(This, nameof(Property.Model), new ObservableCollection<DataModel>(DataXML.XmlLoad(This.SourceFilePath)));

            foreach(DataModel obj in This.Model)
            {
                OlderAlarmCheck(obj);
            }
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
                return _LvGuestListSelectionChangedCommand ?? (_LvGuestListSelectionChangedCommand = new CommandBase<object>(LvGuestListSelectionChanged, CanExecute_LvGuestListSelectionChanged, false));
            }
        }

        private void LvGuestListSelectionChanged(object args)
        {
            SetProperty(This, nameof(Property.PpTextShowing), true);
        }

        private bool CanExecute_LvGuestListSelectionChanged(object args)
        {
            if (This.Model.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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
