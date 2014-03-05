using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using NUnit.Framework;

namespace Tests.Parser
{
    [TestFixture]
    public class DimensionTest
    {
        [Test]
        public void ItDeterminesDimensionEquality()
        {
            Assert.AreEqual( new Dimension(1,2,3), new Dimension(1,2,3) );
        }

        [Test]
        public void ItDeterminesDimensionInEquality()
        {
            Assert.AreNotEqual( new Dimension(1,2,3), new Dimension(1,2,4) );
        }
    }
}
