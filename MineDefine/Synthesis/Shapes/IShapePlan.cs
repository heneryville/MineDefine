using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;

namespace Tests.Synthesis.Shapes
{
    public interface IShapePlan
    {
        IEnumerable<Location> GetLocations();
    }
}
