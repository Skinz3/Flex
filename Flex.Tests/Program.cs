using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Flex.Tests
{
    class Program
    {
        static void Main()
        {
            Database database = new SQLiteDatabase("test.sqlite");
            database.CreateAllTables();

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

            var movies = table.Select();

            MovieRecord movie = new MovieRecord()
            {
                Name = "whassup"
            };

            table.Insert(movie);

        }
    }
}
