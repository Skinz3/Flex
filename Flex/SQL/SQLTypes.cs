using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.SQL
{
    public class SQLTypes
    {
        public static string INT = "INT";

        public static string DATETIME = "DATETIME";

        public static string SMALLINT = "SMALLINT";

        public static string BIGINT = "BIGINT";

        public static string TINYINT = "TINYINT";

        public static string VARCHAR(int size)
        {
            return string.Format("VARCHAR({0})", size);
        }
    }
}
