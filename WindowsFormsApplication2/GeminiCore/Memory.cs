/**
 * Kyle Sullivan and Melody Lugo
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GeminiCore
{
    public class Memory
    {
        static List<short> binaryInstructions = new List<short>(10);
        static List<string> assemblyInstructions = new List<string>(10);
        public static int[] stack = new int[256];
        Cache cache;
        int cacheSize;
        public int writeHitCounter { get; private set; }
        public int readHitCounter { get; private set; }
        public int writeMissCounter { get; private set; }
        public int readMissCounter { get; private set; }
        String hitOrMiss = "---------";
        
        bool addressMode;

        /*class Cache
        {
            public static int[] cacheStack;
            public int[] getCacheStack(){
                return cacheStack;
            }
            public void setCacheStack(int size){
                cacheStack = new int[size];
            }
        }*/

        class Cache
        {
            private int[] arr;

            public Cache(int size)
            {
                arr = new int[size];
            }
            public int this[int i]
            {
                get
                {
                    //Debug.WriteLine("Do Read stuff");
                    return arr[i];
                }
                set
                {
                    //Debug.WriteLine("Do write stuff");
                    arr[i] = value;
                }
            }
        }

        //Reading
        int getStackValue(int stackIndex)
        {
            //true = 2way, false = 1way
            if (!addressMode)
            {//1 way
                int temp = cache[stackIndex % cacheSize];
                int tag = temp >> 24;
                if (tag == stackIndex)
                {
                    Debug.WriteLine("Hit: requested item at stack[" + stackIndex + "] and it was in cache. Yay :D");
                    Debug.WriteLine("readhit counter is: " + readHitCounter++);
                    return ((temp & 16711680) >> 16);
                }
                else
                {
                    Debug.WriteLine("Miss: requested item at stack[" + stackIndex + "] and stack[" + tag + "] was there instead. Boo :(");
                    Debug.WriteLine("readmiss counter is: " + readMissCounter++);
                    //Taking value from memory and putting it into stack
                    int dirty = temp << 31;
                    if (dirty == -2147483648)
                    {
                        int data = ((temp & 16711680) >> 16);
                        stack[tag] = data;
                        Debug.WriteLine("Replaced stack[" + tag + "] with the value " + data);
                    }

                    cache[stackIndex % cacheSize] = (stackIndex << 24) | (stack[stackIndex] << 16);
                    Debug.WriteLine("Cache[" + (stackIndex % cacheSize) + "] has the value of " + stack[stackIndex]);
                    return ((cache[stackIndex % cacheSize] & 16711680) >> 16); // using a mask to extract the data bits
                }


            }
            return stack[0];
        }

        void writeStackValue(int stackIndex, int value){
            if(!addressMode)
            {//1 way
                int temp = cache[stackIndex % cacheSize];
                int tag = temp >> 24;
                if (tag == stackIndex)
                {//hit
                    
                    cache[stackIndex % cacheSize] = tag | (value << 16) | 1;
                    int data = ( (cache[stackIndex % cacheSize] & 16711680) >> 16);
                    Debug.WriteLine("Write Hit: writing to item at stack[" + stackIndex + "] and it was in cache, and now its marked dirty");
                    Debug.WriteLine("write hit counter is: " + writeHitCounter++);
                    Debug.WriteLine("Value at cache index[" + (stackIndex % cacheSize) + "] is " + data);
                }
                else
                {//miss
                    Debug.WriteLine("Write Miss: writing to stack[" + stackIndex + "] and stack[" + tag + "] was in cache instead.");
                    Debug.WriteLine("writemiss counter is: " + writeMissCounter++);
                    stack[stackIndex] = value;
                    Debug.WriteLine("Wrote " + value + " to stack[" + stackIndex + "]");

                }
            }     
        }

        //How do you implement block size?

        public Memory(int cacheSize, bool addressMode)
        {
            binaryInstructions = new List<short>(10); // Default instruction size is 10
            assemblyInstructions = new List<string>(10); // Default instruction size 10
            stack = new int[256]; // Default memory size is 256
            cache = new Cache(cacheSize);
            this.cacheSize = cacheSize;
            this.addressMode = addressMode;
            readHitCounter = 0;
            readMissCounter = 0;
            writeHitCounter = 0;
            writeMissCounter = 0;
            Debug.WriteLine("Just made a new memory object with stack of size 256 and cache of size " + cacheSize);
            //testMemory();
        }

        void testMemory()
        {
            for (int i = 0; i < 256; i++)
            {
                stack[i] = i;
            }

            this[0] = 7;
            int temp = this[4];
            Debug.WriteLine("Item at stack 4 = " + temp);
        }

        public int this[int stackIndex]
        {
            get
            {
                Debug.WriteLine("Trying to read from stack[" + stackIndex + "]");
                return getStackValue(stackIndex);
            }
            set
            {
                Debug.WriteLine("Trying to write to stack[" + stackIndex + "]");
                writeStackValue(stackIndex, value);
            }
        }

        public static void at(IEnumerable<int> x)
        {
            //TODO not really sure yeat
        }

        //Resets the stack back to 0's
        public static void clearStack()
        {
            stack = new int[256];
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

        public static void setCacheStackPointer(int size)
        {
            //cacheStackPointer.setCacheStack(size);
        }

        

    }

  
}
