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
            var lines = File.ReadAllLines(this.FileToParse).ToList<string>();
            int count = 0;

            Debug.WriteLine("Starting foreach loop");
            foreach (var line in lines)
            {
                Regex labelStmtFormat = new Regex(@"^(?<label>.*?)\s*:$");
                var labelStmtMatch = labelStmtFormat.Match(line);
                if (labelStmtMatch.Success)
                {
                   var label = labelStmtMatch.Groups["label"].Value;
                   Debug.WriteLine(label);
                   labels.Add(label, count + 1);
                }
                count++;
            }
            Debug.WriteLine(labels);
            foreach (var line in lines)
            {
                Regex labelStmtFormat = new Regex(@"^(?<label>.*?)\s*!");
                var labelStmtMatch = labelStmtFormat.Match(line);
                if (labelStmtMatch.Success)
                {
                    var label = labelStmtMatch.Groups["label"].Value;
                    Debug.WriteLine(label);
                   
                }
            }
        }
    }
}
