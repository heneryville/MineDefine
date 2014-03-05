using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using Substrate;

namespace MineDefine.Synthesis
{
    public interface IBlockStamp
    {
        void PlaceBlock(AlphaBlock block, Location loc);
    }

    public class BlockStamp : IBlockStamp
    {
        private readonly NbtWorld _world;

        public BlockStamp(NbtWorld world)
        {
            _world = world;
        }

        public void PlaceBlock(AlphaBlock block, Location loc)
        {
            _world.GetBlockManager().SetBlock(loc.X, loc.Y, loc.Z, block);
        }
    }
}
