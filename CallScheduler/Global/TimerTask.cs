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
        }

        public async Task<bool> AlarmStart()
        {
            Func<bool> func = new Func<bool>(() =>
            {
                while (true)
                {
                    if (DateTime.Now == AlarmTime)
                    {
                        return true;
                    }
                }
            });

            return await Task.Run(func);
        }
    }
}
