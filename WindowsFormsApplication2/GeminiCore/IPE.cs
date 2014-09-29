/**
 * Kyle Sullivan and Melody Lugo
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;


namespace GeminiCore
{

    public class IPE
    {

        Dictionary<string, int> labels; // String = label, int = line number the label points to
        Dictionary<string, short> instructionCodes;

        public string FileToParse { get; set; }

        public IPE(string filename)
        {
            this.FileToParse = filename;
            labels = new Dictionary<String, int>();
            instructionCodes = initializeInstrCodes();
        }

        public Dictionary<string, short> initializeInstrCodes()
        {
            //Building a mapping for the assembly instructions to a binary representation
            Dictionary<string, short> temp = new Dictionary<string, short>(20);
            temp.Add("nop", 0);//Other
            temp.Add("hlt", 512);//Other
            temp.Add("lda", 8704);//Memory
            temp.Add("sta", 9216);//Memory
            temp.Add("add", 16896);//Math
            temp.Add("sub", 17408);//Math
            temp.Add("mul", 17920);//Math
            temp.Add("div", 18432);//Math 
            temp.Add("shl", 18944);//Math
            temp.Add("and", 25088);//Logic
            temp.Add("or", 25600);//Logic
            temp.Add("nota", 26112);//Logic
            temp.Add("ba", -32256);//Control
            temp.Add("be", -31744);//Control
            temp.Add("bl", -31232);//Control
            temp.Add("bg", -30720);//Control
            //Flags
            temp.Add("#", 256);
            temp.Add("$", 0);

            //Developer loop to see whats going on inside the dictionary
            foreach (var x in temp)
            {
                string value = (Convert.ToString(x.Value, 2)).PadLeft(16, '0');
                Debug.Write("Key is: " + x.Key + " and value is: " + value + "\n");
            }
            //End developer loop
            return temp;
        }

        public short[] AssemblytoBinary(List<string> assemblyLines)
        {
            //short[] binaryInstructions = new short[assemblyLines.Count];//cant use this cause loading can be 3 instructions
            List<short> binaryInstructions = new List<short>(assemblyLines.Count);
            int count = -1;
            string[] separators = {" ","   "};
            Memory.setAssemblyInstructions(assemblyLines);
            Dictionary<List<string>, int> newCommands = new Dictionary<List<string>, int>();
            foreach (var line in assemblyLines)
            {
                count++;
                //Elements is an array containing each segment of the instruction delimited by whitespace
                //example: line is lda #$5 ---> element[0] = "lda", element[1] = "#$5"
                Debug.Write("Line is " + line + "\n");
                //string[] elements = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string[] elements = line.Split(new char[0],StringSplitOptions.RemoveEmptyEntries);
               
                short opcode = 0, flag = 0, value = 0;
                short currLineBinary = 0;
                
                if (instructionCodes.ContainsKey(elements[0]))
                {
                    opcode = instructionCodes[elements[0]];
                    Debug.Write("Opcode is " + elements[0] + " and its value is " + opcode);
                }
                else
                {
                    //throw an error here cause we used an illegal opcode
                    Debug.Write("Error: Opcode " + elements[0] + " is invalid.");
                }
                if (elements.Length > 1 )
                {
                    
                    Debug.Write("Value in elements[1] is " + elements[1]);
                    Debug.Write(" Substring in element[1] is " + elements[1].Substring(0, 1));
                    string substring = elements[1].Substring(0, 1);
                    if (substring.Equals("#", StringComparison.OrdinalIgnoreCase)) //instructionsCodes.ContainsKey(substring);
                    {
                        flag = instructionCodes[substring];
                        Debug.Write(" elements 1 length is " + (elements[1].Length));
                        Debug.Write(" Flag is " + elements[1] + " and its value is " + flag);
                        var argumentString = elements[1].Substring(2, elements[1].Length-2);
                        Debug.Write(" Argument string is " + argumentString);
                        value = Convert.ToInt16(argumentString);
                        Debug.Write(" Argument string is " + argumentString);
                    }
                    else if (substring.Equals("$", StringComparison.OrdinalIgnoreCase))
                    {
                        var argumentString = elements[1].Substring(1, elements[1].Length-1);
                        value = Convert.ToInt16(argumentString);
                        Debug.Write("Argument string is " + argumentString);
                    }
                    else//branch case
                    {
                        value = (short)(labels[elements[1]]);
                        Debug.Write(" Label is " + elements[1] + " and its value is " + value);
                    }
                    
                }
                //Segmentation fault
                if ((value > 255) && (opcode == 9216))
                {
                    Debug.Write("Index out of bounds for stack.");
                    throw new Exception("Segmentation fault: index of " + value + " is out of bounds."); ;
                }
                //Special case - shift bits to load stuff
                else if ((value > 255) && (opcode == 8704) && (flag == 256))
                {
                    Debug.Write("Special shift case for load");
                    short instrPart1, shift, instrPart2;
                    string valueWhole, valuePart1, valuePart2;

                    valueWhole = Convert.ToString(value, 2).PadLeft(16, '0');
                    Debug.Write("Larger than normal load value: " + valueWhole);
                    valuePart1 = valueWhole.Substring(0, 8);
                    valuePart2 = valueWhole.Substring(8, 8);

                    instrPart1 = (short)(8960 | Convert.ToInt16(valuePart1,2));
                    shift = 19208;// (18944 | 256 | 8)
                    instrPart2 = (short)(25856 | Convert.ToInt16(valuePart2,2));
                    Debug.Write("Part 1 is " + valuePart1 + " and part 2 is " + valuePart2);

                    binaryInstructions.Add(instrPart1);
                    binaryInstructions.Add(shift);
                    binaryInstructions.Add(instrPart2);

                    Debug.WriteLine("Just added " + instrPart1 + ", " + shift + ", and " + instrPart2);
                    //1 instruction is becoming 3, so we need to adjust our labels
                    Dictionary<string, int> temp = new Dictionary<string, int>(labels.Count+10);
                    //var labelArray = labels.ToArray();
                    foreach(var label in labels)
                    {
                        if (label.Value > count)
                        {
                            temp.Add(label.Key, (label.Value + 2));
                        }
                        else
                        {
                            temp.Add(label.Key, label.Value);
                        }
                        Debug.WriteLine("Copied over " + temp.Last().Key + ", " + temp.Last().Value);
                    }
                    labels = temp;

                    //also have to update the assemblyInstructions
                    List<string> tempLines = new List<string>(3);
                    tempLines.Add("or #$" + Convert.ToInt16(valuePart2, 2));
                    tempLines.Add("shl #$" + 8);
                    tempLines.Add("lda #$" + Convert.ToInt16(valuePart1, 2));
                    newCommands.Add(tempLines, count);
                   
                   // instrPart1 = 8704 | 256 | Convert.ToInt16(valuePart1);
                    
                    continue;
                    

                }
                //Special case for adding numbers larger than 255
                else if ( (value > 255) && (opcode == 16896) && (flag == 256) )
                {
                    short staSpecial = 13312; //special store to temp register command
                    short addSpecial = 20992;//special add between temp and ACC
                    Debug.Write("Special shift case for add");
                    short instrPart1, shift, instrPart2;
                    string valueWhole, valuePart1, valuePart2;

                    valueWhole = Convert.ToString(value, 2).PadLeft(16, '0');
                    Debug.Write("Larger than normal load value: " + valueWhole);
                    valuePart1 = valueWhole.Substring(0, 8);
                    valuePart2 = valueWhole.Substring(8, 8);

                    instrPart1 = (short)(8960 | Convert.ToInt16(valuePart1, 2));
                    shift = 19208;// (18944 | 256 | 8)
                    instrPart2 = (short)(25856 | Convert.ToInt16(valuePart2, 2));
                    Debug.Write("Part 1 is " + valuePart1 + " and part 2 is " + valuePart2);

                    binaryInstructions.Add(staSpecial);
                    binaryInstructions.Add(instrPart1);
                    binaryInstructions.Add(shift);
                    binaryInstructions.Add(instrPart2);
                    binaryInstructions.Add(addSpecial);

                    Debug.WriteLine("Just added " + instrPart1 + ", " + shift + ", and " + instrPart2);
                    //1 instruction is becoming 3, so we need to adjust our labels
                    Dictionary<string, int> temp = new Dictionary<string, int>(labels.Count + 10);
                    //var labelArray = labels.ToArray();
                    foreach (var label in labels)
                    {
                        if (label.Value > count)
                        {
                            temp.Add(label.Key, (label.Value + 5));
                        }
                        else
                        {
                            temp.Add(label.Key, label.Value);
                        }
                        Debug.WriteLine("Copied over " + temp.Last().Key + ", " + temp.Last().Value);
                    }
                    labels = temp;

                    //also have to update the assemblyInstructions
                    List<string> tempLines = new List<string>(5);
                    tempLines.Add("add $TEMP");
                    tempLines.Add("or #$" + Convert.ToInt16(valuePart2, 2));
                    tempLines.Add("shl #$" + 8);
                    tempLines.Add("lda #$" + Convert.ToInt16(valuePart1, 2));
                    tempLines.Add("sta $TEMP");
                    newCommands.Add(tempLines, count);

                    // instrPart1 = 8704 | 256 | Convert.ToInt16(valuePart1);
                    
                    continue;

                }
                Debug.Write("Opcode is " + Convert.ToString(opcode, 2));
                Debug.Write(" Flag is " + flag);
                Debug.Write(" and value is " + value);
                currLineBinary = (short)((ushort)opcode | (ushort)flag | (ushort)value);
                Debug.Write("Instruction is " + line + " and binary is " + (Convert.ToString(currLineBinary, 2).PadLeft(16, '0')));
                Debug.Write("Command: ");
                foreach (var temp in elements)
                {
                    Debug.Write(temp + " " );
                    if (instructionCodes.ContainsKey(temp))
                    {
                        //STOPPED HERE - have to piece together the parts of the instruction first
                    }

                }
                Debug.Write("\n");

                var tempString = Convert.ToString(currLineBinary, 2).PadLeft(16, '0');
                Debug.Write("Temp string is " + tempString + " and currLineBinary is " + currLineBinary);// + " and temp short is " + tempShort);

                binaryInstructions.Add(currLineBinary);
                //binaryInstructions[count] = currLineBinary;
                Debug.Write("Added the short " + currLineBinary + " to the array of binary instructions.\n");
              
            }

            int addedCount = 0;
            foreach (var temp in newCommands)
            {
                int adjustedIndex = temp.Value + addedCount;
                Debug.Write("Removed: " + assemblyLines.ElementAt(adjustedIndex));
                assemblyLines.RemoveAt(temp.Value + addedCount);
                foreach (var item in temp.Key)
                {
                    assemblyLines.Insert(adjustedIndex, item);
                    addedCount++;
                }
                addedCount--;
                Debug.WriteLine(", added count is " + addedCount);
            }

            Debug.WriteLine("New assembly lines:");
            foreach (var temp in assemblyLines)
            {
                Debug.WriteLine(temp);
            }

            for (int i = 0; i < binaryInstructions.Count(); i++)
            {
                Debug.Write("\nLine " + i + ": " + binaryInstructions[i]);
            }
                return binaryInstructions.ToArray();
        }

        public void WriteBinarytoFile(short[] binaryInstructions, string fileName) {

            FileStream writeStream;
            try
            {
                writeStream = new FileStream(fileName, FileMode.Create);
                BinaryWriter writeBinary = new BinaryWriter(writeStream);

                for (int i = 0; i < binaryInstructions.Length; i++)
                {
                    writeBinary.Write(binaryInstructions[i]);
                }
                

                    writeBinary.Close();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        
        }

        public short[] readBinaryFromFile(string fileName)
        {
            BinaryReader binRead = new BinaryReader(File.Open(fileName, FileMode.Open));

            List<short> ans = new List<short>();

            while (binRead.PeekChar() != -1)
            {
                short temp = binRead.ReadInt16();
                ans.Add(temp);
                Debug.Write("\n" + temp);
            }

            binRead.Close();

            return ans.ToArray();
        }

        public List<string> ParseFile()
        {
            var linesRaw = File.ReadAllLines(this.FileToParse).ToArray<string>();
            var linesWithLabels = new List<string>();
            var lines = new List<string>();
            var assemblyLines = new List<string>();

            //Some code here to remove whitespace lines//
            Debug.WriteLine("Printing file without white space...");
            Regex emptyLineFormat = new Regex(@"^\s+$[\r\n]*");
            for (int i = 0; i < linesRaw.Length; i++)
            {
                var emptyLineMatch = emptyLineFormat.Match(linesRaw[i]);
                if (!string.IsNullOrWhiteSpace(linesRaw[i]))
                {
                    Debug.WriteLine(linesRaw[i]);
                    linesWithLabels.Add(linesRaw[i]);
                }
            }
            //Lines is now a list containing all the lines from the file without whitespace lines

            Debug.WriteLine("\nStarting label foreach loop\n");
            int labelCount = 0; // important to index labels to instructions
            int lineCount = 0; // which line we are on
            Regex labelStmtFormat = new Regex(@"^(?<label>.*?)\s*:$");
            var linesTemp = linesWithLabels.ToArray<string>();
            for (int i = 0; i < linesTemp.Length; i++)
            {
                var labelStmtMatch = labelStmtFormat.Match(linesTemp[i]);
                if (labelStmtMatch.Success)
                {
                    var label = labelStmtMatch.Groups["label"].Value;
                    
                    labels.Add(label, i - labelCount);//subtract because "label" will be removed from the final instruction list
                    Debug.WriteLine("Inserted label named " + label + " which points to instruction at line " + (i-labelCount));
                    Debug.WriteLine("Line is currently " + i + " and label count is currently " + labelCount);
                    labelCount++;
                }
                else
                {
                    lines.Add(linesTemp[i]);//Add non-label lines to our clean list of lines
                }
                lineCount++;
            }

            /*
            int labelCount = 0; // important to index labels to instructions
            int lineCount = 0; // which line we are on
            foreach (var line in lines)
            {
                Regex labelStmtFormat = new Regex(@"^(?<label>.*?)\s*:$");
                var labelStmtMatch = labelStmtFormat.Match(line);
                if (labelStmtMatch.Success)
                {
                   var label = labelStmtMatch.Groups["label"].Value;
                   Debug.WriteLine(label);
                   labels.Add(label, lineCount - labelCount);//subtract because "label" will be removed from the final instruction list
                   labelCount++;
                }
                lineCount++;
            }
             */
            //Remove all the labels we found from lines

            Debug.WriteLine(labels);
            Debug.WriteLine("Parsing out instructions:\n");
            try
            {
                foreach (var line in lines)
                {
                    //Regex instructionCommentStmtFormat = new Regex(@"^(?<instructionWithComment>.*?)\s*!");//grabs any text before a ! (comment)
                    Regex instructionCommentStmtFormat = new Regex(@"^(?<instructionWithComment>\w*?)\s*!");

                    //string[] separators = { " ", "  " };
                    string[] elements = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if(elements[0].CompareTo("!") == 0){
                        Debug.Write("Found a line with a comment on it: " + line +"\n");
                        continue;
                    }
                    if (elements.Length > 2 && !elements[2].Substring(0,1).Equals("!", StringComparison.Ordinal))
                    {
                        throw new Exception("Error: improperly formated instruction at instruction " + line );
                    }

                    var instructionCommentStmtMatch = instructionCommentStmtFormat.Match(line);
                    if (instructionCommentStmtMatch.Success)
                    {
                        var instruction = instructionCommentStmtMatch.Groups["instructionWithComment"].Value;
                        Debug.WriteLine(instruction);
                        assemblyLines.Add(instruction);

                    }
                    else
                    {
                        //Regex instructionStmtFormat = new Regex(@"\s*\w+\s+\w*\#?\$?\d*"); //zero or more white space followed by 
                        Regex instructionStmtFormat = new Regex(@"\w+\s+\w*\#?\$?\d*");
                        var instructionStmtMatch = instructionStmtFormat.Match(line);

                        Regex instructionBlankFormat = new Regex(@"\w+");
                        var instructionBlankMatch = instructionBlankFormat.Match(line);

                        if (instructionStmtMatch.Success)
                        {
                            var instruction = instructionStmtMatch.Value;
                            Debug.WriteLine(instruction);
                            assemblyLines.Add(instruction);
                        }
                        else if (instructionBlankMatch.Success)
                        {
                            var instruction = instructionBlankMatch.Value;
                            Debug.WriteLine(instruction);
                            assemblyLines.Add(instruction);
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Debug.Write(e.ToString());
                return new List<string>();
            }
            foreach (var mem in labels)
            {
                Debug.WriteLine("Label {0} points to instruction at line {1}", mem.Key, mem.Value);
            }

            Memory.setAssemblyInstructions(assemblyLines);
            return assemblyLines;
        }
    }
}
