using Flex.Attributes;
using Flex.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex.IO
{
    public class TableReader<T> where T : IEntity
    {
        private MySqlDatabase Database
        {
            get;
            set;
        }
        private Type Type
        {
            get;
            set;
        }
        private PropertyInfo[] Properties
        {
            get;
            set;
        }
        public TableReader(MySqlDatabase database)
        {
            this.Database = database;
            this.Type = typeof(T); // no more computation of typeof(T)
            this.Properties = Type.GetProperties().Where(x => x.GetCustomAttribute<TransientAttribute>() == null).ToArray();
        }
        public IEnumerable<T> Query(string query)
        {
            MySqlConnection connection = Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                List<T> results = new List<T>();

                MySqlDataReader reader = command.ExecuteReader(); // ExecuteReader() should be called once?

                if (reader.FieldCount != Properties.Length)
                {
                    throw new InvalidOperationException("Inccorect mapping.");
                }
                while (reader.Read())
                {
                    var obj = new object[this.Properties.Length];

                    for (var i = 0; i < reader.FieldCount; i++) // O(2n) complexity necessary ? only one for loop ?
                    {
                        obj[i] = ConvertProperty(reader[i], Properties[i]);
                    }

                    T entity = (T)Activator.CreateInstance(Type);

                    for (int i = 0; i < Properties.Length; i++)
                    {
                        Properties[i].SetValue(entity, obj[i]);
                    }

                    results.Add(entity);

                }

                reader.Close();

                return results;
            }
        }

        private object ConvertProperty(object value, PropertyInfo property)
        {
            return Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
        }
    }
}
