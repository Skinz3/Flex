using Flex.Attributes;
using Flex.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Tests
{
    [Table("users")]
    public class UserRecord : IEntity
    {
        [Primary]
        [AutoIncrement]
        public int Id
        {
            get;
            set;
        }
        [Primary]
        public string Name
        {
            get;
            set;
        }
        public DateTime CreationDate
        {
            get;
            set;
        }
    }
}
