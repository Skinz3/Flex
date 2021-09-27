using Flex.Entities;
using Flex.Expressions;
using Flex.Extensions;
using System;
using System.Linq.Expressions;

namespace Flex.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlDatabase database = new MySqlDatabase("movies");

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

            var results = table.Select();



        }
    }
}
