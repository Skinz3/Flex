using Flex.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Entities
{
    public interface ITable
    {
        void Create();

        IScheduler GetScheduler();
    }
}
