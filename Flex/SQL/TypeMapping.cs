using Flex.Attributes;
using Flex.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Flex.SQL
{
    public class TypeMapping
    {
        private static Dictionary<Type, string> Mapping = new Dictionary<Type, string>()
        {
            { typeof(string),   SQLTypes.VARCHAR(255) },
            { typeof(int),      SQLTypes.INT},
            { typeof(long),     SQLTypes.BIGINT },
            { typeof(short),    SQLTypes.SMALLINT },
            { typeof(byte),     SQLTypes.TINYINT },
            { typeof(DateTime), SQLTypes.DATETIME },

        };
        public static string GetSQLType(PropertyInfo property)
        {
            if (property.HasAttribute<BlobAttribute>() || property.PropertyType.IsCollection())
            {
                return "BLOB";
            }

            if (Mapping.ContainsKey(property.PropertyType))
            {
                return Mapping[property.PropertyType];
            }
            else
            {
                throw new NotImplementedException("Type : " + property.PropertyType.Name + " is not handled.");
            }
        }
    }
}
