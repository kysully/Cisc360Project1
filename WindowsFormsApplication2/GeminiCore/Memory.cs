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
    public class Memory
    {
        static List<short> binaryInstructions = new List<short>(10);
        static List<string> assemblyInstructions = new List<string>(10);
        int[] memory;

        public Memory()
        {
            binaryInstructions = new List<short>(10); // Default instruction size is 10
            assemblyInstructions = new List<string>(10); // Default instruction size 10
            memory = new int[256]; // Default memory size is 256
        }

        public static void clearInstructions()
        {
            binaryInstructions = new List<short>(0);
            assemblyInstructions = new List<string>(0);
        }

        public static void setAssemblyInstructions(List<string> instr){
            assemblyInstructions = instr;
        }

        public static List<string> getAssemblyInstructions()
        {
            return assemblyInstructions;
        }

        public static void setBinaryInstructions(List<short> instr)
        {
            binaryInstructions = instr;
        }

        public static List<short> getBinaryInstructions()
        {
            return binaryInstructions;
        }

    }
}
