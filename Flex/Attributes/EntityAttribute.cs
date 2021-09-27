using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Attributes
{
    public class EntityAttribute : Attribute
    {
        public string TableName
        {
            get;
            private set;
        }
        public EntityAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
