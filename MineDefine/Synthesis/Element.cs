using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine.Parser.AST;
using MineDefine.Synthesis.Shapes;
using Substrate;
using Tests.Synthesis.Shapes;
using Dimension = MineDefine.Parser.AST.Dimension;

namespace MineDefine.Synthesis
{
    public interface IElement
    {
        int Width { get; }
        int Depth { get; }
        int Height { get; }
        void Build(IBlockStamp stamp, Transform transform);
    }

    public class ComplexElement : IElement
    {
        public int Width { get; private set; }
        public int Depth { get; private set; }
        public int Height { get; private set; }

        public void Build(IBlockStamp stamp, Transform transform)
        {
            foreach (var buildInstruction in BuildInstructions)
            {
                buildInstruction.Build(stamp, transform);
            }
        }

        public IList<BuildInstruction> BuildInstructions { get; private set; }

        public ComplexElement()
        {
            BuildInstructions = new List<BuildInstruction>();
        }

        public void AddBuildInstruction(BuildInstruction instruction)
        {
            BuildInstructions.Add(instruction);
            Width = Math.Max(Width, instruction.Width + instruction.Location.X);
            Depth = Math.Max(Depth, instruction.Depth + instruction.Location.Z);
            Height = Math.Max(Height, instruction.Height + instruction.Location.Y);
        }
    }

    public class BuildInstruction : IElement
    {
        public Location Location { get; private set; }
        private readonly BuildShape _shape;
        private readonly Dimension _dimension;
        private readonly IElement _toBuild;

        public int Width { get; private set; }
        public int Depth { get; private set; }
        public int Height { get; private set; }

        public void Build(IBlockStamp stamp, Transform transform)
        {
            IShapePlan plan = null;
            switch (_shape)
            {
                case BuildShape.Box:
                    plan = new BoxPlan(_dimension);
                    break;
                case BuildShape.Wall:
                    plan = new WallPlan(_dimension);
                    break;
                default: throw new RuntimeException("Unknown shape: " + _shape);
            }
            foreach (var location in plan.GetLocations()
                        .Select(x => new Location(
                            x.X*_toBuild.Width + Location.X + transform.X,
                            x.Z*_toBuild.Depth + Location.Z + transform.Z,
                            x.Y*_toBuild.Height + Location.Y + transform.Y
                            )))
            {
                _toBuild.Build(stamp, new Transform(location.X, location.Z, location.Y));
            }
        }

        public BuildInstruction(BuildShape shape, Dimension dimension, Location location, IElement toBuild)
        {
            _shape = shape;
            _dimension = dimension;
            _toBuild = toBuild;

            Width = dimension.Width*toBuild.Width;
            Depth = dimension.Depth*toBuild.Depth;
            Height = dimension.Height*toBuild.Height;

            Location = location;
        }
    }

    public class SingleBlock : IElement
    {
        private readonly AlphaBlock _block;

        public SingleBlock(AlphaBlock block)
        {
            _block = block;
        }

        public int Width { get { return 1; } }
        public int Depth { get { return 1; } }
        public int Height { get { return 1; } }

        public void Build(IBlockStamp stamp, Transform trans)
        {
            stamp.PlaceBlock(_block, trans.ToLocation());
        }

        public Location Location { get { return Location.Origin; } }
    }
}
