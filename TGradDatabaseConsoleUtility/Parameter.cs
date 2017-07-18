using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    enum ParameterType
    {
        Number, Decimal, Cursor, AnsiString, Clob, DateTime, Blob, String, Date, Timestamp
    }

    enum ParameterDirection
    {
        Input, Output
    }

    class Parameter
    {

        #region Ctor

        public Parameter(string name, string serverName, string type, string direction)
        {
            Name = name;
            ServerName = serverName;
            Type = StringToParameterType[type];
            Direction = StringToParameterDirection[direction];
        }

        #endregion

        #region PublicProperties

        public string Name { set; get; }

        //public string ACL { set; get; }

        public string ServerName { set; get; }
        
        public ParameterType Type { set; get; }

        public ParameterDirection Direction { set; get; }

        //public string Value { set; get; }

        #endregion

        #region StaticProperties

        public static Dictionary<string, ParameterType> StringToParameterType { get; } = new Dictionary<string, ParameterType>
        {
            {"Number", ParameterType.Number},
            {"Decimal", ParameterType.Decimal}
        };

        public static Dictionary<string, ParameterDirection> StringToParameterDirection { get; } = new Dictionary<string, ParameterDirection>
        {
            {"Input", ParameterDirection.Input},
            {"Output", ParameterDirection.Output}
        };

        #endregion
    }
}
