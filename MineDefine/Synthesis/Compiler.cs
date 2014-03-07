using System.IO;
using System.Text;
using MineDefine.Lexer;
using MineDefine.Parser;

namespace MineDefine.Synthesis
{
    public class MineDefineCompiler
    {

        private MineDefineCompiler() { }

        public static MineDefineExecutable Compile(string code)
        {
            return Compile(new MemoryStream(Encoding.UTF8.GetBytes(code)));
        }

        public static MineDefineExecutable Compile(Stream input)
        {
            var tokens = new MineDefineLexer(input).Lex();
            var ast = new MineDefineParser(tokens).Parse();
            ast = new StandaloneTransformSugar().Transform(ast);
            var layout = new MineDefineLayout().Layout(ast);
            return new MineDefineExecutable(ast,layout);
        }

    }
}
