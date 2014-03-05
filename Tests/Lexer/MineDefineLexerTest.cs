using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MineDefine.Lexer;

namespace Tests.Lexer
{
    [TestFixture]
    public class MineDefineLexerTest
    {
        [TestCase("@chair:{}", Result = "Identifier,Colon,OpenBrace,CloseBrace")]
        [TestCase("4,5", Result = "Location")]
        [TestCase("4,5,2", Result = "Location")]
        public string ItLexes(string input)
        {
            var pieces = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input)))
                    .Lex()
                    .Select(x => x.Kind)
                ;
            return string.Join(",", pieces);
        }
    }
}
