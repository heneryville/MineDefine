using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using Substrate;

namespace Tests
{
    public class FakeStamp : IBlockStamp
    {
        private Dictionary<Location,AlphaBlock> placements = new Dictionary<Location, AlphaBlock>();

        public void PlaceBlock(AlphaBlock block, Location loc)
        {
            placements[loc] = block;
        }

        public void AssertPlaced(Location loc)
        {
            var isPlaced = placements.ContainsKey(loc);
            Assert.IsTrue(isPlaced, "Expected a block to be placed at " + loc);
        }

        public void AssertPlaced(int x, int z, int y)
        {
            var loc = new Location(x, z, y);
            AssertPlaced(loc);
        }

        public int Mass { get { return placements.Count; } }
    }
}
