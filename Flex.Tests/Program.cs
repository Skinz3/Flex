using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.Providers;
using Flex.Schedulers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Flex.Tests
{
    class Program
    {
        static void Main()
        {
            MySqlProvider provider = new MySqlProvider("gla", "localhost", "root", "");

            ISqlProvider provider2 = new SQLiteProvider("test.sqlite");

            Database db = new Database(provider);

            var table = db.GetTable<MovieRecord>();

            var test = table.Select();

            

        }
    }
}
