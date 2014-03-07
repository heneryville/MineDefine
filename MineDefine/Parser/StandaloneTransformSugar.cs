using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;

namespace MineDefine.Parser
{
    public class StandaloneTransformSugar
    {
        public MineDefineAST Transform(MineDefineAST ast)
        {
            return new MineDefineAST(Transform(ast.Statements));
        }

        private IEnumerable<IStatement> Transform(IEnumerable<IStatement> statements)
        {
            var enumerator = statements.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var statement = enumerator.Current;
                if (statement is OriginTransformStatement)
                {
                    var trans = (OriginTransformStatement) statement;
                    if (trans.Statements == null)
                    {
                        yield return
                            new OriginTransformStatement(trans.Instructions, Transform(EnumerateOut(enumerator)));
                    }
                    else
                    {
                        yield return new OriginTransformStatement(trans.Instructions, Transform(trans.Statements));
                    }
                }
                else if (statement is BuildByStatements)
                {
                    var build = (BuildByStatements) statement;
                    yield return new BuildByStatements(build.Shape, build.Dimension, build.Location, Transform(build.Statements));
                }
                else if (statement is ElementDefinition)
                {
                    var def = (ElementDefinition) statement;
                    yield return new ElementDefinition(def.Name, Transform(def.Statements));
                }
                else yield return statement;
            }
        }

        private IEnumerable<T> EnumerateOut<T>(IEnumerator<T> enumerator)
        {
            var list = new List<T>();
            while(enumerator.MoveNext()) list.Add(enumerator.Current);
            return list;
        }
    }
}
