using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.StateMachines
{
    public class BotStateMachine
    {
        private enum State
        {
            Idle,
            CoffeeBreakBuilding,
            CoffeeRemind,
            CoffeePostponed,
            CoffeeBreak
        }

        public enum Event
        {
            CoffeeEmoji,
            CoffeeResolve,
            CoffeeCancel,
            CoffeePostpone,
            CoffeeRemindTimeout,
            CoffeeBreakEnd
        }
    }
}
