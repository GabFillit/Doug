using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    public interface MessagingService
    {
        Message GetMessageById(string id);

        void SendMessage(Message message);

        void AddVoteReactionsToMessage(string messageId);
    }
}
