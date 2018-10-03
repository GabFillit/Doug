using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Doug.StateMachines.BotStateMachine;

namespace Doug.StateMachines
{
    public interface InstanceStateMachine
    {
        void CoffeeEmoji(User participant);
        void Skip(User user);
        void Resolve();
    }
}
