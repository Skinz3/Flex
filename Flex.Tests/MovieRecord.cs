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
        [Primary]
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

        public string Image
        {
            get;
            set;
        }
        public string StreamLinks
        {
            get;
            set;
        }
        public string Quality
        {
            get;
            set;
        }
        [Transient]
        public bool BDO
        {
            get;
            set;
        }
        public string OriginalLink
        {
            get;
            set;
        }
    }
}
