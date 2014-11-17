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
        bool addressMode = false; // default 1 way
        Queue<String> instructionsInPipeline;

        public Form1()
        {

            InitializeComponent();
            fillMemComboBox();
            fillCacheSizeComboBox();
            fillBlockSizeComboBox();

            cacheSizeBox.SelectedIndex = 0;

            memory = new Memory((int)(cacheSizeBox.SelectedItem), (int)(blockSizeBox.SelectedItem), addressMode);
            myCPU = new CPU(memory);
            myCPU.OnFetchDone += myCPU_OnFetchDone;
            instructionsInPipeline = new Queue<String>(5);

            fillCacheIndexComboBox();

            this.zeroLabel.Text = this.myCPU.ZERO.ToString();
            this.oneLabel.Text = this.myCPU.ONE.ToString();

            //Starts with 1 way mode highlighted
            button2_Click(null, null);


#if DEBUG
            loadFileButton.Text = "Load File";
#endif
        }

        void myCPU_OnFetchDone(object sender, FetchEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Fetch Done in GUI " + this.myCPU.ACC);
                this.irLabel.Text = args.CurrentIR.ToString();
                String instructionText = Memory.getAssemblyInstructions().ElementAt(args.CurrentInstructionIndex);
                this.instructionsInPipeline.Enqueue(instructionText);

            };

            if (this.InvokeRequired)
            {
                this.Invoke(method);
            }
            else
            {
                method.Invoke();
            }
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
                this.myCPU.nextInstructionPipeline();//used to be nextInstruction
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
            this.pcLabel.Text = this.myCPU.PC.ToString();
            this.tempLabel.Text = this.myCPU.TEMP.ToString();
            if (myCPU.PC < Memory.getBinaryInstructions().Count)
            {
                this.irLabel.Text = Memory.getBinaryInstructions().ElementAt(myCPU.PC).ToString();
            }
            this.ccLabel.Text = this.myCPU.CC.ToString();
        }

        /////////////////////////////////////////////////////////////////
        //New to Project 2, method which handles updating cache labels///
        /////////////////////////////////////////////////////////////////
        public void setCacheLabelsToView()
        {
            this.readHitLabel.Text = this.myCPU.memory.readHitCounter.ToString();
            this.readMissLabel.Text = this.myCPU.memory.readMissCounter.ToString();
            this.writeMissLabel.Text = this.myCPU.memory.writeMissCounter.ToString();
            this.writeHitLabel.Text = this.myCPU.memory.writeHitCounter.ToString();
            this.totalHitsLabel.Text = (this.myCPU.memory.readHitCounter + this.myCPU.memory.writeHitCounter).ToString();
            this.totalMissesLabel.Text = (this.myCPU.memory.readMissCounter + this.myCPU.memory.writeMissCounter).ToString();
            var index = this.cacheIndexComboBox.SelectedIndex;
            cacheValueLabel.Text = (((this.myCPU.memory.cache[index]) & 16711680) >> 16).ToString();
            cacheTagLabel.Text = (this.myCPU.memory.getTagAtCacheIndex(index)).ToString();
            this.hitOrMissLabel.Text = this.myCPU.memory.hitOrMiss;
            this.spatialHitsLabel.Text = this.myCPU.memory.spatialCounter.ToString();

        }

        public void resetGUI()
        {
            this.myCPU.reset();
            memory = new Memory( (int)(this.cacheSizeBox.SelectedItem), (int)(this.blockSizeBox.SelectedItem), addressMode );
            this.myCPU.memory = memory;
            setCPUValuesToView();
            setCacheLabelsToView();
            Memory.clearStack();
            this.currMemValueLabel.Text = Memory.stack[this.memComboBox.SelectedIndex].ToString();
            this.currentInstructionLabel.Text = "--------------------------------";
            this.previousInstructionLabel.Text = "--------------------------------";
            this.instructionCount = 0;
            this.currInstructionCountLabel.Text = instructionCount.ToString();
            this.pcLabel.Text =  this.myCPU.PC.ToString();
            this.accLabel.Text = this.myCPU.ACC.ToString();
            this.aLabel.Text = this.myCPU.ACC.ToString();
            this.ccLabel.Text = this.myCPU.ACC.ToString();
            this.totalInstructionCountLabel.Text = "0";
            this.fileNameLabel.Text = "...";
            this.irLabel.Text = "0";
            this.cacheIndexComboBox.Items.Clear();
            fillCacheIndexComboBox();
            
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

        ////////////////////////////////////////////////////////////////
        //New to Project 2, method which handles cache index combo box//
        ////////////////////////////////////////////////////////////////
        public void fillCacheIndexComboBox()
        {
            this.cacheIndexComboBox.SelectedIndexChanged +=
                new System.EventHandler(ComboBox3_SelectedIndexChanged);

            for (int i = 0; i < this.myCPU.memory.cacheSize; i++)
            {
                this.cacheIndexComboBox.Items.Add(i);
            }

            this.cacheIndexComboBox.SelectedIndex = 0;
        }

        ///////////////////////////////////////////////////////////////
        //New to Project 2, method which handles cache size combo box//
        ///////////////////////////////////////////////////////////////
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

        ///////////////////////////////////////////////////////////////
        //New to Project 2, method which handles block size combo box//
        ///////////////////////////////////////////////////////////////
        public void fillBlockSizeComboBox()
        {
            this.blockSizeBox.SelectedIndexChanged +=
                new System.EventHandler(blockSizeBox_SelectedIndexChanged);

            this.blockSizeBox.Items.Add(1);
            this.blockSizeBox.Items.Add(2);
            this.blockSizeBox.SelectedIndex = 0;
        }

     
        private void button1_Click(object sender, EventArgs e) 
        {//2way
            addressMode = true;
            button1.BackColor = Color.LightYellow;
            button2.BackColor = resetButton.BackColor;
        }
        private void button2_Click(object sender, EventArgs e)
        {//1way
            addressMode = false;
            button2.BackColor = Color.LightYellow;
            button1.BackColor = resetButton.BackColor;
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
            //Debug.WriteLine("Cachesizebos selected index is " + index);
            Memory.setCacheStackPointer(index);
            //Debug.WriteLine("Got here");
        }

        private void ComboBox3_SelectedIndexChanged(object sender,
        System.EventArgs e)
        {
            var index = this.cacheIndexComboBox.SelectedIndex;
            cacheValueLabel.Text = (((this.myCPU.memory.cache[index]) & 16711680) >> 16).ToString();
            cacheTagLabel.Text = (this.myCPU.memory.getTagAtCacheIndex(index)).ToString();
        }

        private void blockSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = this.blockSizeBox.SelectedIndex;
            //Memory.setBlockSize(index);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void accLabel_Click(object sender, EventArgs e)
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

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void tempLabel_Click(object sender, EventArgs e)
        {

        }

        private void pcLabel_Click(object sender, EventArgs e)
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

        private void cacheSizeLabel_Click(object sender, EventArgs e)
        {

        }

        private void memComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void currMemValueLabel_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

 
        

    }
}
