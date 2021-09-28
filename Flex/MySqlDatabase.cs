using Flex.Attributes;
using Flex.Entities;
using Flex.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex
{
    public class MySqlDatabase : Database
    {
        private MySqlConnection MySqlConnection
        {
            get;
            set;
        }

        public override char ParameterPrefix => '?';

        public MySqlDatabase(Assembly entitiesAssembly, string databaseName, string host = "127.0.0.1", string user = "root", string password = "") : base(entitiesAssembly)
        {
            string connectionString = string.Format("Server={0};UserId={1};Password={2};Database={3};SslMode=none;", host, user, password, databaseName);
            MySqlConnection = new MySqlConnection(connectionString);
            UseConnection();
        }
        public MySqlDatabase(string databaseName, string host = "127.0.0.1", string user = "root", string password = "") : this(Assembly.GetEntryAssembly(), databaseName, host, user, password)
        {

        }

        internal MySqlConnection UseConnection()
        {
            if (!MySqlConnection.Ping())
            {
                MySqlConnection.Close();
                MySqlConnection.Open();
            }

            return MySqlConnection;
        }

        public override int NonQuery(string query)
        {
            using (var command = new MySqlCommand(query, UseConnection()))
            {
                return command.ExecuteNonQuery();
            }
        }

        public override T Scalar<T>(string query)
        {
            using (var command = new MySqlCommand(query, UseConnection()))
            {
                return (T)command.ExecuteScalar();
            }
        }

        public override DbCommand CreateSqlCommand()
        {
            return new MySqlCommand(string.Empty, UseConnection());
        }

        public override DbParameter CreateSqlParameter(string name, object value)
        {
            return new MySqlParameter(name, value);
        }
    }
}
