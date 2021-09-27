using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Attributes
{
    public class TableAttribute : Attribute
    {
        public string TableName
        {
            get;
            private set;
        }
        public TableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
