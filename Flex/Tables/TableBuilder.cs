using Flex.Attributes;
using Flex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex.Tables
{
    public class TableBuilder
    {
        public static ITable Build(MySqlDatabase database, Type entityType)
        {
            EntityAttribute entityAttribute = entityType.GetCustomAttribute<EntityAttribute>();

            PropertyInfo[] properties = entityType.GetProperties().Where(x => x.GetCustomAttribute<TransientAttribute>() == null).ToArray();

            Type genericType = typeof(Table<>).MakeGenericType(new Type[] { entityType });
            ITable table = (ITable)Activator.CreateInstance(genericType, new object[] { database, entityAttribute.TableName, properties }); ;

            return table;
        }
    }
}
