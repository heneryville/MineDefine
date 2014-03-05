using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser;
using MineDefine.Parser.AST;
using NUnit.Framework;

namespace Tests.Parser
{
    [TestFixture]
    public class StandaloneTransformSugarTest
    {
        [Test]
        public void ItConvertsNoBlockTransformsToRestOfParent()
        {
            var ast = new MineDefineAST(new IStatement[] {
                new BuildByIdentifier(BuildShape.Box, new Dimension(), new Location(), "wood"), 
                new OriginTransformStatement(new AbsoluteTransformInstructions(AbsoluteOriginTransform.Down, 1), null),
                new BuildByIdentifier(BuildShape.Box, new Dimension(), new Location(), "stone"), 
            });

            var sugar = new StandaloneTransformSugar();
            var done = sugar.Transform(ast);

            Assert.AreEqual(2, done.Statements.Count);
            Assert.IsInstanceOf<BuildByIdentifier>(done.Statements[0]);
            Assert.AreEqual("wood", ((BuildByIdentifier)done.Statements[0]).Identifier);

            var trans = (OriginTransformStatement) done.Statements[1];

            Assert.AreEqual(1, trans.Statements.Count);
            Assert.IsInstanceOf<BuildByIdentifier>(trans.Statements[0]);
            Assert.AreEqual("stone", ((BuildByIdentifier)trans.Statements[0]).Identifier);

        }
    }
}
