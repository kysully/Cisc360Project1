/**
 * Seth Morecraft
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiCore
{
    public class CPU
    {
        public short ACC { get; private set; }
        public short A { get; private set; }
        public short B { get; private set; }
        public short PC { get; private set; }
        public readonly short ONE = 1;
        public readonly short ZERO = 0;
        public CPU()
        {
            ACC = 0;
        }

        public void nextInstruction()
        {
            ACC++;
        }
    }
}
