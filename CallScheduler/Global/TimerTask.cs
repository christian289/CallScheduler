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
        private DateTime AlarmTime { get; set; }

        public TimerTask(DataModel Target)
        {
            AlarmTime = Target.AlarmTime;
        }

        public bool AlarmStart()
        {
            while (true)
            {
                if (DateTime.Compare(DateTime.Now, AlarmTime) == 0)
                {
                    return true;
                }
            }
        }
    }
}
