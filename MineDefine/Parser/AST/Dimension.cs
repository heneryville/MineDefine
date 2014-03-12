using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Parser.AST
{
    public struct Dimension
    {
        private int _width;
        private int _depth;
        private int _height;

        public Dimension(int width = 1, int depth = 1, int height = 1)
        {
            _width = width;
            _depth = depth;
            _height = height;
        }

        public int Width { get { return _width; } }
        public int Depth { get { return _depth; } }
        public int Height { get { return _height; } }

        private static Dimension _default = new Dimension(1,1,1);

        public static Dimension Default { get { return _default; } }

        public static Dimension Parse(string value)
        {
            var ints = value.Split('x').Select(int.Parse).ToList();
            if(ints.Count > 3 || ints.Count <= 0) throw new ArgumentException("Cannot parse " + value + " into a dimension");
            ints.AddRange(Enumerable.Range(0,3 - ints.Count).Select(x => 1));
            return new Dimension(ints[0], ints[1], ints[2]);
        }

        public override string ToString()
        {
            return "" + Width + "x" + Depth + "x" + Height;
        }
    }
}
