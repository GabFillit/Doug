using Doug.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Repositories
{
    interface InstanceRepository
    {
        void AddInstance(Instance instance);

        List<Instance> GetInstances();
    }
}
