using Flex.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Schedulers
{
    public class PeriodicSynchronizer
    {
        private Task RefreshTask
        {
            get;
            set;
        }
        private float DelaySeconds
        {
            get;
            set;
        }

        private Database Database
        {
            get;
            set;
        }

        public PeriodicSynchronizer(Database database, float delaySeconds)
        {
            this.DelaySeconds = delaySeconds;
            this.DelaySeconds = delaySeconds;
            Database = database;
        }

        public void Start()
        {
            if (RefreshTask != null)
            {
                throw new InvalidOperationException("Task is already running.");
            }

            CreateTask();
        }

        private void CreateTask()
        {
            RefreshTask = Task.Factory.StartNewDelayed((int)(DelaySeconds * 1000), Synchronize);
        }

        private void Synchronize()
        {
            foreach (var table in Database.GetTables())
            {
                table.GetScheduler().Apply();
            }
        }
    }
}
