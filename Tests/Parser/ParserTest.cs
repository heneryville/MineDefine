using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Lexer;
using MineDefine.Parser;
using MineDefine.Parser.AST;
using NUnit.Framework;
using Tests.Parser.AST;

namespace Tests.Parser
{
    [TestFixture]
    public class ParserTest
    {
        public TestCaseData[] itParsesBuildStatements = new []{
            new TestCaseData("box 10x9x3 4,5,1 @wood;",
                BuildShape.Box,new Dimension(10,9,3),
                new Location(4,5,1), "wood")
                .SetName("Full build statement"),
            new TestCaseData("box 4,5,1 @wood;",
                BuildShape.Box,new Dimension(1,1,1),
                new Location(4,5,1), "wood")
                .SetName("No dimension build statement"),
            new TestCaseData("4,5,1 @wood;",
                BuildShape.Box,new Dimension(1,1,1),
                new Location(4,5,1), "wood")
                .SetName("No shape build statement"),
            new TestCaseData("@wood;",
                BuildShape.Box,new Dimension(1,1,1),
                new Location(0,0,0), "wood")
                .SetName("No location build statement"),
            new TestCaseData("triangular-prisim @wood;",
                BuildShape.TriangularPrisim,new Dimension(1,1,1),
                new Location(0,0,0), "wood")
                .SetName("Alternative shapes"),
        };

        [TestCaseSource("itParsesBuildStatements")]
        public void ItParsesBuildStatements(string input,BuildShape shape , Dimension dim, Location loc, string element)
        {
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var build = (BuildByIdentifier)new MineDefineParser(tokens).Parse().Statements.Single();
            Assert.AreEqual(shape, build.Shape);
            Assert.AreEqual(dim, build.Dimension);
            Assert.AreEqual(loc, build.Location);
            Assert.AreEqual(element, build.Identifier);
        }

        [Test]
        public void ItParsesAbsoluteScopedTransforms()
        {
            var input = @"up 10 { @wood; }";
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var trans = (OriginTransformStatement) new MineDefineParser(tokens).Parse().Statements.Single();
            Assert.IsInstanceOf<AbsoluteTransformInstructions>(trans.Instructions);
            Assert.AreEqual(10, ((AbsoluteTransformInstructions)trans.Instructions).Degree);

            var build = (BuildByIdentifier) trans.Statements.Single();
            Assert.AreEqual("wood", build.Identifier);
        }

        [Test]
        public void ItParsesAbsoluteSyntacticSurgarTransforms()
        {
            var input = @"up 10;";
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var trans = (OriginTransformStatement) new MineDefineParser(tokens).Parse().Statements.Single();
            Assert.IsInstanceOf<AbsoluteTransformInstructions>(trans.Instructions);
            Assert.AreEqual(10, ((AbsoluteTransformInstructions)trans.Instructions).Degree);
            Assert.IsNull(trans.Statements);
        }

        [Test]
        public void ItParsesBlockDefinitions()
        {
            var input = @"@a: {}";
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var def = (ElementDefinition) new MineDefineParser(tokens).Parse().Statements.Single();
            Assert.AreEqual("a", def.Name);
            Assert.AreEqual(0, def.Statements.Count);
        }

        [Test]
        public void ItParsesSingleLineDefinitions()
        {
            var input = @"@a: @wood;";
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var def = (ElementDefinition) new MineDefineParser(tokens).Parse().Statements.Single();
            Assert.AreEqual("a", def.Name);
            Assert.AreEqual(1, def.Statements.Count);
            var build = (BuildByIdentifier)def.Statements.Single();
            Assert.AreEqual("wood",build.Identifier);
        }

        [Test]
        public void ItParsesAFullProgram()
        {
            var input = @"@building: {

  @chair: {
    2x1 @wood;
    1,1 @wood;
  }

  @floor: {
    10x10x1 @stone;
    up 1;
    4,5 @chair;
    10x10x4 @wood;
  }
  @roof: triangular-prisim 10x10x5 @wood;

  1x1x10 @floor;
  top @roof;
  box 1x1x3 @wood;
}

@building;";
            var tokens = new MineDefineLexer(new MemoryStream(Encoding.UTF8.GetBytes(input))).Lex();
            var statements = new MineDefineParser(tokens).Parse().Statements;
            Assert.AreEqual(2,statements.Count);
            var buildingDef = (ElementDefinition)statements.First();

            Assert.AreEqual("building",buildingDef.Name);
            Assert.AreEqual(6, buildingDef.Statements.Count);

            var chairDef = (ElementDefinition)buildingDef.Statements.First();
            Assert.AreEqual("chair",chairDef.Name);
            Assert.AreEqual(2, chairDef.Statements.Count);

            var chairBase = (BuildByIdentifier) chairDef.Statements.First();
            Assert.AreEqual(BuildShape.Box, chairBase.Shape);
            Assert.AreEqual(new Dimension(2,1,1), chairBase.Dimension);
            Assert.AreEqual("wood", chairBase.Identifier);

            var chairTop = (BuildByIdentifier) chairDef.Statements.Last();
            Assert.AreEqual(BuildShape.Box, chairTop.Shape);
            Assert.AreEqual(new Location(1,1,0), chairTop.Location);
            Assert.AreEqual("wood", chairTop.Identifier);

            var floorDef = (ElementDefinition) buildingDef.Statements[1];
            var floorCover = (BuildByIdentifier) floorDef.Statements.First();
            Assert.AreEqual(BuildShape.Box, floorCover.Shape);
            Assert.AreEqual(new Dimension(10,10,1), floorCover.Dimension);
            Assert.AreEqual("stone", floorCover.Identifier);

            var buildingCall = (BuildByIdentifier)statements.Last();// etc
            Assert.AreEqual("building",buildingCall.Identifier);
        }
    }
}
