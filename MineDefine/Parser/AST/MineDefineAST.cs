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
    }
}
