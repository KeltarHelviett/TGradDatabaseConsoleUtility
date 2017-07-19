using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TGradDatabaseConsoleUtility
{
    class Command
    {
        #region Ctor

        public Command(string name, string package, string serverName)
        {
            Name = name;
            Package = Package;
            ServerName = serverName;
        }

        public Command(XElement command)
        {
            try
            {
                Name = command.Attribute("Name").Value;
                var text = command.Attribute("Text").Value;
                var i = text.IndexOf('.');
                Package = text.Substring(0, i);
                ServerName = text.Substring(i + 1);
            }
            catch (Exception e)
            {
                throw new Exception($"Command has invalid attributes.");
            }
        }

        #endregion

        #region PublicProperties

        public string Name { set; get; }

        public string Package { set; get; }

        public string ServerName { set; get; }

        public List<Parameter> Parameters { set; get; } = new List<Parameter>();

        #endregion
    }
}
