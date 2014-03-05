using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using Moq;
using NUnit.Framework;

namespace Tests.Synthesis
{
    [TestFixture]
    public class TransformTest
    {
        [TestCase(AbsoluteOriginTransform.Up, 2, Result = "10,10,12")]
        [TestCase(AbsoluteOriginTransform.Down, 2, Result = "10,10,8")]
        [TestCase(AbsoluteOriginTransform.East, 2, Result = "12,10,10")]
        [TestCase(AbsoluteOriginTransform.West, 2, Result = "8,10,10")]
        [TestCase(AbsoluteOriginTransform.North, 2, Result = "10,8,10")]
        [TestCase(AbsoluteOriginTransform.South, 2, Result = "10,12,10")]
        public string ItAdjustsFromAbsoluteTransforms(AbsoluteOriginTransform dir, int degree)
        {
            var trans = new Transform(10,10,10).Adjust(dir, degree);
            return string.Join(",", new[] {trans.X, trans.Z, trans.Y});
        }

        [TestCase(RelativeOriginTransform.Top,Result = "10,10,12")]
        [TestCase(RelativeOriginTransform.Bottom,Result = "10,10,0")]
        [TestCase(RelativeOriginTransform.Right, Result = "12,10,10")]
        [TestCase(RelativeOriginTransform.Left, Result = "0,10,10")]
        [TestCase(RelativeOriginTransform.Back, Result = "10,0,10")]
        [TestCase(RelativeOriginTransform.Front, Result = "10,12,10")]
        public string ItAdjustsFromRelativeTransforms(RelativeOriginTransform dir)
        {
            var element = new Mock<IElement>();
            element.SetupGet(x => x.Width).Returns(12);
            element.SetupGet(x => x.Depth).Returns(12);
            element.SetupGet(x => x.Height).Returns(12);
                var trans = new Transform(10,10,10)
                .Adjust(dir, element.Object);
            return string.Join(",", new[] {trans.X, trans.Z, trans.Y});
        }
    }
}
