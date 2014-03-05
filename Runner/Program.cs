using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mineDefineFile = new FileStream(args[0],FileMode.Open);
            var worldName = args[1];

            var md = new MineDefine.MineDefine();
            md.Build(mineDefineFile, worldName);
        }
    }
}
