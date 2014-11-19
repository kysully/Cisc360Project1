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

        //////////////////////
        ///New to Project 3///
        //////////////////////
        public short IR_D { get; set; }//short since our instructions are shorts
        private Queue<FetchedInstruction> fetched_instructions;
        private Queue<DecodedInstruction> decoded_instructions;
        private Queue<ExecutedInstruction> executed_instructions;
        private int Fetch_Counter { get; set; }
        private int Decode_Counter { get; set; }
        private int Execute_Counter { get; set; }
        private int Store_Counter { get; set; }
        public int cycles_elapsed { get; private set; }
        public int cycle_penalties { get; private set; }
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
        public delegate void BranchTaken(object sender, BranchEventArgs args);
        public event BranchTaken OnBranchTaken;
        public String currBranchInstr = "";

        public bool bypassing { get; set; }
        bool areWeDone = false;
        bool tookBranch = false;

        Barrier stageBarrier = new Barrier(participantCount:4);

       

        //////////////////////////
        ///End new to project 3///
        //////////////////////////

        public CPU(Memory memory)
        {
            ACC = 0;
            PC = 0;
            TEMP = 0;
            cycles_elapsed = 0;
            cycle_penalties = 0;
            Fetch_Counter = 0;
            Decode_Counter = 0;
            Execute_Counter = 0;
            Store_Counter = 0;
            //We store a reference to main memory so the CPU can interact with it
            //essentially simulating the CPU making calls to memory.
            this.memory = memory;
            
            ///Initlialize all the new threads for pipelining
            IR_D = 0;//default to NOP
            bypassing = false;
            fetched_instructions = new Queue<FetchedInstruction>();
            decoded_instructions = new Queue<DecodedInstruction>();
            executed_instructions = new Queue<ExecutedInstruction>();

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


        public class FetchedInstruction
        {
            public short binary { get; private set; }
            public int index { get; private set; }
            public FetchedInstruction(short binary, int index)
            {
                this.binary = binary;
                this.index = index;
            }
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
            public int index { get; private set; }
            public DecodedInstruction(short binary, int index){
                //Decodes the binary
                String binaryString = Convert.ToString(binary, 2).PadLeft(16, '0');
                this.binary = binary;
                opcode = binaryString.Substring(0, 3);
                command = binaryString.Substring(3, 4);
                flag = binaryString.Substring(7, 1);
                String valueRaw = binaryString.Substring(8, 8);
                value = Convert.ToInt16(valueRaw, 2);
                this.index = index;
            }
        }

        public enum StoreType
        {
            Accumulator,
            Memory,
            None
        }

        //In execute, the math is done and the result is stored in this wrapper
        //in addition to how that result will be stored, either in ACC or memory
        public class ExecutedInstruction
        {
            public int result { get; private set; }
            public int memoryIndex { get; private set; }
            public int instrIndex { get; private set; }
            public StoreType type { get; private set; }
            public ExecutedInstruction(int result, StoreType type, int instrIndex)
            {
                this.result = result;
                this.type = type;
                this.instrIndex = instrIndex;
                this.memoryIndex = -1;
            }
            public ExecutedInstruction(int result, StoreType type, int instrIndex, int memoryIndex)
            {
                this.result = result;
                this.type = type;
                this.instrIndex = instrIndex;
                this.memoryIndex = memoryIndex;
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
                //I think this could go here...
                if (tookBranch)
                {
                    //to clear out the instructions that were being worked on before
                    tookBranch = false;//reset tookbranch
                    fetched_instructions.Clear();
                    decoded_instructions.Clear();
                    executed_instructions.Clear();

                }
                fetchEvent.Set();

                decodeEvent.Set();

                executeEvent.Set();

                storeEvent.Set();

                cycles_elapsed++;
                Console.WriteLine("Cycles elapsed: " + cycles_elapsed);
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
                    var FI = new FetchedInstruction(instruction, Fetch_Counter);
                    fetched_instructions.Enqueue(FI);
  
                    this.IR = instruction;
                    Console.WriteLine("IR is " + this.IR);

                    if (OnFetchDone != null)
                    {
                        OnFetchDone(this, new FetchEventArgs(FI));
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
                    FetchedInstruction instr = fetched_instructions.Dequeue();
                    
                    Console.WriteLine("Decode counter is: " + Decode_Counter);
                    DecodedInstruction decodedInstr = new DecodedInstruction(instr.binary, instr.index);
                    decoded_instructions.Enqueue(decodedInstr);
                    Decode_IR = decodedInstr;
                    //If Branch, then add to table.
                    if (decodedInstr.opcode == "100")
                    {
                        currBranchInstr = Memory.getAssemblyInstructions().ElementAt(decodedInstr.index);
                    }
                    else
                    {
                        currBranchInstr = "";

                    }

                    Console.WriteLine("Just decoded: " + decodedInstr.binary);

                    if (OnDecodeDone != null)
                    {
                        OnDecodeDone(this, new DecodeEventArgs(decodedInstr));
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
                    while (waitingForStoreResult())
                    {
                        Console.WriteLine("Waiting for a result to store into ACC/memory...");
                    }
                    executeInstruction(instr);
                    Debug.WriteLine("Just executed: " + instr.binary);
                    //this is done in execute instructionexecuted_instructions.Enqueue(instr.binary);

                    if (OnExecuteDone != null)
                    {
                        OnExecuteDone(this, new ExecuteEventArgs(instr));
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
                if (executed_instructions.Count > 0)
                {
                    ExecutedInstruction instrResult = executed_instructions.Dequeue();
                    if (instrResult.type == StoreType.Accumulator)
                    {
                        ACC = instrResult.result;
                    }
                    else if(instrResult.type == StoreType.Memory)
                    {
                        memory[instrResult.memoryIndex] = instrResult.result;
                        Console.WriteLine("Stored " + instrResult.result + " into memory index " + instrResult.memoryIndex);
                    }
                    //Console.WriteLine("Performed store on: " + binary);
                    if (OnStoreDone != null)
                    {
                        OnStoreDone(this, new StoreEventArgs(instrResult.instrIndex));
                    }
                    Store_Counter++;
                    PC++;
                    Console.WriteLine("PC is: " + PC + " and Fetch counter is: " + Fetch_Counter);                    
                }
                else if (Fetch_Counter == Memory.getBinaryInstructions().Count())
                {
                        //this means we just stored the final instruction, so increment PC to trip the end of the program
                        PC++;
                }
            }
        }

        public bool waitingForStoreResult()
        {
            foreach (var instr in executed_instructions)
            {
                if (instr.type != StoreType.None)
                {
                    return true;
                }
            }
            return false;
        }

        //////////////////////////////////
        ///End new methods to Project 3///
        //////////////////////////////////

        
        //Method which resets CPU to default state
        public void reset()
        {
            ACC = 0;
            PC = 0;
            Fetch_Counter = 0;
            Decode_Counter = 0;
            Execute_Counter = 0;
            Store_Counter = 0;
            fetched_instructions.Clear();
            decoded_instructions.Clear();
            executed_instructions.Clear();
            cycles_elapsed = 0;
            cycle_penalties = 0;
            bypassing = false;
            tookBranch = false;
            areWeDone = false;
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

            //PC++;
            string binaryString = Convert.ToString(instr.binary, 2).PadLeft(16, '0');
            String opcode = instr.opcode;
            String command = instr.command;
            String flag = instr.flag;
            short value = instr.value;
            int instrIndex = instr.index;
            //Defaults to none for things like NOP and branches
            ExecutedInstruction executedInstr = new ExecutedInstruction(0, StoreType.None, instrIndex);           

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
                        //Maybe signal to the GUI here that there was a load-use delay
                        if (!bypassing)
                        {
                            //penalty for a load-use delay is 1 cycle
                            cycle_penalties++;
                        }
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("LDA# has been reached");
                            //ACC = value;
                            executedInstr = new ExecutedInstruction(value, StoreType.Accumulator, instrIndex);
                        }
                        else{
                            //$
                            Debug.WriteLine("LDA$ has been reached");
                            //ACC = Memory.stack[value];
                            //ACC = memory[value];
                            executedInstr = new ExecutedInstruction(memory[value], StoreType.Accumulator, instrIndex);
                        }
                    }
                    if(command == "0010"){ //STA
                        Debug.WriteLine("STA has been reached");
                        //Memory.stack[value] = ACC;
                        //memory[value] = ACC;
                        executedInstr = new ExecutedInstruction(ACC, StoreType.Memory, instrIndex, value);
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
                            //ACC += value;
                            executedInstr = new ExecutedInstruction(ACC + value, StoreType.Accumulator, instrIndex);
                            Debug.Write(" ACC is " + ACC);
                        }
                        else{
                            //$
                            Debug.WriteLine("ADD$ has been reached");
                            //ACC += Memory.stack[value];
                            //ACC += memory[value];
                            executedInstr = new ExecutedInstruction(ACC + memory[value], StoreType.Accumulator, instrIndex);
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
                          //ACC -= value;
                          executedInstr = new ExecutedInstruction(ACC - value, StoreType.Accumulator, instrIndex);
                          Debug.WriteLine("ACC is now " + ACC);
                        }
                        else{
                           //$
                            //int temp = Memory.stack[value];
                            //int temp1 = memory[value];
                            Debug.WriteLine("SUB$ has been reached");
                            //Debug.WriteLine("ACC is " + ACC + " and value is " + temp1);
                            //ACC -= temp;
                            executedInstr = new ExecutedInstruction(ACC - memory[value], StoreType.Accumulator, instrIndex);
                            Debug.WriteLine("ACC is now " + ACC);
                        }
                    }
                    if(command == "0011"){//MUL
                        //maybe signal to the GUI that there was a hazard
                        cycle_penalties = cycle_penalties + 4;
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("MUL# has been reached");
                            //ACC = ACC * value;
                            executedInstr = new ExecutedInstruction(ACC * value, StoreType.Accumulator, instrIndex);
                        }
                        else{
                            //$
                            Debug.WriteLine("MUL$ has been reached");
                            //ACC = ACC * Memory.stack[value];
                            //ACC = ACC * memory[value];
                            executedInstr = new ExecutedInstruction(ACC * memory[value], StoreType.Accumulator, instrIndex);
                        }
                    }
                    if(command == "0100"){//DIV
                        //maybe signal to the GUI that there was a hazard
                        cycle_penalties = cycle_penalties + 4;
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("DIV# has been reached");
                            //ACC = ACC / value;
                            executedInstr = new ExecutedInstruction(ACC / value, StoreType.Accumulator, instrIndex);
                        }
                        else{
                            //$
                            Debug.WriteLine("DIV$ has been reached");
                            //ACC = ACC / Memory.stack[value];
                            //ACC = ACC / memory[value];
                            executedInstr = new ExecutedInstruction(ACC / memory[value], StoreType.Accumulator, instrIndex);

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
                            //ACC = ACC & value;
                            executedInstr = new ExecutedInstruction(ACC & value, StoreType.Accumulator, instrIndex);
                        }
                        else{
                            //$
                            //int temp = Memory.stack[value];
                            //int temp = memory[value];
                            Debug.WriteLine("AND$ has been reached");
                            //Debug.WriteLine("ACC is " + ACC + " and value is " + temp);
                            //ACC = ACC & temp;
                            executedInstr = new ExecutedInstruction(ACC & memory[value], StoreType.Accumulator, instrIndex);
                            Debug.WriteLine("ACC is now " + ACC);
                        }
                    }
                    if(command == "0010"){//OR
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("OR# has been reached");
                            //ACC = ACC | (ushort)value; // Do we need to cast here? perhaps
                            executedInstr = new ExecutedInstruction(ACC | (ushort)value, StoreType.Accumulator, instrIndex);
                        }
                        else{
                            //$
                            Debug.WriteLine("OR$ has been reached");
                            //ACC = ACC | Memory.stack[value];
                            //ACC = ACC | memory[value];
                            executedInstr = new ExecutedInstruction(ACC | memory[value], StoreType.Accumulator, instrIndex);
                        }
                    }
                    if(command == "0011"){//NOTA
                      //NOTA things
                        Debug.WriteLine("NOTA# has been reached");
                        Debug.WriteLine("!" + ACC + " is " + ~ACC);
                        //ACC = ~ACC; // I think ~ is a bitwise not
                        executedInstr = new ExecutedInstruction(~ACC, StoreType.Accumulator, instrIndex);
                    }
                    break;
                case "100": // ------------------GROUP5
                    bool tookBranch = false;
                    if (command == "0001"){//BA
                        Debug.WriteLine("BA has been reached");
                        
                        tookBranch = true;
                    } 
                    if(command == "0010"){//BE
                        Debug.WriteLine("BE has been reached");
                        if (CC == 0)
                        {
                            
                            tookBranch = true;
                        }
                    } 
                    if(command == "0011"){//BL
                        Debug.WriteLine("BL has been reached");
                        if (CC < 0)
                        {
                            
                            tookBranch = true;
                        }
                    }
                    if (command == "0100"){//BG
                        Debug.WriteLine("BG has been reached");
                        if (CC > 0)
                        {
                            
                            tookBranch = true;
                        }
                    }

                    if (tookBranch)
                    {
                        //Normal branching code
                        PC = (short)(value-1);//think it was -1 due to the PC incrementing after// PC = (short)(value - 1);
                        Fetch_Counter = PC;
                        Decode_Counter = -1;//PC
                        Execute_Counter = -1;//PC
                        //penalty for a taken branch is 1 cycle
                        cycle_penalties++;
                        //gotta call something here to flush out pipeline queue in GUI
                        //moved to execute thread
                        if (OnBranchTaken != null)
                        {
                            OnBranchTaken(this, new BranchEventArgs(instr, instrIndex));
                            //OnFetchDone(this, new FetchEventArgs(0, -1));
                            //OnDecodeDone(this, new DecodeEventArgs(null, -1));
                            //OnExecuteDone(this, new ExecuteEventArgs(0, -1));

                        }
                    }
                    break;
            }

            //This is to avoid the next instruction being a branch and depending on the
            //cc to be updated from the previous instruction when it goes to store
            if (executedInstr.type == StoreType.Accumulator)
            {
                if (executedInstr.result > 0)
                {
                    CC = 1;
                }
                else if (executedInstr.result < 0)
                {
                    CC = -1;
                }
                else CC = 0;
            }
            //Passes the result of execute to the store stage, to be stored next cycle
            executed_instructions.Enqueue(executedInstr);

            return;
        }
    }
}
