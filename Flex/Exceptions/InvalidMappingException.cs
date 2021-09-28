using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Exceptions
{
    public class InvalidMappingException : Exception
    {
        public InvalidMappingException(string message) : base(message)
        {

        }
    }
}
