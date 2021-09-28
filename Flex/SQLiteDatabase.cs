using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace Flex
{
    public class SQLiteDatabase : Database
    {
        private SQLiteConnection Connection
        {
            get;
            set;
        }

        public SQLiteDatabase(Assembly entitiesAssembly, string filePath) : base(entitiesAssembly)
        {
            this.Connection = new SQLiteConnection("Data Source=" + filePath);
        }
        public SQLiteDatabase(string filePath) : this(Assembly.GetEntryAssembly(), filePath)
        {

        }
    }
}
