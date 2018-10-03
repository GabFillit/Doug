using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Services
{
    interface FlameService
    {
        void FlameUser(User user);

        void CleanUpBadSlurs();

        void AddSlur(Slur slur);
    }
}
