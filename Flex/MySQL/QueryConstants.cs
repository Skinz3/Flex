using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.MySQL
{
    class QueryConstants
    {
        public const string CreateTable = "CREATE TABLE if not exists {0} ({1})";

        public const string DROP_TABLE = "DROP TABLE IF EXISTS {0}";

        public const string DELETE_TABLE = "DELETE FROM {0}";

        public const string SELECT = "SELECT * FROM `{0}`";

        public const string COUNT = "SELECT COUNT(*) FROM `{0}`";

        public const string INSERT = "INSERT INTO `{0}` VALUES {1}";

        public const string UPDATE = "UPDATE `{0}` SET {1} WHERE {2} = {3}";

        public const string REMOVE = "DELETE FROM `{0}` WHERE `{1}` = {2}";
    }
}
