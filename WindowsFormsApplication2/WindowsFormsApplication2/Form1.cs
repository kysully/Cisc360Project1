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
using System.IO;

using GeminiCore;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public CPU myCPU;
        public Memory memory;
        int instructionCount = 0;

        public Form1()
        {

            InitializeComponent();
            fillMemComboBox();
            fillCacheSizeComboBox();

            cacheSizeBox.SelectedIndex = 0;

            memory = new Memory(2, false);
            myCPU = new CPU(memory);

            this.zeroLabel.Text = "0x" + this.myCPU.ZERO.ToString("X8");
            this.oneLabel.Text = "0x" + this.myCPU.ONE.ToString("X8");

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
                        string fileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                        this.fileNameLabel.Text = fileName + ".s";
                        var ipe = new IPE(ofd.FileName);
                        List<string> assemblyLines = ipe.ParseFile();
                        if (assemblyLines.Count == 0)
                        {
                            return;
                        }
                        short[] binaryLines = ipe.AssemblytoBinary(assemblyLines);
                        string tempFileName = Path.GetDirectoryName(ofd.FileName) + "\\" + fileName + ".out";
                        Debug.WriteLine("\nOutput to file: " + tempFileName);
                        ipe.WriteBinarytoFile(binaryLines, tempFileName);//Write out the binary instructions to a file
                        binaryLines = ipe.readBinaryFromFile(tempFileName);//Read in the binary and load to memory
                        Memory.setBinaryInstructions(binaryLines.ToList());//Load the binary we just read from file into Memory
                        currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(0);
                        totalInstructionCountLabel.Text = (Memory.getAssemblyInstructions().Count).ToString();
                        instructionCount = 1;
                        currInstructionCountLabel.Text = (this.myCPU.PC+1).ToString();
                    }
                    catch (Exception err)
                    {
                        // show a dialog with error     
                        MessageBox.Show(err.Message);
                        resetGUI();
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

                var temp = (this.previousInstructionLabel.Text).Substring(0, 3);
                if ( temp.CompareTo("sta") == 0)
                {
                    this.currMemValueLabel.Text = Memory.stack[this.memComboBox.SelectedIndex].ToString();
                }
                if ((this.myCPU.PC < (Memory.getAssemblyInstructions().Count)))
                {
                    this.currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(myCPU.PC);
                    currInstructionCountLabel.Text = (this.myCPU.PC + 1).ToString();
                }
                else
                {
                    this.currentInstructionLabel.Text = "--------------------------------";
                    this.setCPUValuesToView();
                    this.setCacheLabelsToView();
                    this.currInstructionCountLabel.Text = this.totalInstructionCountLabel.Text;
                    MessageBox.Show("The loaded assembly program has finished.");

                }

                this.setCPUValuesToView();
                this.setCacheLabelsToView();
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
            //Debug.Write("test");
            while (this.myCPU.PC < Memory.getBinaryInstructions().Count)
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

            this.accLabel.Text = this.myCPU.ACC.ToString();
            this.pcLabel.Text = "0x" + this.myCPU.PC.ToString("X8");
            this.tempLabel.Text = "0x" + this.myCPU.TEMP.ToString("X8");
            if (myCPU.PC < Memory.getBinaryInstructions().Count)
            {
                this.irLabel.Text = "0x" + Memory.getBinaryInstructions().ElementAt(myCPU.PC).ToString("X8");
            }
            this.ccLabel.Text = "0x" + this.myCPU.CC.ToString("X8");
        }

        public void setCacheLabelsToView()
        {
            this.readHitLabel.Text = this.myCPU.memory.readHitCounter.ToString();
            this.readMissLabel.Text = this.myCPU.memory.readMissCounter.ToString();
            this.writeMissLabel.Text = this.myCPU.memory.writeMissCounter.ToString();
            this.writeHitLabel.Text = this.myCPU.memory.writeHitCounter.ToString();
            this.totalHitsLabel.Text = (this.myCPU.memory.readHitCounter + this.myCPU.memory.writeHitCounter).ToString();
            this.totalMissesLabel.Text = (this.myCPU.memory.readMissCounter + this.myCPU.memory.writeMissCounter).ToString();

        }

        public void resetGUI()
        {
            this.myCPU.reset();
            memory = new Memory( (int)(this.cacheSizeBox.SelectedItem), false );
            this.myCPU.memory = memory;
            setCPUValuesToView();
            setCacheLabelsToView();
            Memory.clearStack();
            this.currMemValueLabel.Text = Memory.stack[this.memComboBox.SelectedIndex].ToString();
            this.currentInstructionLabel.Text = "--------------------------------";
            this.previousInstructionLabel.Text = "--------------------------------";
            this.instructionCount = 0;
            this.currInstructionCountLabel.Text = instructionCount.ToString();
            this.pcLabel.Text = "0x" + this.myCPU.PC.ToString("X8");
            this.accLabel.Text = "0x" + this.myCPU.ACC.ToString("X8");
            this.aLabel.Text = "0x" + this.myCPU.ACC.ToString("X8");
            this.ccLabel.Text = "0x" + this.myCPU.ACC.ToString("X8");
            this.totalInstructionCountLabel.Text = "0";
            this.fileNameLabel.Text = "...";
            this.irLabel.Text = "0x" + "00000000";
            
        }

        public void fillMemComboBox()
        {
            this.memComboBox.SelectedIndexChanged +=
            new System.EventHandler(ComboBox1_SelectedIndexChanged);

            for (int i = 0; i < 256; i++)
            {
                this.memComboBox.Items.Add(i);
            }

            this.memComboBox.SelectedIndex = 0;
        }

        public void fillCacheSizeComboBox()
        {
            this.cacheSizeBox.SelectedIndexChanged +=
            new System.EventHandler(comboBox2_SelectedIndexChanged);

            this.cacheSizeBox.Items.Add(2);
            this.cacheSizeBox.Items.Add(4);
            this.cacheSizeBox.Items.Add(8);
            this.cacheSizeBox.Items.Add(16);
            this.cacheSizeBox.SelectedIndex = 0;
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
        }

        private void ComboBox1_SelectedIndexChanged(object sender,
        System.EventArgs e)
        {
            var index = this.memComboBox.SelectedIndex;
            currMemValueLabel.Text = (Memory.stack[index]).ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var index = this.cacheSizeBox.SelectedIndex;
            Debug.WriteLine("Cachesizebos selected index is " + index);
            Memory.setCacheStackPointer(index);
            Debug.WriteLine("Got here");
        }

        private void blockSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = this.blockSizeBox.SelectedIndex;
            Debug.WriteLine("Got here1");
            //Memory.setBlockSize(index);
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tempLabel_Click(object sender, EventArgs e)
        {

        }

        private void pcLabel_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click_1(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void cacheSizeLabel_Click(object sender, EventArgs e)
        {

        }

        private void memComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void currMemValueLabel_Click(object sender, EventArgs e)
        {

        }

    }
}
