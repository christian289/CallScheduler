using CallScheduler.Helper;
using CallScheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallScheduler.Global
{
    /// <summary>
    /// 비동기 Thread로 알람 처리
    /// </summary>
    public class TimerTask
    {
        private DataModel _Target { get; set; }
        private DateTime AlarmTime { get; set; }

        public TimerTask(DataModel Target)
        {
            _Target = Target;

            string[] HourMinute = _Target.AlarmTime.Split(':');
            AlarmTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HourMinute[0].ToInt32Ex(), HourMinute[1].ToInt32Ex(), 0);
        }
    }
}
