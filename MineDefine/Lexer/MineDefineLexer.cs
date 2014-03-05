using System.Collections.Generic;
using System.IO;

namespace MineDefine.Lexer
{
    public class MineDefineLexer
    {
        private Scanner scanner;

        public MineDefineLexer(Stream stream)
        {
            scanner = new Scanner(stream);
        }

        public IEnumerable<LexToken> Lex()
        {
            int raw = 0;
            while((raw = scanner.yylex()) > (int)TokenKind.EOF)
            {
                yield return new LexToken()
                {
                    Kind = (TokenKind)raw,
                    Line = scanner.line,
                    Col = scanner.col,
                    Value = scanner.yytext.ToLower()
                };

            }
        }
    }
}
