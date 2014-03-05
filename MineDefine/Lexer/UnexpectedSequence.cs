using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Lexer
{
    public class UnexpectedSequenceException : Exception
    {
        public UnexpectedSequenceException(string sequence, int line, int col)
            : base(string.Format("Unexpectec character '{0}' on line {1} column {2}", sequence, line, col)) {}
    }
}
