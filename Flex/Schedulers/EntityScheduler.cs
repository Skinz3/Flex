using Flex.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Schedulers
{
    public class EntityScheduler<T> : IScheduler where T : IEntity
    {
        private Table<T> Table
        {
            get;
            set;
        }
        private ConcurrentBag<T> NewElements
        {
            get;
            set;
        }
        private ConcurrentBag<T> DirtyElements
        {
            get;
            set;
        }
        private ConcurrentBag<T> RemovedElements
        {
            get;
            set;
        }
        public EntityScheduler(Table<T> table)
        {
            this.Table = table;
            this.NewElements = new ConcurrentBag<T>();
            this.DirtyElements = new ConcurrentBag<T>();
            this.RemovedElements = new ConcurrentBag<T>();
        }

        public void InsertLater(T entity)
        {
            NewElements.Add(entity);
        }
        public void UpdateLater(T entity)
        {
            if (!NewElements.Contains(entity))
            {
                DirtyElements.Add(entity);
            }
        }
        public void DeleteLater(T entity)
        {
            if (NewElements.Contains(entity))
            {
                NewElements.Add(entity);
            }
            RemovedElements.Add(entity);
        }

        public void Apply()
        {
            Table.Insert(NewElements);

            Table.Update(DirtyElements);

            foreach (var element in RemovedElements) // todo 
            {
                Table.Delete(element);
            }

            NewElements.Clear();
            DirtyElements.Clear();
            RemovedElements.Clear();
        }
    }
}
