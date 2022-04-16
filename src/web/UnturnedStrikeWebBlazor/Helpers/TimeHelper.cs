using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeWebBlazor.Helpers
{
    public class TimeHelper
    {
        public static string GetVIPTimeLeft(DateTime vipExpireDate)
        {
            var timeLeft =  vipExpireDate - DateTime.UtcNow;

            if (timeLeft.TotalDays > 0)
            {
                return $"Your VIP expires in {timeLeft.Days} days";
            } else
            {
                var local = DateTime.Now.Add(timeLeft);
                return $"Your VIP expires {(local.Day == DateTime.Now.Day ? "today" : "tomorrow")} at {DateTime.Now.Add(timeLeft):H:mm}";
            }
        }
    }
}
