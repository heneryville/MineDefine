using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using NUnit.Framework;

namespace Tests.Parser.AST
{
    [TestFixture]
    public class DimensionTest
    {
        [TestCase("1x2x3", Result = "1,2,3")]
        [TestCase("1x2", Result = "1,2,1")]
        [TestCase("1", Result = "1,1,1")]
        public string ItParsesDimensions(string input)
        {
            var dim = Dimension.Parse(input);
            return string.Join(",", new[]
            {
                dim.Width,
                dim.Depth,
                dim.Height
            });
        }
    }
}
