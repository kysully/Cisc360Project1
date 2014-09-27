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
        public short ACC { get; private set; }
        public short A { get; private set; }
        public short B { get; private set; }
        public short PC { get; private set; }
        public readonly short ONE = 1;
        public readonly short ZERO = 0;
        public CPU()
        {
            ACC = 0;
        }

        public void nextInstruction()
        {
            ACC++;
        }

        public static bool isFlag(string f)
        {
            if (f == "1")
                return true;
            else 
                return false;
        }

        public static void executeBinary(short binInstruction)
        {
            string binaryString = Convert.ToString(binInstruction, 2).PadLeft(16, '0');

            String opcode = binaryString.Substring(0, 3);
            String command = binaryString.Substring(3, 4);
            String flag = binaryString.Substring(7, 1);
            String value = binaryString.Substring(8, 8); 

            Debug.WriteLine("AGERNGGKDNBKS ---->" + binaryString + " O " + opcode + " C " + command + " F " + flag + " V " + value);

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
                    }
                    break;
                case "001":// ------------GROUP2
                    if(command == "0001"){ //LDA
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("LDA# has been reached");
                        }
                        else{
                            //$
                            Debug.WriteLine("LDA$ has been reached");
                        }
                    }
                    if(command == "0010"){ //STA
                        Debug.WriteLine("STA has been reached");
                    }
                    break;
                case "010":// --------------GROUP3
                    if(command == "0001"){ //ADD
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("ADD# has been reached");
                        }
                        else{
                            //$
                            Debug.WriteLine("ADD$ has been reached");
                        }
                    }
                    if(command == "0010"){ // SUB
                      if(flag == "1"){
                            //#
                          Debug.WriteLine("SUB# has been reached");
                        }
                        else{
                            //$
                          Debug.WriteLine("SUB$ has been reached");
                        }
                    }
                    if(command == "0011"){//MUL
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("MUL# has been reached");
                        }
                        else{
                            //$
                            Debug.WriteLine("MUL$ has been reached");
                        }
                    }
                    if(command == "0100"){//DIV
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("DIV# has been reached");

                        }
                        else{
                            //$
                            Debug.WriteLine("DIV$ has been reached");

                        }
                    }
                    if(command == "0101"){//SHL
                      //SHL things
                        Debug.WriteLine("SHL has been reached");

                    }
                    break;
                case "011": //-----------------GROUP4
                    if(command == "0001"){//AND
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("AND# has been reached");
                        }
                        else{
                            //$
                            Debug.WriteLine("AND$ has been reached");
                        }
                    }
                    if(command == "0010"){//OR
                        if(flag == "1"){
                            //#
                            Debug.WriteLine("OR# has been reached");
                        }
                        else{
                            //$
                            Debug.WriteLine("OR$ has been reached");
                        }
                    }
                    if(command == "0011"){//NOTA
                      //NOTA things
                        Debug.WriteLine("NOTA# has been reached");
                    }
                    break;
                case "100": // ------------------GROUP5
                    if (command == "0001"){//BA
                        Debug.WriteLine("BA has been reached");
                    } 
                    if(command == "0010"){//BE
                        Debug.WriteLine("BE has been reached");
                    } 
                    if(command == "0011"){//BL
                        Debug.WriteLine("BL has been reached");
                    }
                    if (command == "0100"){//BG
                        Debug.WriteLine("BG has been reached");
                    }
                    break;
            }

            return;
        }
    }
}
