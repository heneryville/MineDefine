using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;

namespace Tests.Parser.AST
{
    public class ElementDefinition : IStatement
    {
        public string Name { get; private set; }
        public IImmutableList<IStatement> Statements { get; private set; }

        public ElementDefinition(string name, IEnumerable<IStatement> statements)
        {
            Name = name;
            Statements = ImmutableList.CreateRange(statements);
        }
    }
}
