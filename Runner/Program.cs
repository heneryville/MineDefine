using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MineDefine;
using MineDefine.Synthesis;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mineDefineFile = new FileStream(args[0],FileMode.Open);
            var worldName = args[1];

            Console.WriteLine("Compiling");
            var exe = MineDefineCompiler.Compile(mineDefineFile);
            exe.Create(worldName);
        }
    }
}
