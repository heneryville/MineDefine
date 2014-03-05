using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;

namespace MineDefine.Synthesis
{
    public struct Transform
    {
        private static Transform identity = new Transform();
        public static Transform Identity { get { return identity; } }

        private int _x;
        private int _y;
        private int _z;

        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int Z { get { return _z; } }

        public Transform(int x, int z, int y) { _x = x; _y = y; _z = z; }

        public Location Adjust(Location location)
        {
            return new Location(location.X + X, location.Z + Z, location.Y + Y);
        }

        public Transform Adjust(AbsoluteOriginTransform direction, int degree)
        {
            switch (direction)
            {
                case AbsoluteOriginTransform.Up: return new Transform(X,Z,Y + degree);
                case AbsoluteOriginTransform.Down: return new Transform(X,Z,Y - degree);
                case AbsoluteOriginTransform.East: return new Transform(X+degree,Z,Y);
                case AbsoluteOriginTransform.West: return new Transform(X-degree,Z,Y);
                case AbsoluteOriginTransform.North: return new Transform(X,Z-degree,Y);
                case AbsoluteOriginTransform.South: return new Transform(X,Z+degree,Y);
                default: throw new RuntimeException("Unknown abosulte transform direction: " + direction);
            }
        }

        public Transform Adjust(RelativeOriginTransform direction, IElement element)
        {
            switch (direction)
            {
                case RelativeOriginTransform.Top: return new Transform(X,Z,element.Height);
                case RelativeOriginTransform.Bottom: return new Transform(X,Z,0);
                case RelativeOriginTransform.Right: return new Transform(element.Width,Z,Y);
                case RelativeOriginTransform.Left: return new Transform(0,Z,Y);
                case RelativeOriginTransform.Front: return new Transform(X,element.Depth,Y);
                case RelativeOriginTransform.Back: return new Transform(X,0,Y);
                default: throw new RuntimeException("Unknown relative transform direction: " + direction);
            }
        }

        public Location ToLocation()
        {
            return new Location(X,Z,Y);
        }
    }
}
