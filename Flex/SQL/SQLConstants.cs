using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.SQL
{
    class SQLConstants
    {
        public const string CreateTable = "CREATE TABLE if not exists {0} ({1})";

        public const string Count = "SELECT COUNT(*) FROM `{0}`";

        public const string Select = "SELECT * FROM `{0}`";

        public const string SelectWhere = "SELECT * from `{0}` where `{1}`";

        public const string Drop = "DROP TABLE IF EXISTS {0}";

        public const string Delete = "DELETE FROM {0}";

        public const string Insert = "INSERT INTO `{0}` VALUES {1}";

        public const string UPDATE = "UPDATE `{0}` SET {1} WHERE {2} = {3}";

        public const string REMOVE = "DELETE FROM `{0}` WHERE `{1}` = {2}";
    }
}
