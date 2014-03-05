using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Synthesis
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message) { }
    }
}
