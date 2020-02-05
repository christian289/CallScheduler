using CallScheduler.Base;
using CallScheduler.Global;
using CallScheduler.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CallScheduler.ViewModel
{
    public partial class MainViewModel
    {

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
                LvGuestListEnable = false;
            }
            else
            {
                AddButtonName = "알람 추가";
                Mode = CRUDmode.Read;
                LvGuestListEnable = true;

                MessageBox.Show("추가 완료", "알람", MessageBoxButton.OK, MessageBoxImage.Information); // 나중에 프레젠터로 뺄수있으면 빼.
                // MessageBox 는 View로 보는사람도 있고 Code로 보는 사람도 있음. 나도 View인지 Code인지 판단 기준이 안서서 모르겠음.
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
            if (Mode == CRUDmode.Running || Mode == CRUDmode.Create || LvModel.SelectedItem is null || Model.Count <= 0)
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
                Model.Remove(LvModel.SelectedItem as DataModel);
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
                    param => Save(SourceFilePath), param => CanExecute_Save(), false));
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
                string strMsg = $"메세지를 전송할 화면이 {WindowInfo.TargetHandle.Count}개 입니다. 전송하시겠습니까?";

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
            PpAlarmName = string.Empty;
            PpAlarmDateTime = new DateTime();
            PpAlarmPhoneNumber = string.Empty;
            PpAlarmMemo = string.Empty;
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
            if (!PpDTPAlarmTime) // 팝업 열기
            {
                PpDTPAlarmTime = true;
                SelectedDate = ((DataModel)LvModel.SelectedItem).AlarmTime;
            }
            else // 팝업 닫기
            {
                PpTextShowing = true;
                PpDTPAlarmTime = false;
                ((DataModel)LvModel.SelectedItem).AlarmTime = SelectedDate;
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
                AlarmStateString = "알람중지";
                tokenSource = new CancellationTokenSource();

                foreach (DataModel obj in Model)
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
                            PpOpen = true;
                        }

                        if (LiAlarmList.Count == 0)
                        {
                            AlarmStateString = "알람시작";
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
                AlarmStateString = "알람시작";
                Mode = CRUDmode.Read;
                tokenSource.Cancel();
                LiAlarmList = new List<Task<bool>>();
            }
        }

        private bool CanExecute_BtnAlarmState()
        {
            if ((Mode == CRUDmode.Read && Model.Count > 0) || Mode == CRUDmode.Running)
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
                    PpAlarmName = pobj.Name;
                    PpAlarmPhoneNumber = pobj.PhoneNumber;
                    PpAlarmDateTime = pobj.AlarmTime;
                    PpAlarmMemo = pobj.Memo;

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
            PpTextShowing = true;
        }

        private bool CanExecute_LvGuestListSelectionChanged(object args)
        {
            if (Model.Count <= 0)
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
    }
}
