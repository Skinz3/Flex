using Flex.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Entities
{
    public interface ITable
    {
        string Name
        {
            get;
        }
        IScheduler GetScheduler();

        void Create();
    }
}
