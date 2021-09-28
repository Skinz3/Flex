using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Flex.IO
{
    public class ProtoSerializer
    {
        public static byte[] Serialize(object @object)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, @object);
                return stream.ToArray();
            }
        }
        public static object Deserialize(Type type, byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return Serializer.Deserialize(type, stream);
            }
        }
    }
}
