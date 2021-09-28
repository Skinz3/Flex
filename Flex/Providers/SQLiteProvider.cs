using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace Flex.Providers
{
    public class SQLiteProvider : ISqlProvider
    {
        public char ParameterPrefix => ':';

        private const string ConnectionString = "Data Source={0}";

        private SQLiteConnection Connection
        {
            get;
            set;
        }

        public SQLiteProvider(string filePath)
        {
            this.Connection = new SQLiteConnection(string.Format(ConnectionString, filePath));
        
        }
        public void Connect()
        {
            this.Connection.Open();
        }
        public int NonQuery(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, Connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public T Scalar<T>(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, Connection))
            {
                return (T)command.ExecuteScalar();
            }
        }

        public DbCommand CreateSqlCommand()
        {
            return new SQLiteCommand(Connection);
        }

        public DbParameter CreateSqlParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }

        
    }
}
