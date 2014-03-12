using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Lexer
{
    public class LexToken
    {
        public TokenKind Kind;
        public string Value;
        public int Line;
        public int Col;

        public override string ToString()
        {
            if (Kind == TokenKind.Identifier
                || Kind == TokenKind.Location
                || Kind == TokenKind.Dimension
                || Kind == TokenKind.Operation
                || Kind == TokenKind.Numeral
                ) return Kind + " (" + Value + ")";
            return Kind.ToString();
        }
    }
}
