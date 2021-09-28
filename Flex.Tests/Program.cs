using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
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
            Database database = new MySqlDatabase("gla");// new SQLiteDatabase("file.sqlite");

            database.CreateAllTables();

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

            var movies = table.Select();

            MovieRecord movie = new MovieRecord()
            {
                Name = "whassup",
            };



            table.Insert(movie);

        }
    }
}
