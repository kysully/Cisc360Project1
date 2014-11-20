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
        public Cache cache;
        public String[] copyCache;
        public int cacheSize { get; private set; }
        public int blockSize { get; private set; }
        public int writeHitCounter { get; private set; }
        public int readHitCounter { get; private set; }
        public int writeMissCounter { get; private set; }
        public int readMissCounter { get; private set; }
        public int spatialCounter { get; private set; }
        public string hitOrMiss = "---------";
        bool addressMode;

        ///////////////////////////////////////////////////////////
        //New to Project 2, class which handles underlying cache///
        ///////////////////////////////////////////////////////////
        public class Cache
        {   private int[] arr;
            public Cache(int size) {arr = new int[size];}
            public int this[int i]
            {
             get{return arr[i];}
             set{arr[i] = value;}
            }
        }

        ////////////////////////////////////////////////
        //New to Project 2, method which handles reads//
        ////////////////////////////////////////////////
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
                    ++readHitCounter;
                    Debug.WriteLine("readhit counter is: " + readHitCounter);
                    hitOrMiss = "Hit";
                    Debug.WriteLine(temp);
                    if ((temp & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }
                     return ((temp & 16711680) >> 16);
                }
                else
                {
                    Debug.WriteLine("Miss: requested item at stack[" + stackIndex + "] and stack[" + tag + "] was there instead. Boo :(");
                    ++readMissCounter;
                    Debug.WriteLine("readmiss counter is: " + readMissCounter);
                    hitOrMiss = "Miss";
                    //Taking value from memory and putting it into stack
                    int dirty = temp << 31;
                    if (dirty == -2147483648)
                    {
                        int data = ((temp & 16711680) >> 16);
                        stack[tag] = data;
                        Debug.WriteLine("Replaced stack[" + tag + "] with the value " + data);
                    }

                    //The main read from stack and store on cache call
                    cache[stackIndex % cacheSize] = (stackIndex << 24) | (stack[stackIndex] << 16);
                    
                    //If block size is 2, we want to read the partner value onto cache as well
                    //but first check to ensure we arent overwritting a dirty value on cache
                    if (blockSize > 1)
                    {
                        Debug.WriteLine("Block size bigger than 1");
                        if ((stackIndex % 2) == 0)
                        {
                            int check = cache[(stackIndex + 1) % cacheSize];// = ((stackIndex+1) << 24) | (stack[stackIndex+1] << 16);
                            //before we overwrite the value on cache we check if it needs to be saved onto the stack
                            if ((check << 31) == -2147483648 )
                            {
                                int data = ((check & 16711680) >> 16);
                                stack[(check>>24)] = data;
                            }
                            Debug.WriteLine("Spatial value is: " + (((stackIndex + 1) << 24) | (stack[stackIndex + 1] << 16) | 2));
                            cache[(stackIndex + 1) % cacheSize] = ((stackIndex + 1) << 24) | (stack[stackIndex + 1] << 16) | 2; // or'd with 2 to give it a spatial flag
                        }
                        else
                        {
                            int check = cache[(stackIndex - 1) % cacheSize];// = ((stackIndex+1) << 24) | (stack[stackIndex+1] << 16);
                            //before we overwrite the value on cache we check if it needs to be saved onto the stack
                            if ((check << 31) == -2147483648)
                            {
                                int data = ((check & 16711680) >> 16);
                                stack[(check >> 24)] = data;
                            }
                            Debug.WriteLine("Spatial value is: " + (((stackIndex - 1) << 24) | (stack[stackIndex - 1] << 16) | 2));
                            cache[(stackIndex - 1) % cacheSize] = ((stackIndex-1) << 24) | (stack[stackIndex-1] << 16) | 2;
                        }
                        Debug.WriteLine("Performed a spatial load");
                    }
                    Debug.WriteLine("Cache[" + (stackIndex % cacheSize) + "] has the value of " + stack[stackIndex]);
                    return ((cache[stackIndex % cacheSize] & 16711680) >> 16); // using a mask to extract the data bits
                }


            }
            else
            {//2 way
                int way0 = cache[(stackIndex % (cacheSize / 2)) * 2]; // cache index = stackindex % num sets
                int way1 = cache[((stackIndex) % ((cacheSize / 2))) * 2 + 1];
                if ((way0 >> 24) == stackIndex)
                {//way 0 read hit

                    if ((way0 & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }
                    ++readHitCounter;
                    Debug.WriteLine("Way 0 read hit, stack["+stackIndex+"] was found at cache["+(way0>>24)+"]");
                    hitOrMiss = "Hit";
                    return ((way0 & 16711680) >> 16);

                }
                else if ((way1 >> 24) == stackIndex)
                {//way 1 read hit
                    if ((way1 & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }
                    ++readHitCounter;
                    Debug.WriteLine("Way 1 read hit, stack[" + stackIndex + "] was found at cache[" + (way1 >> 24) + "]");
                    hitOrMiss = "Hit";
                    return ((way1 & 16711680) >> 16);
                }
                else
                {// 2 way read miss

                    Random rnd = new Random();
                    int offset = rnd.Next(0, 2);

                    if (blockSize > 1)
                    {
                        Debug.WriteLine("Block size bigger than 1");
                        if ((stackIndex % 2) == 0)
                        {
                            int replacePartnerIndex = ((stackIndex + 1) % (cacheSize / 2)) * 2 + offset;
                            int replacePartner = cache[replacePartnerIndex];
                            //before we overwrite the value on cache we check if it needs to be saved onto the stack
                            if ((replacePartner << 31) == -2147483648)
                            {
                                int data = ((replacePartner & 16711680) >> 16);
                                stack[(replacePartner >> 24)] = data;
                            }
                            Debug.WriteLine("Spatial value is: " + (((stackIndex + 1) << 24) | (stack[stackIndex + 1] << 16) | 2));
                            cache[replacePartnerIndex] = ((stackIndex + 1) << 24) | (stack[stackIndex + 1] << 16) | 2; // or'd with 2 to give it a spatial flag
                        }
                        else
                        {
                            int replacePartnerIndex = ((stackIndex + -1) % (cacheSize / 2)) * 2 + offset;
                            int replacePartner = cache[replacePartnerIndex];
                            //before we overwrite the value on cache we check if it needs to be saved onto the stack
                            if ((replacePartner << 31) == -2147483648)
                            {
                                int data = ((replacePartner & 16711680) >> 16);
                                stack[(replacePartner >> 24)] = data;
                            }
                            Debug.WriteLine("Spatial value is: " + (((stackIndex - 1) << 24) | (stack[stackIndex - 1] << 16) | 2));
                            cache[replacePartnerIndex] = ((stackIndex - 1) << 24) | (stack[stackIndex - 1] << 16) | 2;// or'd with 2 to give it a spatial flag
                        }
                        Debug.WriteLine("Performed a spatial load");
                    }

                    int replaceIndex = (stackIndex % (cacheSize / 2)) * 2 + offset;
                    Debug.WriteLine("2 way miss, randomly selected cache[" + replaceIndex + "] to replace. Offset is " + offset);
                    int replaceValue = cache[replaceIndex];
                    int dirty = replaceValue << 31;
                    if (dirty == -2147483648)
                    {
                        int data = ((cache[replaceIndex] & 16711680) >> 16);
                        stack[replaceValue>>24] = data;
                        Debug.WriteLine("Replaced stack[" + (replaceValue>>24) + "] with the value " + data);
                    }
                    readMissCounter++;
                    hitOrMiss = "Miss";
                    //The main read from stack and store on cache call
                    cache[replaceIndex] = (stackIndex << 24) | (stack[stackIndex] << 16);
                    return((cache[replaceIndex] & 16711680) >> 16); // using a mask to extract the data bits
                }

            }
            //return stack[0];
        }

        //////////////////////////////////////////////////
        //New to Project 2, method which handles writes//
        /// /////////////////////////////////////////////
        
        void writeStackValue(int stackIndex, int value){
            if(!addressMode)
            {//1 way
                int temp = cache[stackIndex % cacheSize];
                int tag = temp >> 24;
                Debug.WriteLine("Temp is " + temp + " and tag is " + tag);
                if (tag == stackIndex)
                {//hit
                    
                    cache[stackIndex % cacheSize] = (stackIndex << 24) | (value << 16) | 1;
                    int data = ( (cache[stackIndex % cacheSize] & 16711680) >> 16);
                    Debug.WriteLine("Write Hit: writing to item at stack[" + stackIndex + "] and it was in cache, and now its marked dirty");
                    ++writeHitCounter;
                    Debug.WriteLine("write hit counter is: " + writeHitCounter);
                    Debug.WriteLine("Value at cache[" + (stackIndex % cacheSize) + "] is " + temp );
                    hitOrMiss = "Hit";
                    Debug.WriteLine("Value at cache index[" + (stackIndex % cacheSize) + "] is " + data);
                    if ((temp & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }
                }
                else
                {//miss
                    Debug.WriteLine("Write Miss: writing to stack[" + stackIndex + "] and stack[" + tag + "] was in cache instead.");
                    ++writeMissCounter;
                    Debug.WriteLine("writemiss counter is: " + writeMissCounter);
                    hitOrMiss = "Miss";
                    stack[stackIndex] = value;
                    Debug.WriteLine("Wrote " + value + " to stack[" + stackIndex + "]");

                }
            }
            else
            {//2 way
                int way0 = cache[(stackIndex % (cacheSize/2))*2]; // cache index = stackindex % num sets
                int way1 = cache[((stackIndex) % ((cacheSize/2)))*2+1];
                Debug.WriteLine("Stack index is " + stackIndex +
                    " and way 0 index is " + ((stackIndex % (cacheSize/2))*2) +
                    " and way 1 index is " + ((stackIndex % (cacheSize/2))*2+1));

                if ((way0 >> 24) == stackIndex)
                {//hit on way 0
                    cache[(stackIndex % (cacheSize/2))*2] = (stackIndex << 24) | (value << 16) | 1;
                    int data = ((cache[(stackIndex % (cacheSize/2))*2] & 16711680) >> 16);
                    Debug.WriteLine("Write Hit: writing to item at stack[" + stackIndex + "] and it was in cache, and now its marked dirty");
                    ++writeHitCounter;
                    Debug.WriteLine("write hit counter is: " + writeHitCounter);
                    Debug.WriteLine("Way 0 write");
                    hitOrMiss = "Hit";
                    Debug.WriteLine("Value at cache index[" + ((stackIndex % (cacheSize/2))*2) + "] is " + data);
                    if ((way0 & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }

                }
                else if ((way1 >> 24) == stackIndex)
                {//hit on way 1
                    cache[(stackIndex % (cacheSize / 2))*2+1] = (stackIndex << 24) | (value << 16) | 1;
                    int data = (cache[((stackIndex % (cacheSize / 2))*2+1)] & 16711680) >> 16;
                    Debug.WriteLine("Write Hit: writing to item at stack[" + stackIndex + "] and it was in cache, and now its marked dirty");
                    ++writeHitCounter;
                    Debug.WriteLine("write hit counter is: " + writeHitCounter);
                    Debug.WriteLine("Way 1 write");
                    hitOrMiss = "Hit";
                    Debug.WriteLine("Value at cache index[" + ((stackIndex % (cacheSize / 2))*2+1) + "] is " + data);
                    if ((way1 & 2) == 2)
                    {//this hit was a result of spatial help
                        Debug.WriteLine("Spatial help: " + spatialCounter + 1);
                        spatialCounter++;
                    }

                }
                else
                {//miss, was at neither way 0/1
                    
                    stack[stackIndex] = value;
                    Debug.WriteLine("2-way Miss: put " + value + " into stack[" + stackIndex + "]");
                    hitOrMiss = "Miss";
                    writeMissCounter++;
                    
                }
            }     
        }

        public Memory(int cacheSize, int blockSize, bool addressMode)
        {
            binaryInstructions = new List<short>(10); // Default instruction size is 10
            assemblyInstructions = new List<string>(10); // Default instruction size 10
            stack = new int[256]; // Default memory size is 256
            cache = new Cache(cacheSize);
            copyCache = new string[cacheSize];
            initializeCopyCache();
            this.cacheSize = cacheSize;
            this.addressMode = addressMode;
            this.blockSize = blockSize;
            readHitCounter = 0;
            readMissCounter = 0;
            writeHitCounter = 0;
            writeMissCounter = 0;
            spatialCounter = 0;
            Debug.WriteLine("Just made a new memory object with stack of size 256 and cache of size " + cacheSize +
                " with address mode:" + (addressMode ? "2-way" : "1-way") + " and block size of " + blockSize);
        }


       public void clearMemory()
        {
            stack = new int[256];
            cache = new Cache(cacheSize);
            readMissCounter = 0;
            readHitCounter = 0;
            writeHitCounter = 0;
            writeMissCounter = 0;
            spatialCounter = 0;
        }

        public void initializeCopyCache()
       {
           for (int i = 0; i < cacheSize; i++)
           {
               copyCache[i] = "unvisited";
           }
       }

       /////////////////////////////////////////////////////
       //New to Project 2, method which handles cache tags//
       /////////////////////////////////////////////////////
        public int getTagAtCacheIndex(int index){
            int temp = cache[index % cacheSize];
            Debug.Write("Full value at cache[" + index + "] is " + temp);
            Debug.Write(". The related stack index is " + (temp >> 24) + "\n");
            return temp >> 24;

        }

        ////////////////////////////////////////////////////////////
        //New to Project 2, method which overrides offset operator//
        ////////////////////////////////////////////////////////////
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
