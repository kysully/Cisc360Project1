using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiCore;

namespace GeminiCore
{
    public class StageDoneEventArgs : EventArgs
    {
        //public short CurrentInstr { get; set; }
        public int CurrentInstructionIndex { get; set; }
        public CPU.StageType stageType { get; set; }

        public StageDoneEventArgs(int instrIndex, CPU.StageType stageType)
        {
            //this.CurrentInstr = instr.binary;
            this.CurrentInstructionIndex = instrIndex;
            this.stageType = stageType;
        }
        public StageDoneEventArgs() { }
    }
}
