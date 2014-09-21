﻿/**
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
        List<short> instructions;
        int[] memory;

        public Memory()
        {
            instructions = new List<short>(10); // Default instruction size is 10
            memory = new int[256]; // Default memory size is 256
        }
    }
}
