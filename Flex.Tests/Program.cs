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
            MySqlDatabase database = new MySqlDatabase("gla");

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

            table.DeleteAll();

        }
    }
}
