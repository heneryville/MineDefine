using System.Collections.Generic;
using MineDefine.Parser.AST;
using Tests.Synthesis.Shapes;

namespace MineDefine.Synthesis.Shapes
{
    public class WallPlan : IShapePlan
    {
        private readonly Dimension _dim;

        public WallPlan(Dimension dim)
        {
            _dim = dim;
        }

        public IEnumerable<Location> GetLocations()
        {
            for (int x = 0; x < _dim.Width; ++x)
            {
                for (int z = 0; z < _dim.Depth; ++z)
                {
                    if( x > 0 && x < _dim.Width -1 && z > 0 && z < _dim.Depth -1 )continue;
                    for (int y = 0; y < _dim.Height; ++y)
                    {
                        yield return new Location(x,z,y);
                    }
                }
            }
        }
    }
}
