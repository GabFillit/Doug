using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface MessagingService
    {
        MessageSlur GetSlurById(string id);

        void SendMessage(Message message);

        void AddVoteReactionsToMessage(string messageId);
    }
}
