using System;
using System.Collections.Generic;
using MineDefine.Parser.AST;
using Tests.Synthesis.Shapes;

namespace MineDefine.Synthesis.Shapes
{
    public class TriangularPrismPlan : IShapePlan
    {
        private readonly Dimension _dim;

        public TriangularPrismPlan(Dimension dim)
        {
            _dim = dim;
        }

        public IEnumerable<Location> GetLocations()
        {

            var states = _dim.Width/2 + (_dim.Width % 2 == 0 ? 0 : 1);
            var levelsPerState = _dim.Depth/states;
            var statesPerLevel = states/_dim.Depth;

            if (levelsPerState > 0)
            {
                var overflow = _dim.Depth%states;
                for (int z = 0; z < _dim.Depth; ++z)
                {
                    var indents = Math.Max(0, z - overflow)/levelsPerState;
                    for (int x = 0; x < _dim.Width; ++x)
                    {
                        if (x < indents || x > _dim.Width - indents - 1) continue;
                        for (int y = 0; y < _dim.Height; ++y)
                        {
                            yield return new Location(x, z, y);
                        }
                    }
                }
            }
            else
            {
                for (int z = 0; z < _dim.Depth; ++z)
                {
                    var indents = z*statesPerLevel;
                    for (int x = 0; x < _dim.Width; ++x)
                    {
                        if (x < indents || x > _dim.Width - indents - 1) continue;
                        for (int y = 0; y < _dim.Height; ++y)
                        {
                            yield return new Location(x, z, y);
                        }
                    }
                }
            }

        }
    }
}
