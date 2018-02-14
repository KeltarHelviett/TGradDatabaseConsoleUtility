using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    enum ArgumentType
    {
        VARCHAR, VARCHAR2, DECIMAL, NUMBER, STRING, BOOLEAN, DATE, TIMESTAMP, CLOB, BLOB, CURSOR
    }

    enum ArgumentDirection
    {
        Input = 1, Output, InputOutput, ReturnValue
    }

    class Argument
    {

        #region PublicProperties

        public string Name { set; get; }

        public ArgumentType Type { set; get; }

        public ArgumentDirection Direction { set; get; }

        public bool IsDefault { set; get; }

        #endregion

        #region StaticPublicProperties

        public static Dictionary<string, ArgumentType> StringToArgumentType { get; } = new Dictionary<string, ArgumentType>
        {
            { "varchar2", ArgumentType.VARCHAR2 },
            { "decimal", ArgumentType.DECIMAL },
            { "number", ArgumentType.NUMBER },
            { "boolean", ArgumentType.BOOLEAN },
            { "date", ArgumentType.DATE },
            { "timestamp", ArgumentType.TIMESTAMP },
            { "string", ArgumentType.STRING },
            { "clob",  ArgumentType.CLOB },
            { "blob", ArgumentType.BLOB },
            { "sys_refcursor", ArgumentType.CURSOR },
            { "varchar", ArgumentType.VARCHAR }

        };

        #endregion
    }
}
