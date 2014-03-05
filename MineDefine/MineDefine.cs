using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Lexer;
using MineDefine.Parser;
using MineDefine.Parser.AST;
using MineDefine.Synthesis;
using Substrate;
using Substrate.Core;

namespace MineDefine
{
    public class MineDefine
    {

        public void Build(Stream mineDefineInput, string worldName)
        {
            NbtWorld nbtWorld;
            if (Directory.Exists(worldName)) nbtWorld = NbtWorld.Open(worldName);
            else
            {
                Directory.CreateDirectory(worldName);
                nbtWorld = AnvilWorld.Create(worldName);
            }
            var chunks = CreateWorld(nbtWorld);

            var tokens = new MineDefineLexer(mineDefineInput).Lex();
            var ast = new MineDefineParser(tokens).Parse();
            ast = new StandaloneTransformSugar().Transform(ast);
            var stamp = new WorldStamp(nbtWorld);
            var synth = new Synthesizer(stamp);
            synth.Place(ast,new Transform(0,0,9));

            foreach (var chunk in chunks)
            {
                // Reset and rebuild the lighting for the entire chunk at once
                chunk.Blocks.RebuildHeightMap();
                chunk.Blocks.RebuildBlockLight();
                chunk.Blocks.RebuildSkyLight();
            }

            stamp.Save();
            nbtWorld.Save();
        }

        
        static void FlatChunk(ChunkRef chunk, int height)
        {
            // Create bedrock
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunk.Blocks.SetID(x, y, z, (int)BlockType.BEDROCK);
                    }
                }
            }

            // Create stone
            for (int y = 2; y < height - 5; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunk.Blocks.SetID(x, y, z, (int)BlockType.STONE);
                    }
                }
            }

            // Create dirt
            for (int y = height - 5; y < height - 1; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunk.Blocks.SetID(x, y, z, (int)BlockType.DIRT);
                    }
                }
            }

            // Create grass
            for (int y = height - 1; y < height; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunk.Blocks.SetID(x, y, z, (int)BlockType.GRASS);
                    }
                }
            }
        }

        private IEnumerable<ChunkRef> CreateWorld(NbtWorld world)
        {
            var cm = world.GetChunkManager();
            int xmin = -4;
            int xmax = 4;
            int zmin = -4;
            int zmaz = 4;

            world.Level.AllowCommands = true;
            world.Level.LevelName = "MineDefined";
            world.Level.Spawn = new SpawnPoint(-5, -5, 9);

            world.Level.SetDefaultPlayer();
            world.Level.Player.GameType= PlayerGameType.Creative;
            world.Level.Player.Position = new Vector3() {
                X = -5,
                Y = 9,
                Z = -5
            };
            world.Level.Player.IsOnGround = false;
            var chunks = new List<ChunkRef>();

            // We'll create chunks at chunk coordinates xmin,zmin to xmax,zmax
            for (int xi = xmin; xi < xmax; xi++)
            {
                for (int zi = zmin; zi < zmaz; zi++)
                {
                    // This line will create a default empty chunk, and create a
                    // backing region file if necessary (which will immediately be
                    // written to disk)
                    ChunkRef chunk = cm.CreateChunk(xi, zi);
                    chunks.Add(chunk);

                    // This will suppress generating caves, ores, and all those
                    // other goodies.
                    chunk.IsTerrainPopulated = true;

                    // Auto light recalculation is horrifically bad for creating
                    // chunks from scratch, because we're placing thousands
                    // of blocks.  Turn it off.
                    chunk.Blocks.AutoLight = false;

                    // Set the blocks
                    FlatChunk(chunk, 8);

                    Console.WriteLine("Built Chunk {0},{1}", chunk.X, chunk.Z);

                    // Save the chunk to disk so it doesn't hang around in RAM
                }
            }
            return chunks;
        }

    }
}
