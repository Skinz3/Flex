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
using System.Reflection;
using System.Threading;
using static Flex.Optimizations.LambdaActivator;

namespace Flex.Tests
{
    public class Test
    {
        public string Arg
        {
            get;
            set;
        }
        public Test(string arg)
        {
            this.Arg = arg;
        }
    }
    class Program
    {
        static void Main()
        {
            MySqlProvider provider = new MySqlProvider("gla", "localhost", "root", "");

            //   ISqlProvider provider2 = new SQLiteProvider("test.sqlite");

            Console.WriteLine("Started");

            Database db = new Database(provider);

            var mvies = db.GetTable<MovieRecord>().Select();

            int a = 2;
            /*var table = db.GetTable<MovieRecord>();


            table.DeleteAll();


            for (int i = 0; i < 10; i++)
            {
                MovieRecord movie = new MovieRecord()
                {
                    Id = i,
                    Image = "erjizejr",
                    Name = "zerez" + i,
                    OriginalLink = "l;ef",
                    Quality = "dk,fz",
                    StreamLinks = null
                };

                table.Insert(movie);
            }


            Console.WriteLine("Inserted.");
            Console.ReadLine();

            var test = table.Select();

            var bb = test.Take(2);

            table.Delete(bb); */

        }
    }
}
