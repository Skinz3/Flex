using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.SQL
{
    /// <summary>
    /// Should disappear once the query builder is finished
    /// </summary>
    class SQLQueries
    {
        public const string CREATE_TABLE = "CREATE TABLE if not exists {0} ({1})";

        public const string COUNT = "SELECT COUNT(*) FROM `{0}`";

        public const string SELECT = "SELECT * FROM `{0}`";

        public const string WHERE_CLAUSE = " where {0}";

        public const string DROP = "DROP TABLE IF EXISTS {0}";

        public const string DELETE = "DELETE FROM {0}";

        public const string DELETE_IN = "DELETE FROM {0} WHERE id IN({1})";

        public const string DELETE_WHERE = "DELETE FROM {0} WHERE {1}";

        public const string INSERT = "INSERT INTO `{0}` ({1}) VALUES {2}";

        public const string UPDATE = "UPDATE `{0}` SET {1} WHERE {2} = {3}";

        public const string MAX = "SELECT MAX(`{1}`) FROM `{0}`";

        public const string PRIMARY_KEY = "PRIMARY KEY(`{0}`)";

        public const string AUTO_INCREMENT = "AUTO_INCREMENT";

        public const string NOT_NULL = "NOT NULL";

        public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public const string SET_MAX_ALLOWED_PACKET = "SET GLOBAL max_allowed_packet = {0}";

        public const string GET_MAX_ALLOWED_PACKET = "SELECT @@global.max_allowed_packet";
    }
}
