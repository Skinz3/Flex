using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Flex.Tests
{
    class Program
    {
        static void Main()
        {
            Database database = new Database(new MySqlProvider("test")); // new SQLiteProvider("file.sqlite")

            database.CreateAllTables();

            Table<UserRecord> table = database.GetTable<UserRecord>();

            var users = table.Select();


            UserRecord user = new UserRecord()
            {
                Name = "whassup'",
                CreationDate = DateTime.Now
            };


            table.Insert(user);

        }
    }
}
