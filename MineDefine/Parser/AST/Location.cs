using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineDefine.Parser.AST
{
    public struct Location
    {
        private int _x;
        private int _z;
        private int _y;

        public Location(int x = 0, int z = 0, int y = 0)
        {
            _x = x;
            _z = z;
            _y = y;
        }
       
        public int X { get { return _x; } } //Left to Right
        public int Z { get { return _z; }} //Front to back (Negative is northward)
        public int Y { get { return _y; }}

        public override string ToString()
        {
            return X + "," + Z + "," + Y;
        }


        public static Location Origin { get { return origin; } }
        private static Location origin = new Location();

//Up to down

        public static Location Parse(string value)
        {
            var ints = value.Split(',').Select(int.Parse).ToList();
            if(ints.Count > 3 || ints.Count <= 0) throw new ArgumentException("Cannot parse " + value + " into a location");
            ints.AddRange(Enumerable.Range(0,3 - ints.Count).Select(x => 0));
            return new Location(ints[0], ints[1], ints[2]);
        }
    }
}
