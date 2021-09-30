using Flex.SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Flex.Providers
{
    public class MySqlProvider : ISqlProvider
    {
        private const string DefaultConnectionString = "Server={0};UserId={1};Password={2};Database={3};SslMode=none;";

        public char ParameterPrefix => '?';

        private MySqlConnection MySqlConnection
        {
            get;
            set;
        }

        public MySqlProvider(MySqlConnection connection)
        {
            this.MySqlConnection = connection;
        }
        public MySqlProvider(string connectionString)
        {
            MySqlConnection = new MySqlConnection(connectionString);
        }
        public MySqlProvider(string databaseName, string host, string user, string password) : this(string.Format(DefaultConnectionString, host, user, password, databaseName))
        {

        }
        /// <summary>
        /// Update MAX_ALLOWED_PACKET size (global)
        /// </summary>
        /// <param name="sizeMb">size in megabytes</param>
        public void SetMaxAllowedPacket(double sizeMb)
        {
            NonQuery(string.Format(SQLQueries.SET_MAX_ALLOWED_PACKET, (sizeMb * 1048576d).ToString()));
        }
        /// <summary>
        /// Return MAX_ALLOWED_PACKET size (global)
        /// </summary>
        public double GetMaxAllowedPacket()
        {
            return Scalar<ulong>(SQLQueries.GET_MAX_ALLOWED_PACKET) / 1048576d;
        }
        public void Connect()
        {
            UseConnection();
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

        public int NonQuery(string query)
        {
            using (var command = new MySqlCommand(query, UseConnection()))
            {
                return command.ExecuteNonQuery();
            }
        }

        public T Scalar<T>(string query)
        {
            using (var command = new MySqlCommand(query, UseConnection()))
            {
                return (T)command.ExecuteScalar();
            }
        }

        public DbCommand CreateSqlCommand()
        {
            return new MySqlCommand(string.Empty, UseConnection());
        }


    }
}
