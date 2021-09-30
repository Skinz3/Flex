using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Attributes
{
    public class PrimaryAttribute : Attribute
    {
        public GenerationType GenerationType
        {
            get;
            private set;
        }
        public PrimaryAttribute(GenerationType type)
        {
            this.GenerationType = type;
        }
    }
    public enum GenerationType // TODO
    {
        AutoIncrement,
        None
    }
}
