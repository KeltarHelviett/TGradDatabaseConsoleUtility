using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    class Subroutine
    {
        public List<Argument> Arguments { get; } = new List<Argument>();

        public  string Name { set; get; }
    }
}
