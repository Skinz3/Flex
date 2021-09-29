using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.Providers;
using Flex.Schedulers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Flex.Tests
{
    class Program
    {
        static void Main()
        {
            //  Database database = new Database(new SQLiteProvider("file.sqlite"));

            Database database = new Database(new MySqlProvider("gla"));

            Table<UserRecord> table = database.GetTable<UserRecord>();

            table.DeleteAll();

            table.Create();

            var values = table.Select();

            table.Scheduler.InsertLater(new UserRecord() { Id = 1, Name = "Benoit" });
            table.Scheduler.InsertLater(new UserRecord() { Id = 2, Name = "Jean" });
            table.Scheduler.InsertLater(new UserRecord() { Id = 3, Name = "Kevin" });

            table.Scheduler.Apply();
        }
    }
}
