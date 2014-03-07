using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MineDefine;
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
            var exe = MineDefineCompiler.Compile(code);
            var fakeStamp = new FakeStamp();
            exe.Place(fakeStamp,Transform.Identity);

            foreach (var assertion in assertions.Split('|').Select(Location.Parse))
            {
                fakeStamp.AssertPlaced(assertion);
            }

            Assert.AreEqual(mass,fakeStamp.Mass);
        }
    }
}
