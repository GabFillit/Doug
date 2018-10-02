using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    interface ChannelService
    {
        Channel GetChannelById(string id);
    }
}
