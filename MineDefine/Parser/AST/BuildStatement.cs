using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Parser.AST
{
    public abstract class BuildStatement : IStatement
    {
        public BuildStatement(BuildShape shape, Dimension dimension, Location location)
        {
            Shape = shape;
            Dimension = dimension;
            Location = location;
        }


        public BuildShape Shape { get; private set; }

        public Dimension Dimension { get; private set; }

        public Location Location { get; private set; }
    }

    public class BuildByIdentifier : BuildStatement
    {
        public BuildByIdentifier(BuildShape shape, Dimension dimension, Location location, string identifier)
            :base(shape, dimension, location)
        {
            Identifier = identifier;
        }

        public string Identifier { get; private set; }
    }

    public class BuildByStatements : BuildStatement
    {
        public BuildByStatements(BuildShape shape, Dimension dimension, Location location, IEnumerable<IStatement> statements)
            :base(shape, dimension, location)
        {
            Statements = ImmutableList.CreateRange(statements);
        }

        public IImmutableList<IStatement> Statements { get; set; }
    }


    public enum BuildShape
    {
        Box,
        Wall,
        TriangularPrisim
    }
}
