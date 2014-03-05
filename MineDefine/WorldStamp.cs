using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using Substrate;
using Substrate.Core;

namespace MineDefine
{
    public class WorldStamp : IBlockStamp
    {
        private readonly NbtWorld _world;
        private IChunkManager _chunkManager;
        private IBlockManager _blockManager;

        public WorldStamp(NbtWorld world)
        {
            _world = world;
            _chunkManager = _world.GetChunkManager();
            _blockManager = _world.GetBlockManager();
        }

        public void PlaceBlock(AlphaBlock block, Location loc)
        {
            _blockManager.SetID(loc.X, loc.Y, loc.Z,block.ID);
            Console.WriteLine("Setting block at " + loc);
        }

        public void Save()
        {
            _chunkManager.Save();
        }
    }
}
