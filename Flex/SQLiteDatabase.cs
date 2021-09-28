using System;
using System.Collections.Generic;
using System.Data.Common;
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
            this.Connection.Open();
        }
        public SQLiteDatabase(string filePath) : this(Assembly.GetEntryAssembly(), filePath)
        {

        }

        public override int NonQuery(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, Connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public override T Scalar<T>(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, Connection))
            {
                return (T)command.ExecuteScalar();
            }
        }

        public override DbDataReader ExecuteReader(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, Connection))
            {
                using (DbDataReader reader = command.ExecuteReader())
                {
                    return reader;
                }
                
            }
        }
    }
}
