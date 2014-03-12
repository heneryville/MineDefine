using System;
using System.IO;
using System.Linq;
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
            Console.WriteLine("Lexing");
            var tokens = new MineDefineLexer(input).Lex();
            Console.WriteLine("\nTokens:");
            Console.WriteLine(string.Join(", ", new MineDefineLexer(input).Lex()));
            Console.WriteLine("\nParsing");
            var ast = new MineDefineParser(tokens).Parse();
            Console.WriteLine("\nAST:");
            Console.WriteLine(ast.ToString());
            Console.WriteLine("\nLowering");
            ast = new StandaloneTransformSugar().Transform(ast);
            Console.WriteLine("Planning");
            var layout = new MineDefineLayout().Layout(ast);
            return new MineDefineExecutable(ast,layout);
        }

    }
}
