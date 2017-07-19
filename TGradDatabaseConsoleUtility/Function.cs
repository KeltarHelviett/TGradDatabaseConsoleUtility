using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    class Function: Subroutine
    {
        private Argument returnValue;

        public Argument ReturnValue
        {
            get { return returnValue; }
            set
            {
                if (value.Direction != ArgumentDirection.ReturnValue)
                    throw new Exception("Return value must have ArgumentDirection equal to ReturnValue");
                returnValue = value;
            }
        }
    }
}
