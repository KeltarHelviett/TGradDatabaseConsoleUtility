using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;

namespace TGradDatabaseConsoleUtility
{
    static class Metabase
    {
        #region PublicProperties

        public static List<Command> Commands { get; } = new List<Command>();

        #endregion

        public static void ReadFromXmlFile(string fileName)
        {             
            var xdoc = XDocument.Load(fileName);
            var commands = xdoc.Descendants("Command");
            Commands.Clear();
            Commands.Capacity = commands.Count();
            var types = new List<string>();
            foreach (var command in commands)
            {
                var name = command.Attribute("Name").ToString();
                var tmp = command.Attribute("Text").ToString();
                var i = tmp.IndexOf('.');
                if (i == -1)
                    continue;
                var package = tmp.Substring(0, i).ToLower();
                var serverName = tmp.Substring(i + 1);
                var parameters = command.Descendants("Parameter");
                var com = new Command(name, package, serverName);
                com.Parameters.Capacity = parameters.Count();
                foreach (var parameter in parameters)
                {
                    var paramName = parameter.Attribute("Name").Value ?? "";
                    var paramServerName = parameter.Attribute("ServerName").Value ?? "";                    
                    var type = parameter.Attribute("Type").Value ?? "";
                    var direction = parameter.Attribute("Direction").Value ?? "";
                    var param = new Parameter(paramName, paramServerName, type, direction);
                    com.Parameters.Add(new Parameter(paramName, paramServerName, type, direction));

                }
                Commands.Add(com);
            }
        }
    }
}
