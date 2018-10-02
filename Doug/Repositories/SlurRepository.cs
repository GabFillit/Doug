using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    interface SlurRepository
    {
        void AddSlur(Slur slur);

        Slur GetSlur(int id);

        Slur GetRandomSlur();

        void RemoveSlur(int id);
    }
}
