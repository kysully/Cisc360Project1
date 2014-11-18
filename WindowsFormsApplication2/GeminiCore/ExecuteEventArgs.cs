using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiCore;

namespace GeminiCore
{
    public class ExecuteEventArgs : EventArgs
    {
        public CPU.DecodedInstruction CurrentDecodedInstr { get; set; }
        public int CurrentInstructionIndex { get; set; }

        public ExecuteEventArgs()
        {
        }
    }
}
