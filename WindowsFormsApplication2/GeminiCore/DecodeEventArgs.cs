using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiCore;

namespace GeminiCore
{
    public class DecodeEventArgs : EventArgs
    {
        public CPU.DecodedInstruction CurrentDecodedInstr { get; set; }
        public int CurrentInstructionIndex { get; set; }

        public DecodeEventArgs(CPU.DecodedInstruction ir, int instructionIndex)
        {
            CurrentDecodedInstr = ir;
            CurrentInstructionIndex = instructionIndex;
        }
    }
}
