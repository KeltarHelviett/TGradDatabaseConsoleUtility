using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    class Subroutine
    {
        public Dictionary<string, Argument> Arguments { get; } = new Dictionary<string, Argument>();

        public  string Name { set; get; }
    }
}
