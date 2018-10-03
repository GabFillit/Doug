using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Doug.StateMachines.BotStateMachine;

namespace Doug.StateMachines
{
    interface InstanceStateMachine
    {
        void CoffeeEmoji(User participant);

        State GetCurrentState();
    }
}
