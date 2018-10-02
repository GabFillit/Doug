using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    interface MessagingService
    {
        MessageSlur GetSlurById(string id);

        void SendMessage(string conversationId);

        void AddVoteReactionsToMessage(string messageId);
    }
}
