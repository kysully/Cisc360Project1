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
        public short CurrentInstr { get; set; }
        public int CurrentInstructionIndex { get; set; }

        public ExecuteEventArgs(short binary, int Execute_Counter)
        {
            this.CurrentInstr = binary;
            this.CurrentInstructionIndex = Execute_Counter;
        }
    }
}
