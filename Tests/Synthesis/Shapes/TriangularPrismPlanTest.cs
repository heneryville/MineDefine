using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis.Shapes;
using NUnit.Framework;

namespace Tests.Synthesis.Shapes
{
    [TestFixture]
    public class TriangularPrismPlanTest
    {
        [TestCase("3x2x2", "0,0,0|1,0,0|2,0,0|1,1,0|0,0,1|1,0,1|2,0,1|1,1,1")]
        [TestCase("5x2x1", "0,0,0|1,0,0|2,0,0|3,0,0|4,0,0|2,1,0")]
        [TestCase("3x5x1", "0,0,0|1,0,0|2,0,0|0,1,0|1,1,0|2,1,0|0,2,0|1,2,0|2,2,0|1,3,0|1,4,0")]
        public void ItDefinesPointsInATriangularPrism(string dim, string assertions)
        {
            var box = new TriangularPrismPlan(Dimension.Parse(dim));
            var orderedActual = box.GetLocations();
            var actual = new HashSet<Location>(orderedActual);
            var expected = assertions.Split('|').Select(Location.Parse).ToList();
            foreach (var loc in expected)
            {
                Assert.That(actual.Contains(loc),"Does not have " + loc);
            }
            Assert.AreEqual(expected.Count, actual.Count,
                string.Join("|",orderedActual));
        }
    }
}
