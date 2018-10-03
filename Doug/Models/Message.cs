using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string Text { get; set; }
    }
}
