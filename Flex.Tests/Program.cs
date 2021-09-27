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
        static void Main(string[] args)
        {
            MySqlDatabase database = new MySqlDatabase("movies");

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

            var movies = table.Select().GroupBy(x => x.Quality);

        }
    }
}
