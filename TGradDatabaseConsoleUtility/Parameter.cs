using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TGradDatabaseConsoleUtility
{
    enum ParameterType
    {
        Number, Decimal, Cursor, AnsiString, Clob, DateTime, Blob, String, Date, Timestamp
    }

    enum ParameterDirection
    {
        Input, Output, ReturnValue, InputOutput
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

        public Parameter(XElement param)
        {
            try
            {
                Name = param.Attribute("Name").Value;
                ServerName = param.Attribute("ServerName").Value;
                Type = StringToParameterType[param.Attribute("Type").Value];
                Direction = StringToParameterDirection[param.Attribute("Direction").Value];
            }
            catch (Exception e)
            {
                throw new Exception($"Parameter {Name.ToString()} has invalid attributes");
            }
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

        #region StaticPublicProperties

        public static Dictionary<string, ParameterType> StringToParameterType { get; } = new Dictionary<string, ParameterType>
        {
            { "Number", ParameterType.Number },
            { "Decimal", ParameterType.Decimal },
            { "Cursor", ParameterType.Cursor },
            { "AnsiString", ParameterType.AnsiString },
            { "Clob", ParameterType.Clob },
            { "DateTime", ParameterType.DateTime },
            { "Blob", ParameterType.Blob },
            { "String", ParameterType.String },
            { "Date", ParameterType.Date },
            { "Timestamp", ParameterType.Timestamp }
        };

        public static Dictionary<string, ParameterDirection> StringToParameterDirection { get; } = new Dictionary<string, ParameterDirection>
        {
            { "Input", ParameterDirection.Input },
            { "Output", ParameterDirection.Output },
            { "ReturnValue", ParameterDirection.ReturnValue },
            { "InputOutput", ParameterDirection.InputOutput }
        };

        #endregion
    }
}
