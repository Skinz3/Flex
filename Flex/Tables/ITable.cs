using Flex.Schedulers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Flex.Entities
{
    public interface ITable
    {
        string Name
        {
            get;
        }

        PropertyInfo PrimaryProperty
        {
            get;
        }

        IScheduler GetScheduler();

        void Create();

    }
}
