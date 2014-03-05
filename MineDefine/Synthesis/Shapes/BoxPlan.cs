using System.Collections.Generic;
using MineDefine.Parser.AST;
using Tests.Synthesis.Shapes;

namespace MineDefine.Synthesis.Shapes
{
    public class BoxPlan : IShapePlan
    {
        private readonly Dimension _dim;

        public BoxPlan(Dimension dim)
        {
            _dim = dim;
        }

        public IEnumerable<Location> GetLocations()
        {
            for (int x = 0; x < _dim.Width; ++x)
            {
                for (int z = 0; z < _dim.Depth; ++z)
                {
                    for (int y = 0; y < _dim.Height; ++y)
                    {
                        yield return new Location(x,z,y);
                    }
                }
            }
        }
    }
}
