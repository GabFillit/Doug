using Doug.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public abstract class Instance
    {
        public int Id { get; set; }
        public string ChannelId { get; set; }
        public BotStateMachine StateMachine { get; set; }
        public List<User> Participants { get; set; }
    }
}
