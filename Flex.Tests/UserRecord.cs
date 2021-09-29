using Flex.Attributes;
using Flex.Entities;
using Flex.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Tests
{
    [Table("users")]
    public class UserRecord : IEntity
    {
        [Primary]
        public int Id
        {
            get;
            set;
        }
        [Update]
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
