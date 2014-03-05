using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Lexer;

namespace MineDefine.Parser
{
    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message) { }
    }

    public class UnexpectedEndOfStream : Exception
    {
        public UnexpectedEndOfStream() : base("Unexpected end of stream") { }
    }

    public class UnexpectedToken : Exception
    {
        public UnexpectedToken(LexToken found, TokenKind expected) 
            : base(string.Format("Expected {0}, but got {1} on line {2} col {3}",expected, found.Kind, found.Line, found.Col)) { }

        public UnexpectedToken(LexToken found, string expected) 
            : base(string.Format("Expected {0}, but got {1} on line {2} col {3}",expected, found.Kind, found.Line, found.Col)) { }
    }
}
