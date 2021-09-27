using Flex.Entities;
using Flex.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Flex.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlDatabase database = new MySqlDatabase("movies");

            Table<MovieRecord> table = database.GetTable<MovieRecord>();

        }
    }
}
