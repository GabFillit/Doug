using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class CoffeeBreak
    {
        public List<User> Participants { get; set; }

        public TimeSpan RemindTimeout { get; set; }
        public DateTime MorningBreak { get; set; }
        public DateTime AfternoonBreak { get; set; }
        public TimeSpan Tolerance { get; set; }
        public TimeSpan BreakDuration { get; set; }

        public virtual bool IsCoffeeTime(TimeZoneInfo timezone)
        {
            throw new NotImplementedException();
        }

        public virtual void RemindTimeoutStart(Action action)
        {
            throw new NotImplementedException();
        }

        public virtual void CancelRemindTimeout()
        {
            throw new NotImplementedException();
        }

        public virtual void CoffeeBreakStart(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
