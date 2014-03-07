using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using Substrate;

namespace MineDefine.Synthesis
{
    public class MineDefineLayout
    {
        public ComplexElement Layout(MineDefineAST ast)
        {
            var symbolTable = DefaultSymbolsTable();
            var rootDef = new ComplexElement();
            Plan(ast.Statements, symbolTable, Transform.Identity, rootDef);
            return rootDef;
        }

        private void Plan(IEnumerable<IStatement> statements, SymbolTable symbolTable, Transform transform, ComplexElement def)
        {
            foreach (var statement in statements)
            {
                if (statement is ElementDefinition) {
                    var anotherDef = (ElementDefinition) statement;
                    var ele = new ComplexElement();
                    Plan(anotherDef.Statements, symbolTable.Push(), Transform.Identity, ele);
                    symbolTable = symbolTable.AddSymbol(anotherDef.Name, ele);
                }
                else if (statement is BuildByIdentifier)
                {
                    var build = (BuildByIdentifier) statement;
                    var ele = symbolTable[build.Identifier];
                    var buildInst = new BuildInstruction(build.Shape, build.Dimension, transform.Adjust(build.Location), ele);
                    def.AddBuildInstruction(buildInst);
                }
                else if (statement is BuildByStatements)
                {
                    var build = (BuildByStatements) statement;
                    var ele = new ComplexElement();
                    Plan(build.Statements, symbolTable, Transform.Identity, ele);
                    var buildInst = new BuildInstruction(build.Shape, build.Dimension, transform.Adjust(build.Location), ele);
                    def.AddBuildInstruction(buildInst);
                }
                else if (statement is OriginTransformStatement)
                {
                    var transStatement = (OriginTransformStatement) statement;
                    if (transStatement.Instructions is AbsoluteTransformInstructions)
                    {
                        var absInstr = (AbsoluteTransformInstructions) transStatement.Instructions;
                        var newTrans = transform.Adjust(absInstr.Direction, absInstr.Degree);
                        Plan(transStatement.Statements,symbolTable,newTrans,def);
                    }
                    else
                    {
                        var absInstr = (RelativeTransformInstructions) transStatement.Instructions;
                        var newTrans = transform.Adjust(absInstr.Direction, def);
                        Plan(transStatement.Statements,symbolTable,newTrans,def);
                    }
                }
            }
        }

        private SymbolTable DefaultSymbolsTable()
        {
            var defs = new[] {
                new {name = "wood", type = BlockType.WOOD},
                new {name = "woodplank", type = BlockType.WOOD_PLANK},
                new {name = "stone", type = BlockType.STONE},
                new {name = "sand", type = BlockType.SAND},
                new {name = "sandstone", type = BlockType.SANDSTONE},
                new {name = "wool", type = BlockType.WOOL},
                new {name = "cobblestone", type = BlockType.COBBLESTONE},
                new {name = "door", type = BlockType.WOOD_DOOR},
                new {name = "glass", type = BlockType.GLASS},
                new {name = "glasspane", type = BlockType.GLASS_PANE},
                new {name = "air", type = BlockType.AIR},
                new {name = "rose", type = BlockType.RED_ROSE},
                new {name = "grass", type = BlockType.GRASS},
            };

            return defs.Aggregate(SymbolTable.BaseSymbolTable,
                (acc, x) => acc.AddSymbol(x.name, new SingleBlock(new AlphaBlock(x.type))));
        }
    }
}
