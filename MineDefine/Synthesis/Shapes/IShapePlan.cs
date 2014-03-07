using System.Collections.Generic;
using MineDefine.Parser.AST;

namespace MineDefine.Synthesis.Shapes
{
    public interface IShapePlan
    {
        IEnumerable<Location> GetLocations();
    }
}
