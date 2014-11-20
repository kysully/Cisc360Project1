using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiCore;

namespace GeminiCore
{
    public class StoreEventArgs : EventArgs
    {
        public int CurrentInstructionIndex { get; set; }
        public bool programDone { get; private set; }
        public StoreEventArgs(int Store_Counter, bool programDone)
        {
            this.CurrentInstructionIndex = Store_Counter;
            this.programDone = programDone;
        }
    }
}
