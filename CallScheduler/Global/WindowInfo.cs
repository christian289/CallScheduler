using CallScheduler.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallScheduler.Global
{
    public static class WindowInfo
    {
        public static Dictionary<IntPtr, string> WindowsDic { get; private set; }

        public static Dictionary<IntPtr, string> TargetHandle { get; private set; }

        static WindowInfo()
        {
            WindowsDic = new Dictionary<IntPtr, string>();
        }

        public static bool GetWindowHandleInfo()
        {
            if (!WindowsAPI.EnumDesktopWindows(IntPtr.Zero, FilterCallback, IntPtr.Zero))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool FilterCallback(IntPtr hWnd, int lParam)
        {
            StringBuilder sbTitle = new StringBuilder(1024);
            WindowsAPI.GetWindowText(hWnd, sbTitle, sbTitle.Capacity);
            string title = sbTitle.ToString();

            if (WindowsAPI.IsWindowVisible(hWnd) && !string.IsNullOrEmpty(title))
            {
                WindowsDic.Add(hWnd, title);// 재전송에러
            }

            return true;
        }

        public static void FindTargetHandle(string Keyword)
        {
            TargetHandle = WindowsDic.AsEnumerable().Where(keyvalue => keyvalue.Value.Contains(Keyword)).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
