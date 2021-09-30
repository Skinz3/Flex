using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Flex.Providers
{
    /*
     * Provide a SQL driver abstraction
     */
    public interface ISqlProvider
    {
        char ParameterPrefix
        {
            get;
        }


        void Connect();

        int NonQuery(string query);

        T Scalar<T>(string query);

        DbCommand CreateSqlCommand();

    }
}
