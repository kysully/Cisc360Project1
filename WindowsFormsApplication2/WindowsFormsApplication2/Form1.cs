/**
 * Kyle Sullivan and Melody Lugo
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using GeminiCore;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public CPU myCPU;
        int instructionCount = 0;
        string defaultFileName = "c:\\users\\kyle\\desktop\\binaryOutput1.out";

        public Form1()
        {
            myCPU = new CPU();

            InitializeComponent();

            this.zeroLabel.Text = "0x" + this.myCPU.ZERO.ToString("X7");
            this.oneLabel.Text = "0x" + this.myCPU.ONE.ToString("X7");

#if DEBUG
            loadFileButton.Text = "Load File";
#endif
        }

        #region Events
        private void loadFileButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        resetGUI();
                        var ipe = new IPE(ofd.FileName);
                        List<string> assemblyLines = ipe.ParseFile();
                        if (assemblyLines.Count == 0)
                        {
                            //maybe make a popup here or something
                            return;
                        }
                        short[] binaryLines = ipe.AssemblytoBinary(assemblyLines);
                        //Memory.setBinaryInstructions(binaryLines.ToList());//Now memory contains all the instructions (in binary) we read in from the file
                        ipe.WriteBinarytoFile(binaryLines, defaultFileName);//Write out the binary instructions to a file
                        binaryLines = ipe.readBinaryFromFile(defaultFileName);//Read in the binary and load to memory
                        Memory.setBinaryInstructions(binaryLines.ToList());//Load the binary we just read from file into Memory
                        currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(0);
                        totalInstructionCountLabel.Text = (Memory.getAssemblyInstructions().Count).ToString();
                        instructionCount = 1;
                        currInstructionCountLabel.Text = (this.myCPU.PC+1).ToString();
                    }
                    catch (Exception err)
                    {
                        // show a dialog with error              
                    }
                }
            }
        }
        #endregion

        private void nextInstructionButton_Click(object sender, EventArgs e)
        {
            if ((this.myCPU.PC) < (Memory.getBinaryInstructions().Count))
            {
                this.myCPU.nextInstruction();
                this.previousInstructionLabel.Text = this.currentInstructionLabel.Text;
                if ((this.myCPU.PC < (Memory.getAssemblyInstructions().Count)))
                {
                    this.currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(myCPU.PC);
                    currInstructionCountLabel.Text = (this.myCPU.PC + 1).ToString();
                }
                else
                {
                    this.currentInstructionLabel.Text = "--------------------------------";
                    this.setCPUValuesToView();
                    //Do a popup here saying you finished the program
                    MessageBox.Show("The loaded assembly program has finished.");

                }

                this.setCPUValuesToView();
                if (instructionCount < Memory.getAssemblyInstructions().Count)
                {
                    this.instructionCount++;
                }
            }
            else
            {
                this.currentInstructionLabel.Text = "--------------------------------";

            }
            
        }

        private void runAllButton_Click(object sender, EventArgs e)
        {
            Debug.Write("test");
            for (int i = this.myCPU.PC; i < Memory.getBinaryInstructions().Count; i++ )
            {
                nextInstructionButton_Click(sender, e);
            }
        }

       private void resetButton_Click(object sender, EventArgs e)
       {
           resetGUI();
       }

        public void setCPUValuesToView()
        {
            this.accLabel.Text = "0x" + this.myCPU.ACC.ToString("X7");
            this.aLabel.Text = "0x" + this.myCPU.ACC.ToString("X7");
            this.pcLabel.Text = "0x" + this.myCPU.PC.ToString("X7");
        }

        public void resetGUI()
        {
            this.myCPU.reset();
            this.currentInstructionLabel.Text = "--------------------------------";
            this.previousInstructionLabel.Text = "--------------------------------";
            this.instructionCount = 0;
            this.currInstructionCountLabel.Text = instructionCount.ToString();
            this.pcLabel.Text = "0x" + this.myCPU.PC.ToString("X7");
            this.totalInstructionCountLabel.Text = "0";


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void accLabel_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void aLabel_Click(object sender, EventArgs e)
        {
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bLabel_Click(object sender, EventArgs e)
        {

        }

        private void totalInstructionCountLabel_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {
            myCPU.executeBinary(8965);
            myCPU.executeBinary(0);
            myCPU.executeBinary(9472);//sta
            myCPU.executeBinary(4352);//lda
            myCPU.executeBinary(17152);//add#
            myCPU.executeBinary(16896);//add
            myCPU.executeBinary(17920);//m
            myCPU.executeBinary(25856);//0r#
            myCPU.executeBinary(-30720);//bg

        }
    }
}
