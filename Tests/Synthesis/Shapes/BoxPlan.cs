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
    public class BoxPlanTest
    {
        [TestCase("2x2x2", "0,0,0|0,0,1|0,1,0|0,1,1|1,0,0|1,0,1|1,1,0|1,1,1")]
        public void ItDefinesPointsInABox(string dim, string assertions)
        {
            var box = new BoxPlan(Dimension.Parse(dim));
            var points = new HashSet<Location>(box.GetLocations());
            foreach (var loc in assertions.Split('|').Select(Location.Parse))
            {
                Assert.That(points.Contains(loc));
            }
        }
    }
}
