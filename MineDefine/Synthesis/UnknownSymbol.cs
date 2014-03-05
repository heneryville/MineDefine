using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Synthesis
{
    public class UnknownSymbol : RuntimeException
    {
        public UnknownSymbol(string symbol) 
            : base(string.Format("Unknown symbol {0}",symbol)) { }
    }
}
