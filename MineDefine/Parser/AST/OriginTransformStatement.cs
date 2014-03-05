using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Parser.AST
{
    public class OriginTransformStatement : IStatement
    {
        public OriginTransformStatement(OriginTransformInstructions instructions, IEnumerable<IStatement> statements)
        {
            Instructions = instructions;
            Statements = statements == null ? null : ImmutableList.CreateRange(statements);
        }

        public OriginTransformInstructions Instructions { get; private set; }
        public IImmutableList<IStatement> Statements { get; private set; }
    }

    public abstract class OriginTransformInstructions { }

    public class AbsoluteTransformInstructions : OriginTransformInstructions
    {
        public AbsoluteTransformInstructions(AbsoluteOriginTransform direction, int degree)
        {
            Direction = direction;
            Degree = degree;
        }

        public AbsoluteOriginTransform Direction { get; private set; }
        public int Degree { get; private set; }
    }

    public class RelativeTransformInstructions : OriginTransformInstructions
    {
        public RelativeTransformInstructions(RelativeOriginTransform direction)
        {
            Direction = direction;
        }

        public RelativeOriginTransform Direction { get; private set; }
    }

    public enum AbsoluteOriginTransform
    {
        Up,
        Down,
        North,
        East,
        South,
        West
    }

    public enum RelativeOriginTransform
    {
        Top,
        Bottom,
        Right,
        Left,
        Front,
        Back,
    }
}
