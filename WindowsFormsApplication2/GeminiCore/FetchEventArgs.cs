/**
 * Kyle Sullivan and Melody Lugo
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiCore
{
    public class FetchEventArgs : EventArgs
    {
        public short CurrentIR { get; set; }
        public int CurrentInstructionIndex { get; set; }

        public FetchEventArgs(CPU.FetchedInstruction instruction)
        {
            CurrentIR = instruction.binary;
            CurrentInstructionIndex = instruction.index;
        }
    }
}
