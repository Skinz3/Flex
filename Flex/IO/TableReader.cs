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
        private Table<T> Table
        {
            get;
            set;
        }
        private Type Type
        {
            get;
            set;
        }

        public TableReader(Table<T> table)
        {
            this.Table = table;
            this.Type = typeof(T); // no more computation of typeof(T)
        }

      
        public IEnumerable<T> Query(string query)
        {
            MySqlConnection connection = Table.Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                List<T> results = new List<T>();

                MySqlDataReader reader = command.ExecuteReader(); // ExecuteReader() should be called once?

                if (reader.FieldCount != Table.Properties.Length)
                {
                    throw new InvalidOperationException("Inccorect mapping.");
                }
                while (reader.Read())
                {
                    var obj = new object[Table.Properties.Length];

                    for (var i = 0; i < reader.FieldCount; i++) // O(2n) complexity necessary ? only one for loop ?
                    {
                        obj[i] = ConvertProperty(reader[i], Table.Properties[i]);
                    }

                    T entity = (T)Activator.CreateInstance(Type);

                    for (int i = 0; i < Table.Properties.Length; i++)
                    {
                       Table.Properties[i].SetValue(entity, obj[i]);
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
