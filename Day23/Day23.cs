using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day23
{
    class Day23 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var nics = new Executable[50];

            for(int i = 0; i< nics.Length; i++)
            {
                nics[i] = new Executable(null, (long[])code.Clone());
                nics[i].AddInput(i);
            }
            long x, y, address;
            bool halt;

            while (true)
            {
                for(int i = 0; i< nics.Length; i++) { 

                    (address, halt) = nics[i].Execute();

                    if (address == -1)
                        continue;

                    (x, halt) = nics[i].Execute();
                    (y, halt) = nics[i].Execute();

                    Console.WriteLine($"NIC {i}: Send X={x}, Y={y} to {address}.");

                    if (address == 255)
                        return y.ToString();

                    nics[address].AddInput(x);
                    nics[address].AddInput(y);

                    if (halt)
                        break;

                    
                }
            }
        }

        public override string SolveSecondPuzzle()
        {
            var code = ReadInputText<string>().Split(',').Select(long.Parse).ToArray();
            var nics = new Executable[50];

            for (int i = 0; i < nics.Length; i++)
            {
                nics[i] = new Executable(null, (long[])code.Clone());
                nics[i].AddInput(i);
            }
            long x, y, address;
            long natx=0, naty = 0;
            bool halt;
            long prevNaty = 0;

            while (true)
            {
                for (int i = 0; i < nics.Length; i++)
                {
                    (address, halt) = nics[i].Execute();

                    if (nics.All(x => x.Idle))
                    {
                        if (naty == prevNaty)
                            return prevNaty.ToString();

                        nics[0].AddInput(natx);
                        nics[0].AddInput(naty);
                        nics[0].Idle = false;
                        prevNaty = naty;
                    }

                    if (address == -1)
                        continue;

                    (x, halt) = nics[i].Execute();
                    (y, halt) = nics[i].Execute();

                    Console.WriteLine($"NIC {i}: Send X={x}, Y={y} to {address}.");

                    if (address == 255)
                    {
                        natx = x;
                        naty = y;
                        continue;
                    }


                    nics[address].AddInput(x);
                    nics[address].AddInput(y);

                }
            }
        }
    }
}
