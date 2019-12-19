using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CallScheduler.Helper
{
    public static class WindowsAPI
    {
        #region 가상키보드 키코드
        public const int VK_LBUTTON = 0x01; // 마우스 왼쪽 버튼
        public const int VK_RBUTTON = 0x02; // 마우스 오른쪽 버튼
        public const int VK_MBUTTON = 0x04; // 마우스 중앙버튼
        public const int VK_XBUTTON1 = 0x05; // Windows 2000/XP 마우스 X1 버튼
        public const int VK_XBUTTON2 = 0x06; // Windows 2000/XP 마우스 X2 버튼
        public const int VK_BACK = 0x08; // Back Space
        public const int VK_TAB = 0x09; // Tab
        public const int VK_ENTER = 0x0D; // Enter
        public const int VK_RETURN = 0x0D; // Enter
        public const int VK_SHIFT = 0x10; //Shift
        public const int VK_CONTROL = 0x11; // Ctrl
        public const int VK_ALT = 0x12; // Alt
        public const int VK_PAUSE = 0x13; // Pause
        public const int VK_CAPITAL = 0x14; // Caps Lock
        public const int VK_HANENG = 0x15; // 한/영키
        public const int VK_HANJA = 0x19; // 한자키
        public const int VK_ESCAPE = 0x1B; // Esc
        public const int VK_SPACE = 0x20; // Space
        public const int VK_PRIOR = 0x21; // Page Up
        public const int VK_NEXT = 0x22; // Page Down
        public const int VK_END = 0x23; // End
        public const int VK_HOME = 0x24; // Home
        public const int VK_LEFT = 0x25; //←
        public const int VK_UP = 0x26;	//↑
        public const int VK_RIGHT = 0x27; //→
        public const int VK_DOWN = 0x28; //↓
        public const int VK_SNAPSHOT = 0x2C; //Print Screen
        public const int VK_INSERT = 0x2D; // Insert
        public const int VK_DELETE = 0x2E; //Delete
        public const int VK_NUM0 = 0x30; // 0 (키패드 0 아님)
        public const int VK_NUM1 = 0x31; // 1 (키패드 1 아님)
        public const int VK_NUM2 = 0x32; // 2 (키패드 2 아님)
        public const int VK_NUM3 = 0x33; // 3 (키패드 3 아님)
        public const int VK_NUM4 = 0x34; // 4 (키패드 4 아님)
        public const int VK_NUM5 = 0x35; // 5 (키패드 5 아님)
        public const int VK_NUM6 = 0x36; // 6 (키패드 6 아님)
        public const int VK_NUM7 = 0x37; // 7 (키패드 7 아님)
        public const int VK_NUM8 = 0x38; // 8 (키패드 8 아님)
        public const int VK_NUM9 = 0x39; // 9 (키패드 9 아님)
        public const int VK_A = 0x41; // A
        public const int VK_B = 0x42; // B
        public const int VK_C = 0x43; // C
        public const int VK_D = 0x44; // D
        public const int VK_E = 0x45; // E
        public const int VK_F = 0x46; // F
        public const int VK_G = 0x47; // G
        public const int VK_H = 0x48; // H
        public const int VK_I = 0x49; // I
        public const int VK_J = 0x4A; // J
        public const int VK_K = 0x4B; // K
        public const int VK_L = 0x4C; // L
        public const int VK_M = 0x4D; // M
        public const int VK_N = 0x4E; // N
        public const int VK_O = 0x4F; // O
        public const int VK_P = 0x50; // P
        public const int VK_Q = 0x51; // Q
        public const int VK_R = 0x52; // R
        public const int VK_S = 0x53; // S
        public const int VK_T = 0x54; // T
        public const int VK_U = 0x55; // U
        public const int VK_V = 0x56; // V
        public const int VK_W = 0x57; // W
        public const int VK_X = 0x58; // X
        public const int VK_Y = 0x59; // Y
        public const int VK_Z = 0x5A; // Z
        public const int VK_LWIN = 0x5B; // 왼쪽 Windows 키
        public const int VK_RWIN = 0x5C; // 오른쪽 Windows 키
        public const int VK_NUMPAD0 = 0x60; // 키패드 0
        public const int VK_NUMPAD1 = 0x61; // 키패드 1
        public const int VK_NUMPAD2 = 0x62; // 키패드 2
        public const int VK_NUMPAD3 = 0x63; // 키패드 3
        public const int VK_NUMPAD4 = 0x64; // 키패드 4
        public const int VK_NUMPAD5 = 0x65; // 키패드 5
        public const int VK_NUMPAD6 = 0x66; // 키패드 6
        public const int VK_NUMPAD7 = 0x67; // 키패드 7
        public const int VK_NUMPAD8 = 0x68; // 키패드 8
        public const int VK_NUMPAD9 = 0x69; // 키패드 9
        public const int VK_MULTIPLY = 0x6A; // 숫자 패드 *
        public const int VK_ADD = 0x6B; // 숫자 패드 +
        public const int VK_SEPARATOR = 0x6C; // 숫자 패드 Enter
        public const int VK_SUBTRACT = 0x6D; //	숫자 패드 -
        public const int VK_DECIMAL = 0x6E; // 숫자 패드 . 
        public const int VK_DIVIDE = 0x6F; //	숫자 패드 /
        public const int VK_F1 = 0x70; // F1
        public const int VK_F2 = 0x71; // F2
        public const int VK_F3 = 0x72; // F3
        public const int VK_F4 = 0x73; // F4
        public const int VK_F5 = 0x74; // F5
        public const int VK_F6 = 0x75; // F6
        public const int VK_F7 = 0x76; // F7
        public const int VK_F8 = 0x77; // F8
        public const int VK_F9 = 0x78; // F9
        public const int VK_F10 = 0x79; // F10
        public const int VK_F11 = 0x7A; // F11
        public const int VK_F12 = 0x7B; // F12
        public const int VK_NUMLOCK = 0x90; // Num Lock
        public const int VK_SCROLL = 0x91; // Scroll Lock
        public const int VK_LSHIFT = 0xA0; //  왼쪽 Shift
        public const int VK_RSHIFT = 0xA1; // 오른쪽 Shift
        public const int VK_LCONTROL = 0xA2; // 왼쪽 Ctrl
        public const int VK_RCONTROL = 0xA3; // 오른쪽 Ctrl
        public const int VK_LMENU = 0xA4; // 왼쪽 Alt
        public const int VK_RMENU = 0xA5; // 오른쪽 Alt
        public const int VK_OFF = 0xDF; // ?
        #endregion

        public const int KEYEVENTF_EXTENDEDKEY = 0x01;
        public const int KEYEVENTF_KEYUP = 0x02;
        public const int KEYEVENTF_SILENT = 0x04;

        public const int WM_SETTEXT = 0x0C;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd">메시지를 받을 윈도우 핸들</param>
        /// <param name="Msg">전달할 메시지</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);

        /// <summary>
        /// 윈도우 핸들로 WIndow 활성화하기
        /// </summary>
        /// <param name="hWnd">Window 핸들</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 활성화된 Window에 키보드 이벤트 출력
        /// </summary>
        /// <param name="bVk">가상키보드의 키코드</param>
        /// <param name="bScan">하드웨어 키코드</param>
        /// <param name="dwFlags">이벤트 플래그</param>
        /// <param name="dwExtraIfno">추가적인 키스트로크 데이터</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool keybd_event(int bVk, uint bScan, int dwFlags, int dwExtraIfno);
    }
}
