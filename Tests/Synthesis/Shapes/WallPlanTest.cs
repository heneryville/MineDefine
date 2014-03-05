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
    public class WallPlanTest
    {
        [TestCase("3x3x2", "0,0,0|1,0,0|2,0,0|0,1,0|2,1,0|0,2,0|1,2,0|2,2,0|0,0,1|1,0,1|2,0,1|0,1,1|2,1,1|0,2,1|1,2,1|2,2,1")]
        public void ItDefinesPointsInAWall(string dim, string assertions)
        {
            var box = new WallPlan(Dimension.Parse(dim));
            var points = new HashSet<Location>(box.GetLocations());
            var expected = assertions.Split('|').Select(Location.Parse).ToList();
            foreach (var loc in expected)
            {
                Assert.That(points.Contains(loc), "Does not contain " + loc);
            }
            Assert.AreEqual(expected.Count, points.Count);
        }
    }
}
