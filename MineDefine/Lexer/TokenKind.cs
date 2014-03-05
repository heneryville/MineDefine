using System.Collections.Generic;

namespace MineDefine.Lexer
{
    public enum TokenKind
    {
        //Syntax
        EOF = 0,
        OpenBrace,
        CloseBrace,
        EndOfLine,
        Colon,

        //Absolute Transform Direction
        Up, Down, North, South, East, West,

        //Relative Transform Direction
        Top, Bottom, Left, Right, Front, Back,

        Numeral,
        Identifier,
        Operation,
        Stack,
        Count,
        Dimension,
        Location,

    }

    public static class TokenKindExtensions
    {
        public static ISet<TokenKind> relativeDirections = new HashSet<TokenKind>(){ TokenKind.Top, TokenKind.Bottom, TokenKind.Left, TokenKind.Right, TokenKind.Front, TokenKind.Back };
        public static ISet<TokenKind> abosluteDirections = new HashSet<TokenKind>(){ TokenKind.Up, TokenKind.Down, TokenKind.North, TokenKind.South, TokenKind.East, TokenKind.West };

        public static bool IsTransformDirection(this TokenKind kind)
        {
            return kind.IsAbsoulteTransformDirection() || kind.IsRelativeTransformDirection();
        }

        public static bool IsAbsoulteTransformDirection(this TokenKind kind)
        {
            return abosluteDirections.Contains(kind);
        }
        public static bool IsRelativeTransformDirection(this TokenKind kind)
        {
            return relativeDirections.Contains(kind);
        }
    }
}
