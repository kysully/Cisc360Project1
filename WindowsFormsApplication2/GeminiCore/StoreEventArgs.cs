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

        public StoreEventArgs(int Store_Counter)
        {
            this.CurrentInstructionIndex = Store_Counter;
        }
    }
}
