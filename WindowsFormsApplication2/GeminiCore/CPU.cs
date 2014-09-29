/**
 * Seth Morecraft
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace GeminiCore
{
    public class CPU
    {
        public int ACC { get; private set; }
        public short A { get; private set; }
        public short B { get; private set; }
        public short PC { get; private set; }
        public int TEMP { get; private set; }
        public short CC { get; private set; }
        public readonly short ONE = 1;
        public readonly short ZERO = 0;
        public CPU()
        {
            ACC = 0;
            PC = 0;
            TEMP = 0;
        }

        
        //Method which resets CPU to default state
        public void reset()
        {
            ACC = 0;
            PC = 0;
            TEMP = 0;
            CC = 0;
            Memory.clearInstructions();
        }

        //When PC == instruction count, that means you executed all instructions
        public void nextInstruction()
        {
            
            if (PC < Memory.getBinaryInstructions().Count)
            {
                short instruction = Memory.getBinaryInstructions().ElementAt(PC);
                executeBinary(instruction);
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

        public void executeBinary(short binInstruction)
        {
            string binaryString = Convert.ToString(binInstruction, 2).PadLeft(16, '0');

            String opcode = binaryString.Substring(0, 3);
            String command = binaryString.Substring(3, 4);
            String flag = binaryString.Substring(7, 1);
            String valueRaw = binaryString.Substring(8, 8);
            short value = Convert.ToInt16(valueRaw, 2);

            Debug.WriteLine("Executing ---->" + binaryString + " O " + opcode + " C " + command + " F " + flag + " V " + value);

            switch (opcode)
            {
                case "000":// --------------GROUP1
                    if(command == "0000"){ // NOP
                        Debug.WriteLine("NOP has been reached");
                        return;
                    }
                    else{ // HTP
                        //DO HLT THINGS
                        Debug.WriteLine("HLT has been reached");
                        return;
                    }
                    break;
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
                            ACC = Memory.stack[value];
                        }
                    }
                    if(command == "0010"){ //STA
                        Debug.WriteLine("STA has been reached");
                        Memory.stack[value] = ACC;
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
                            //ACC += Convert.ToInt16(Convert.ToInt16(value, 2));
                            ACC += value;
                            Debug.Write(" ACC is " + ACC);
                        }
                        else{
                            //$
                            Debug.WriteLine("ADD$ has been reached");
                            ACC += Memory.stack[value];
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
                          ACC -= value;
                        }
                        else{
                            //$
                          Debug.WriteLine("SUB$ has been reached");
                          ACC -= Memory.stack[value];
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
                            ACC = ACC * Memory.stack[value];
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
                            ACC = ACC / Memory.stack[value];
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
                            Debug.WriteLine("AND$ has been reached");
                            ACC = ACC & Memory.stack[value];
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
                            ACC = ACC | Memory.stack[value];
                        }
                    }
                    if(command == "0011"){//NOTA
                      //NOTA things
                        Debug.WriteLine("NOTA# has been reached");
                        ACC = ~ACC; // I think ~ is a bitwise not
                    }
                    break;
                case "100": // ------------------GROUP5
                    if (command == "0001"){//BA
                        Debug.WriteLine("BA has been reached");
                        PC = value;
                    } 
                    if(command == "0010"){//BE
                        Debug.WriteLine("BE has been reached");
                        if (CC == 0)
                        {
                            PC = value;
                        }
                    } 
                    if(command == "0011"){//BL
                        Debug.WriteLine("BL has been reached");
                        if (CC < 0)
                        {
                            PC = value;
                        }
                    }
                    if (command == "0100"){//BG
                        Debug.WriteLine("BG has been reached");
                        if (CC > 0)
                        {
                            PC = value;
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
