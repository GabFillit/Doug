using Doug.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface TimeService
    {
        bool IsCoffeeTime(TimeZoneInfo timezone);

        Task CoffeeRemindTimeout(InstanceStateMachine machine);
        void CancelCoffeeRemindTimeout();
    }
}
