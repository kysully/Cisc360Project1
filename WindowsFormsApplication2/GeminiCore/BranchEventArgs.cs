using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiCore
{
    public class BranchEventArgs : EventArgs
    {
        public CPU.DecodedInstruction CurrentIR { get; set; }
        public int CurrentInstrIndex { get; set; }
        public bool taken { get; set; }
        public BranchEventArgs(CPU.DecodedInstruction instruction, int instructionIndex, bool taken)
        {
            //this is the branch instruction that was taken, and the index of said instruction
            CurrentIR = instruction;
            CurrentInstrIndex = instructionIndex;
            this.taken = taken;
        }
    }
}
