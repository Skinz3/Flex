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

            Database db = new Database(provider2);

            var table = db.GetTable<MovieRecord>();

            var test = table.Select();

            table.DeleteAll();

            List<MovieRecord> movies = new List<MovieRecord>();
            for (int i = 0; i < 100000; i++)
            {
                MovieRecord m = new MovieRecord();
                m.Id = i;
                m.Name = "test" + i;
                m.Image = "test";
                m.OriginalLink = "whatever";
                m.Quality = "ye";
                m.StreamLinks = "oerkezo";
                movies.Add(m);

            }


            /*  table.Insert(movies);
              table.DeleteAll();
              table.Insert(movies);
              table.DeleteAll(); */


            var a = System.Diagnostics.Stopwatch.StartNew();

            table.Insert(movies);

            Console.WriteLine("Insert() Ended in " + a.ElapsedMilliseconds + "ms");



            Console.ReadLine();

        }
    }
}
