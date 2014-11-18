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

        public FetchEventArgs(short ir, int instructionIndex)
        {
            CurrentIR = ir;
            CurrentInstructionIndex = instructionIndex;
        }
    }
}
