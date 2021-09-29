using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Attributes
{
    public class PrimaryAttribute : Attribute
    {
        public PrimaryAttribute()
        {

        }
    }
    public enum GenerationType // TODO
    {
        AutoIncrement,
        None
    }
}
