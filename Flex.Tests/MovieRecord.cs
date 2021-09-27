using Flex.Attributes;
using Flex.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Tests
{
    [Entity("movies")]
    public class MovieRecord : IEntity
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
}
