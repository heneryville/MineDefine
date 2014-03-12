using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Parser.AST
{
    public class MineDefineAST
    {
        public MineDefineAST(IEnumerable<IStatement> statements)
        {
            Statements = ImmutableList.CreateRange(statements);
        }

        public IImmutableList<IStatement> Statements { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(this,sb,0);
            return sb.ToString();
        }

        private void Print(object element, StringBuilder sb, int tabs)
        {
            sb.Append(Tab(tabs));
            if (element is MineDefineAST)
            {
                var ast = (MineDefineAST) element;
                sb.AppendLine("Program");
                foreach (var statement in ast.Statements)
                {
                    Print(statement,sb,tabs+1);
                }
            }
            else if (element is ElementDefinition)
            {
                var def = (ElementDefinition) element;
                sb.AppendLine("Definition: " + def.Name);
                foreach (var statement in def.Statements)
                {
                    Print(statement,sb,tabs+1);
                }

            }
            else if (element is OriginTransformStatement)
            {
                var trans = (OriginTransformStatement) element;
                if (trans.Instructions is AbsoluteTransformInstructions)
                {
                    var absTrans = (AbsoluteTransformInstructions) trans.Instructions;
                    sb.Append("Transform: ").Append(absTrans.Direction).Append(" ").Append(absTrans.Degree).AppendLine();
                }
                else if (trans.Instructions is RelativeTransformInstructions)
                {
                    var relTrans = (RelativeTransformInstructions) trans.Instructions;
                    sb.Append("Transform: ").Append(relTrans.Direction).AppendLine();
                }
                if (trans.Statements != null)
                {
                    foreach (var statement in trans.Statements)
                    {
                        Print(statement,sb,tabs+1);
                    }
                }
            }
            else if (element is BuildStatement)
            {
                var build = (BuildStatement) element;
                sb.Append("Build: ");
                if (build.Shape != BuildShape.Box) sb.Append(build.Shape).Append(" ");
                if (! build.Dimension.Equals(Dimension.Default)) sb.Append(build.Dimension).Append(" ");
                if (! build.Location.Equals(Location.Origin)) sb.Append(build.Location).Append(" ");

                if (build is BuildByIdentifier)
                {
                    var buildById = (BuildByIdentifier) build;
                    sb.AppendLine(buildById.Identifier);
                }
                else if (build is BuildByStatements)
                {
                    var buildByStats = (BuildByStatements) build;
                    sb.AppendLine();
                    foreach (var statement in buildByStats.Statements)
                    {
                        Print(statement,sb,tabs+1);
                    }
                }
            }
        }

        private string Tab(int tabs)
        {
            return new string(Enumerable.Repeat('.', tabs).ToArray());
        }
    }
}
