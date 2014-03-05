using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Lexer;
using MineDefine.Parser;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using NUnit.Framework;

namespace Tests.Synthesis
{
    [TestFixture]
    public class SynthesizerTest
    {
        [TestCase("@wood;","0,0,0",1)]
        [TestCase("1,1,1 @wood;","1,1,1",1)]
        [TestCase("2x1x1 @wood;","0,0,0|1,0,0",2)]
        [TestCase("@blah: { @wood; } @blah;","0,0,0",1)]
        [TestCase("@wood; up; @wood;","0,0,0|0,0,1",2)]
        [TestCase("@wood; top; @wood;","0,0,0|0,0,1",2)]
        [TestCase("@wood; top @wood;","0,0,0|0,0,1",2)]
        public void ItPlacesBlocksWhereExpected(string code, string assertions, int mass)
        {
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(code))).Lex();
            var ast = new MineDefineParser(tokens).Parse();
            ast = new StandaloneTransformSugar().Transform(ast);

            var fakeStamp = new FakeStamp();
            var synthesizer = new Synthesizer(fakeStamp);
            synthesizer.Place(ast, Transform.Identity);

            foreach (var assertion in assertions.Split('|').Select(Location.Parse))
            {
                fakeStamp.AssertPlaced(assertion);
            }

            Assert.AreEqual(mass,fakeStamp.Mass);
        }
    }
}
