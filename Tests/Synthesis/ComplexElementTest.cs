using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using NUnit.Framework;

namespace Tests.Synthesis
{
    [TestFixture]
    public class ComplexElementTest
    {
        private class StubElement : IElement
        {
            public int Width { get; set; }
            public int Depth { get; set; }
            public int Height { get; set; }
            public void Build(IBlockStamp stamp, Transform transform) { throw new NotImplementedException(); }
        }

        [TestCase("1x1x1;0,0,0;1;1;1",Result = "1;1;1")]
        [TestCase("1x1x1;1,1,1;1;1;1",Result = "2;2;2")]
        [TestCase("1x1x1;0,0,0;10;10;10",Result = "10;10;10")]
        [TestCase("1x2x1;0,2,0;10;10;10",Result = "10;22;10")]
        [TestCase("1x1x1;0,0,0;1;1;1|1x1x1;0,1,0;1;1;1",Result = "1;2;1")]
        public string ItDeterminesBoundaries(string input)
        {
            var cmplx = new ComplexElement();
            input.Split('|')
                .Select(x => x.Split(';'))
                .Select(x => new {
                    Dimension = Dimension.Parse(x[0]),
                    Location = Location.Parse(x[1]),
                    Element = new StubElement() {
                        Width = int.Parse(x[2]),
                        Depth = int.Parse(x[3]),
                        Height = int.Parse(x[4]),
                    }
                })
                .ToList()
                .ForEach(x => cmplx.AddBuildInstruction(new BuildInstruction(BuildShape.Box, x.Dimension, x.Location, x.Element)));
            return string.Join(";", new[] {cmplx.Width, cmplx.Depth, cmplx.Height});
        }
    }
}
