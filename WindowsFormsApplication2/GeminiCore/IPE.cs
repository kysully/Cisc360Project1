/**
 * Kyle Sullivan
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

        Dictionary<String, int> labels; // String = label, int = line number the label points to

        public string FileToParse { get; set; }

        public IPE(string filename)
        {
            this.FileToParse = filename;
            labels = new Dictionary<String, int>();
        }

        public void ParseFile()
        {
            var linesRaw = File.ReadAllLines(this.FileToParse).ToArray<string>();
            var linesWithLabels = new List<string>();
            var lines = new List<string>();

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
            foreach (var line in lines)
            {
                //Regex instructionCommentStmtFormat = new Regex(@"^(?<instructionWithComment>.*?)\s*!");//grabs any text before a ! (comment)
                Regex instructionCommentStmtFormat = new Regex(@"^(?<instructionWithComment>\w*?)\s*!");

                var instructionCommentStmtMatch = instructionCommentStmtFormat.Match(line);
                if (instructionCommentStmtMatch.Success)
                {
                    var instruction = instructionCommentStmtMatch.Groups["instructionWithComment"].Value;
                    Debug.WriteLine(instruction);

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
                    }
                    else if (instructionBlankMatch.Success)
                    {
                        var instruction = instructionBlankMatch.Value;
                        Debug.WriteLine(instruction);
                    }

                }

            }
            foreach (var mem in labels)
            {
                Debug.WriteLine("Label {0} points to instruction at line {1}", mem.Key, mem.Value);
            }
        }
    }
}
