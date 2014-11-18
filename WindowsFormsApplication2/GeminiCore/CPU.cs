/**
 * Kyle Sullivan and Melody Lugo
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace GeminiCore
{
    public class CPU : IDisposable
    {
        public int ACC { get; private set; }
        public short PC { get; private set; }
        public short IR { get; private set; }
        public int TEMP { get; private set; }
        public short CC { get; private set; }
        public readonly short ONE = 1;
        public readonly short ZERO = 0;
        public Memory memory;

        /////////////////////////////////////////////////////////////////
        ///New to Project 3, threads for the four stages of pipelining///
        /////////////////////////////////////////////////////////////////
        public short IR_D { get; set; }//short since our instructions are shorts
        private int Fetch_Counter { get; set; }
        private Queue<short> fetched_instructions;
        private Queue<DecodedInstruction> decoded_instructions;
        private int Decode_Counter { get; set; }
        private int Execute_Counter { get; set; }
        private int Store_Counter { get; set; }
        private DecodedInstruction Decode_IR { get; set; }

        Thread fetchThread;
        AutoResetEvent fetchEvent = new AutoResetEvent(false);
        Thread decodeThread;
        AutoResetEvent decodeEvent = new AutoResetEvent(false);
        Thread executeThread;
        AutoResetEvent executeEvent = new AutoResetEvent(false);
        Thread storeThread;
        AutoResetEvent storeEvent = new AutoResetEvent(false);

        public delegate void FetchDone(object sender, FetchEventArgs args);
        public event FetchDone OnFetchDone;
        public delegate void DecodeDone(object sender, DecodeEventArgs args);
        public event DecodeDone OnDecodeDone;
        public delegate void ExecuteDone(object sender, ExecuteEventArgs args);
        public event ExecuteDone OnExecuteDone;
        public delegate void StoreDone(object sender, StoreEventArgs args);
        public event StoreDone OnStoreDone;

        bool areWeDone = false;

        public CPU(Memory memory)
        {
            ACC = 0;
            PC = 0;
            TEMP = 0;
            Fetch_Counter = 0;
            Decode_Counter = 0;
            Execute_Counter = 0;
            Store_Counter = 0;
            //We store a reference to main memory so the CPU can interact with it
            //essentially simulating the CPU making calls to memory.
            this.memory = memory;
            
            ///Initlialize all the new threads for pipelining
            IR_D = 0;//default to NOP
            fetched_instructions = new Queue<short>();
            decoded_instructions = new Queue<DecodedInstruction>();

            fetchThread = new Thread(new ThreadStart(PerformFetch));
            fetchThread.Name = "Fetch Thread";
            fetchThread.Start();

            decodeThread = new Thread(new ThreadStart(PerformDecode));
            decodeThread.Name = "Decode Thread";
            decodeThread.Start();

            executeThread = new Thread(new ThreadStart(PerformExecute));
            executeThread.Name = "Execute Thread";
            executeThread.Start();

            storeThread = new Thread(new ThreadStart(PerformStore));
            storeThread.Name = "Store Thread";
            storeThread.Start();
        }

        //Takes a 16 bit binary instruction and decodes it into
        // opcode, command, flag, and value
        public class DecodedInstruction
        {
            public String opcode { get; private set; }
            public String command { get; private set; }
            public String flag { get; private set; }
            public String valueString { get; private set; }
            public short value { get; private set; }
            public short binary { get; private set; }
            public DecodedInstruction(short binary){
                //Decodes the binary
                String binaryString = Convert.ToString(binary, 2).PadLeft(16, '0');
                this.binary = binary;
                opcode = binaryString.Substring(0, 3);
                command = binaryString.Substring(3, 4);
                flag = binaryString.Substring(7, 1);
                String valueRaw = binaryString.Substring(8, 8);
                value = Convert.ToInt16(valueRaw, 2);
            }
        }

        /////////////////////////////////////////////////////////////////
        ///New to Project 3, methods for the four stages of pipelining///
        /////////////////////////////////////////////////////////////////

        //This should probably be called after the last instruction is completed
        //aka when we have to reset the CPU
        public void Dispose()
        {
            areWeDone = true;
            fetchEvent.Set();
            fetchThread.Join();

            decodeEvent.Set();
            decodeThread.Join();

            executeEvent.Set();
            executeThread.Join();

            storeEvent.Set();
            storeThread.Join();
        }

        public void nextInstructionPipeline()
        {
            if (PC < Memory.getBinaryInstructions().Count)
            {
                fetchEvent.Set();

                decodeEvent.Set();

                executeEvent.Set();

                storeEvent.Set();
                //PC++; // do we need to do this here?
            }
        }

        private void PerformFetch()
        {
            while (!areWeDone)
            {
                fetchEvent.WaitOne();
                Console.WriteLine("In Fetch");
                //fetch the instruction here
                if (Fetch_Counter < Memory.getBinaryInstructions().Count)
                {
                    short instruction = Memory.getBinaryInstructions().ElementAt(Fetch_Counter);
                    fetched_instructions.Enqueue(instruction);
  
                    this.IR = instruction;
                    Console.WriteLine("IR is " + this.IR);

                    if (OnFetchDone != null)
                    {
                        OnFetchDone(this, new FetchEventArgs(this.IR, Fetch_Counter));
                    }
                    Fetch_Counter++;
                }
            }
        }

        public void PerformDecode()
        {
            while (!areWeDone)
            {
                decodeEvent.WaitOne();
                Console.WriteLine("In Decode");
                if (fetched_instructions.Count > 0)
                {
                    short instr = fetched_instructions.Dequeue();
                    
                    Console.WriteLine("Decode counter is: " + Decode_Counter);
                    DecodedInstruction decodedInstr = new DecodedInstruction(instr);
                    decoded_instructions.Enqueue(decodedInstr);
                    Decode_IR = decodedInstr;
                    Console.WriteLine("Just decoded: " + decodedInstr.binary);

                    if (OnDecodeDone != null)
                    {
                        OnDecodeDone(this, new DecodeEventArgs(decodedInstr, Decode_Counter));
                    }
                    Decode_Counter++;
                }
                
                
            }
        }
        public void PerformExecute()
        {
            while (!areWeDone)
            {
                executeEvent.WaitOne();
                Console.WriteLine("In Execute");
                if (decoded_instructions.Count > 0)
                {
                    DecodedInstruction instr = decoded_instructions.Dequeue();
                    
                    executeInstruction(instr);
                    Debug.WriteLine("Just executed: " + instr.binary);

                    if (OnExecuteDone != null)
                    {
                        OnExecuteDone(this, new ExecuteEventArgs(instr.binary, Execute_Counter));
                    }
                    Execute_Counter++;
                }                
            }
        }
        public void PerformStore()
        {
            while (!areWeDone)
            {
                storeEvent.WaitOne();
                Console.WriteLine("In Store");
                if (OnStoreDone != null)
                {
                    OnStoreDone(this, new StoreEventArgs(Store_Counter));
                }
                Store_Counter++;
            }
        }

        //end new methods for Project 3

        
        //Method which resets CPU to default state
        public void reset()
        {
            ACC = 0;
            PC = 0;
            Fetch_Counter = 0;
            Decode_Counter = 0;
            Execute_Counter = 0;
            Store_Counter = 0;
            TEMP = 0;
            CC = 0;
            Memory.clearInstructions();
            memory.clearMemory();
        }

        //When PC == instruction count, that means you executed all instructions
        public void nextInstruction()
        {
            
            if (PC < Memory.getBinaryInstructions().Count)
            {
                short instruction = Memory.getBinaryInstructions().ElementAt(PC);
                //executeBinary(instruction);
                PC++;
            }
        }

        public static bool isFlag(string f)
        {
            if (f == "1")
                return true;
            else 
                return false;
        }

        public void executeInstruction(DecodedInstruction instr)
        {

            PC++;
            string binaryString = Convert.ToString(instr.binary, 2).PadLeft(16, '0');
            String opcode = instr.opcode;
            String command = instr.command;
            String flag = instr.flag;
            short value = instr.value;
           

            Console.WriteLine("Executing ---->" + binaryString + " O " + opcode + " C " + command + " F " + flag + " V " + value);

            switch (opcode)
            {
                case "000":// --------------GROUP1
                    if(command == "0000"){ // NOP
                        Debug.WriteLine("NOP has been reached");
                        return;
                    }
                    else{ // HLT
                        //DO HLT 
                        Debug.WriteLine("HLT has been reached");
                        return;
                    }
                    //break;
                case "001":// ------------GROUP2
                    if(command == "0001"){ //LDA
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("LDA# has been reached");
                            ACC = value;
                        }
                        else{
                            //$
                            Debug.WriteLine("LDA$ has been reached");
                            //ACC = Memory.stack[value];
                            ACC = memory[value];
                        }
                    }
                    if(command == "0010"){ //STA
                        Debug.WriteLine("STA has been reached");
                        //Memory.stack[value] = ACC;
                        memory[value] = ACC;
                        Debug.Write("Stored the value " + ACC + " into stack at index " + value);
                    }
                    else if(command == "1010"){
                        Debug.WriteLine("STA special has been reached");
                        TEMP = ACC;
                        Debug.WriteLine("TEMP = " + TEMP);
                    }
                    break;
                case "010":// --------------GROUP3
                    if(command == "0001"){ //ADD
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("ADD# has been reached");
                            Debug.Write("Value is " + value);
                            ACC += value;
                            Debug.Write(" ACC is " + ACC);
                        }
                        else{
                            //$
                            Debug.WriteLine("ADD$ has been reached");
                            //ACC += Memory.stack[value];
                            ACC += memory[value];
                        }
                        break;
                    }
                    if (command == "1001")//Special add
                    {
                        Debug.WriteLine("Special ADD has been reached");
                        ACC += TEMP;
                    }
                    if(command == "0010"){ // SUB
                      if(flag == "1"){
                            //#
                          Debug.WriteLine("SUB# has been reached");
                          Debug.WriteLine("ACC is " + ACC + " and value is " + value);
                          ACC -= value;
                          Debug.WriteLine("ACC is now " + ACC);
                        }
                        else{
                           //$
                            //int temp = Memory.stack[value];
                            int temp = memory[value];
                            Debug.WriteLine("SUB$ has been reached");
                            Debug.WriteLine("ACC is " + ACC + " and value is " + temp);
                            ACC -= temp;
                            Debug.WriteLine("ACC is now " + ACC);
                        }
                    }
                    if(command == "0011"){//MUL
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("MUL# has been reached");
                            ACC = ACC * value;
                        }
                        else{
                            //$
                            Debug.WriteLine("MUL$ has been reached");
                            //ACC = ACC * Memory.stack[value];
                            ACC = ACC * memory[value];
                        }
                    }
                    if(command == "0100"){//DIV
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("DIV# has been reached");
                            ACC = ACC / value;
                        }
                        else{
                            //$
                            Debug.WriteLine("DIV$ has been reached");
                            //ACC = ACC / Memory.stack[value];
                            ACC = ACC / memory[value];
                        }
                    }
                    if(command == "0101"){//SHL
                      //SHL things
                        Debug.WriteLine("SHL has been reached");
                        ACC = ACC << value;
                    }
                    break;
                case "011": //-----------------GROUP4
                    if(command == "0001"){//AND
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("AND# has been reached");
                            ACC = ACC & value;
                        }
                        else{
                            //$
                            //int temp = Memory.stack[value];
                            int temp = memory[value];
                            Debug.WriteLine("AND$ has been reached");
                            Debug.WriteLine("ACC is " + ACC + " and value is " + temp);
                            ACC = ACC & temp;
                            Debug.WriteLine("ACC is now " + ACC);
                        }
                    }
                    if(command == "0010"){//OR
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("OR# has been reached");
                            ACC = ACC | (ushort)value; // Do we need to cast here? perhaps
                        }
                        else{
                            //$
                            Debug.WriteLine("OR$ has been reached");
                            //ACC = ACC | Memory.stack[value];
                            ACC = ACC | memory[value];
                        }
                    }
                    if(command == "0011"){//NOTA
                      //NOTA things
                        Debug.WriteLine("NOTA# has been reached");
                        Debug.WriteLine("!" + ACC + " is " + ~ACC);
                        ACC = ~ACC; // I think ~ is a bitwise not
                    }
                    break;
                case "100": // ------------------GROUP5
                    if (command == "0001"){//BA
                        Debug.WriteLine("BA has been reached");
                        PC = (short)(value-1);
                        Fetch_Counter = PC;
                        Decode_Counter = PC;
                        Execute_Counter = PC;
                    } 
                    if(command == "0010"){//BE
                        Debug.WriteLine("BE has been reached");
                        if (CC == 0)
                        {
                            PC = (short)(value-1);
                            Fetch_Counter = PC;
                            Decode_Counter = PC;
                            Execute_Counter = PC;
                        }
                    } 
                    if(command == "0011"){//BL
                        Debug.WriteLine("BL has been reached");
                        if (CC < 0)
                        {
                            PC = (short)(value-1);
                            Fetch_Counter = PC;
                            Decode_Counter = PC;
                            Execute_Counter = PC;
                        }
                    }
                    if (command == "0100"){//BG
                        Debug.WriteLine("BG has been reached");
                        if (CC > 0)
                        {
                            PC = (short)(value-1);
                            Fetch_Counter = PC;
                            Decode_Counter = PC;
                            Execute_Counter = PC;
                        }
                    }
                    break;
            }

            if (ACC > 0)
            {
                CC = 1;
            }
            else if (ACC < 0)
            {
                CC = -1;
            }
            else CC = 0;

            return;
        }
    }
}
