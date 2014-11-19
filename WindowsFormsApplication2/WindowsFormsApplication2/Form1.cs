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
        Queue<PipelineInstruction> instructionsInPipeline;

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
            myCPU.OnDecodeDone += myCPU_OnDecodeDone;
            myCPU.OnExecuteDone += myCPU_OnExecuteDone;
            myCPU.OnStoreDone += myCPU_OnStoreDone;
            myCPU.OnBranchTaken += myCPU_OnBranchTaken;
            instructionsInPipeline = new Queue<PipelineInstruction>(5);

            fillCacheIndexComboBox();

            this.zeroLabel.Text = this.myCPU.ZERO.ToString();
            this.oneLabel.Text = this.myCPU.ONE.ToString();

            //Starts with 1 way mode highlighted
            button2_Click(null, null);


#if DEBUG
            loadFileButton.Text = "Load File";
#endif
        }

        //This is a helper class to help with the pipeline map view
        //it includes which stage each instruction is in the pipeline
        public class PipelineInstruction
        {
            public int stage { get; set;}
            public String instructionText {get; set;}
            public int instructionIndex { get; set; }
            public PipelineInstruction(String assemblyInstruction, int instructionIndex)
            {
                stage = 0;
                this.instructionIndex = instructionIndex;
                this.instructionText = assemblyInstruction;
            }
        }

        void myCPU_OnFetchDone(object sender, FetchEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Fetch Done in GUI " + this.myCPU.ACC);
                if (args.CurrentInstructionIndex < Memory.getAssemblyInstructions().Count)
                {
                    this.irLabel.Text = args.CurrentIR.ToString();
                    String instructionText = Memory.getAssemblyInstructions().ElementAt(args.CurrentInstructionIndex);
                    if (this.instructionsInPipeline.Count > 4)
                    {
                        var temp = this.instructionsInPipeline.Dequeue();
                        Debug.WriteLine("Just dequeued " + temp);
                    }
                    this.instructionsInPipeline.Enqueue(new PipelineInstruction(instructionText, args.CurrentInstructionIndex));
                    this.setFetchPipelineLabel(args.CurrentInstructionIndex);
                    this.setPipelineValuesToView();
                }
                

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

        void myCPU_OnDecodeDone(object sender, DecodeEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Decode Done in GUI " + this.myCPU.ACC);

                this.setDecodePipelineLabel(args.CurrentInstructionIndex);
                setPipelineValuesToView();

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

        private void myCPU_OnExecuteDone(object sender, ExecuteEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Execute Done in GUI ");
                this.setExecutePipelineLabel(args.CurrentInstructionIndex);
                this.setCPUValuesToView();
                setPipelineValuesToView();
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

        private void myCPU_OnStoreDone(object sender, StoreEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Store Done in GUI ");
                this.setStorePipelineLabel(args.CurrentInstructionIndex);
                setPipelineValuesToView();
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

        void myCPU_OnBranchTaken(object sender, BranchEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Branch taken in GUI " + this.myCPU.ACC);
                
                /*What this code does is goes through the instructions in the pipeline,
                  and removes the ones that were being worked on prior to the branch being
                  taken
                 NOTE: probably will have to add an if statement to check for branch prediction*/

                int count = 0;
                Console.WriteLine("Queue contents before:");
                foreach (var instr in instructionsInPipeline)
                {
                    Console.WriteLine(count + ") " + instr.instructionText);
                    count++;
                }

                PipelineInstruction[] temp = new PipelineInstruction[instructionsInPipeline.Count()];
                instructionsInPipeline.CopyTo(temp, 0);
                instructionsInPipeline.Clear();
                int takenBranchIndex = args.CurrentInstrIndex;
                Console.WriteLine("Taken branch index is " + takenBranchIndex);
                for(int i = 0; i < temp.Count(); i++)
                {
                    //if the index is larger than the branch that was taken, it needs to be flushed out
                    if ((temp[i]).instructionIndex <= takenBranchIndex)
                    {
                        instructionsInPipeline.Enqueue(temp[i]);
                    }
                }

                count = 0;
                Console.WriteLine("Queue contents after:");
                foreach (var instr in instructionsInPipeline)
                {
                    Console.WriteLine(count + ") " + instr.instructionText);
                    count++;
                }
                this.setPipelineValuesToView();
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
                        this.setPipelineValuesToView();//not sure if this is needed
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
            if ((this.myCPU.PC) <= (Memory.getBinaryInstructions().Count))
            {
                this.myCPU.nextInstructionPipeline();//used to be nextInstruction
                Console.WriteLine("IN NEXT INTR PC counter = " + this.myCPU.PC + " and BI Count = " + Memory.getBinaryInstructions().Count);
                //since it takes 4 "next "instructions" in order for PC to increment
                if (this.myCPU.PC != 0)
                {
                    this.previousInstructionLabel.Text = this.currentInstructionLabel.Text;
                }
                this.cyclesElapsed.Text = myCPU.cycles_elapsed.ToString();
                this.cyclePenalties.Text = myCPU.cycle_penalties.ToString();

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
                //this.setPipelineValuesToView();
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

       
            
            while ((this.myCPU.PC) <= (Memory.getBinaryInstructions().Count))
            {
                nextInstructionButton_Click(sender, e);
                Console.WriteLine("PC counter = " + this.myCPU.PC + " and BI Count = " + Memory.getBinaryInstructions().Count);
                //some sort of barrier should go here to stop the deadlock
            }
             
        }

       private void resetButton_Click(object sender, EventArgs e)
       {
           resetGUI();
       }

        //Updates Pipeline Map
       public void setPipelineTexts(Label pNum, Label pFetch, Label pDecode, Label pExecute, Label pStore, PipelineInstruction instr)
       {
           String nullLine = "--------------";
           pNum.Text = instr.instructionText + "(" + instr.instructionIndex + ")";
           switch (instr.stage)
           {
               case 4:
                   pFetch.Text = "F";
                   pFetch.ForeColor = Color.DarkGreen;
                   pDecode.Text = "D";
                   pDecode.ForeColor = Color.DarkGreen;
                   pExecute.Text = "X";
                   pExecute.ForeColor = Color.DarkGreen;
                   pStore.Text = "M";
                   pStore.ForeColor = Color.DarkGreen;
                   pNum.ForeColor = Color.DarkGreen;
                   break;
               case 3:
                   pFetch.Text = "F";
                   pDecode.Text = "D";
                   pExecute.Text = "X";
                   pStore.Text = nullLine;
                   pFetch.ForeColor = label47.ForeColor;
                   pDecode.ForeColor = label47.ForeColor;
                   pExecute.ForeColor = label47.ForeColor;
                   pStore.ForeColor = label47.ForeColor;
                   pNum.ForeColor = label47.ForeColor;

                   break;
               case 2:
                   pFetch.Text = "F";
                   pDecode.Text = "D";
                   pExecute.Text = nullLine;
                   pStore.Text = nullLine;
                   break;
               case 1:
                   pFetch.Text = "F";
                   pDecode.Text = nullLine;
                   pExecute.Text = nullLine;
                   pStore.Text = nullLine;
                   break;
               case 0:
                   pFetch.Text = nullLine;
                  pDecode.Text = nullLine;
                   pExecute.Text = nullLine;
                   pStore.Text = nullLine;
                   break;
           }
       }

       public void setPipelineTextNull(Label pNum, Label pFetch, Label pDecode, Label pExecute, Label pStore)
       {
           String nullLine = "--------------";
           pNum.Text = nullLine;
           pFetch.Text = nullLine;
           pDecode.Text = nullLine;
           pExecute.Text = nullLine;
           pStore.Text = nullLine;
       }

       public void setPipelineValuesToView()
       {
           int count = 0;
           foreach(var instr in this.instructionsInPipeline){
               Debug.WriteLine("Pipeline value count " + count + " is " + instr);
               switch (count)
               {
                   case 0:
                       setPipelineTexts(pipeline1, pipeline1Fetch, pipeline1Decode, pipeline1Execute, pipeline1Store, instr);
                       break;
                   case 1:
                       setPipelineTexts(pipeline2,pipeline2Fetch, pipeline2Decode, pipeline2Execute, pipeline2Store, instr);
                       break;
                   case 2:
                       setPipelineTexts(pipeline3, pipeline3Fetch, pipeline3Decode, pipeline3Execute, pipeline3Store, instr);
                       break;
                   case 3:
                       setPipelineTexts(pipeline4, pipeline4Fetch, pipeline4Decode, pipeline4Execute, pipeline4Store, instr);
                       break;
                   case 4:
                       setPipelineTexts(pipeline5, pipeline5Fetch, pipeline5Decode, pipeline5Execute, pipeline5Store, instr);
                       break;
               }
               count++;
           }

           //5 since we look at 5 instructions in the pipeline
           //sometimes when we update the pipeline values
           //there may be empty pipelines as a result of a flush
           while (count < 5)
           {
               switch (count)
               {
                   case 0:
                       setPipelineTextNull(pipeline1, pipeline1Fetch, pipeline1Decode, pipeline1Execute, pipeline1Store);
                       break;
                   case 1:
                       setPipelineTextNull(pipeline2, pipeline2Fetch, pipeline2Decode, pipeline2Execute, pipeline2Store);
                       break;
                   case 2:
                       setPipelineTextNull(pipeline3, pipeline3Fetch, pipeline3Decode, pipeline3Execute, pipeline3Store);
                       break;
                   case 3:
                       setPipelineTextNull(pipeline4, pipeline4Fetch, pipeline4Decode, pipeline4Execute, pipeline4Store);
                       break;
                   case 4:
                       setPipelineTextNull(pipeline5, pipeline5Fetch, pipeline5Decode, pipeline5Execute, pipeline5Store);
                       break;
               }
               count++;
           }
       }

       

        //updates the pipeline label indicating the given instruction has been fetched
       public void setFetchPipelineLabel(int instrIndex)
       {
           foreach (var instr in instructionsInPipeline)
           {
               //if the instruction at the label is the same instruction that was just decoded
               //this might break if theres multiple of the same instruction being pipelined
               if (instr.instructionIndex == instrIndex)
               {
                   instr.stage = 1;//fetch stage
                   Console.WriteLine(instr.instructionText + " was set to stage " + instr.stage);
               }
           }
       }
       public void setDecodePipelineLabel(int instrIndex)
       {
           foreach (var instr in instructionsInPipeline)
           {
               //if the instruction at the label is the same instruction that was just decoded
               //this might break if theres multiple of the same instruction being pipelined
               if (instr.instructionIndex == instrIndex)
               {
                   instr.stage = 2;//decode stage
                   Console.WriteLine(instr.instructionText + " was set to stage " + instr.stage);
               }
           }
       }
       public void setExecutePipelineLabel(int instrIndex)
       {
           foreach (var instr in instructionsInPipeline)
           {
               //if the instruction at the label is the same instruction that was just decoded
               //this might break if theres multiple of the same instruction being pipelined
               if (instr.instructionIndex == instrIndex)
               {
                   instr.stage = 3;//execute stage
                   Console.WriteLine(instr.instructionText + " was set to stage " + instr.stage);
               }
           }
       }
       //updates the pipeline label indicating the given instruction has been store
       public void setStorePipelineLabel(int instrIndex)
       {
           foreach (var instr in instructionsInPipeline)
           {
               //if the instruction at the label is the same instruction that was just decoded
               //this might break if theres multiple of the same instruction being pipelined
               if (instr.instructionIndex == instrIndex)
               {
                   instr.stage = 4;//fetch stage
                   Console.WriteLine(instr.instructionText + " was set to stage " + instr.stage);
               }
           }
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
            this.myCPU.reset();//this won't work with pipelines
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
            resetPipelineLabels();
            this.instructionsInPipeline = new Queue<PipelineInstruction>(5);
            this.cyclesElapsed.Text = "-------------";
            this.cyclePenalties.Text = "-------------";
        }

        void resetPipelineLabels(){
            String pipelineText = "--------------";
            List<Label> pipelineMapLabels = new List<Label>();
            pipelineMapLabels.Add(pipeline1);
            pipelineMapLabels.Add(pipeline2);
            pipelineMapLabels.Add(pipeline3);
            pipelineMapLabels.Add(pipeline4);
            pipelineMapLabels.Add(pipeline5);
            pipelineMapLabels.Add(pipeline1Fetch);
            pipelineMapLabels.Add(pipeline2Fetch);
            pipelineMapLabels.Add(pipeline3Fetch);
            pipelineMapLabels.Add(pipeline4Fetch);
            pipelineMapLabels.Add(pipeline5Fetch);
            pipelineMapLabels.Add(pipeline1Decode);
            pipelineMapLabels.Add(pipeline2Decode);
            pipelineMapLabels.Add(pipeline3Decode);
            pipelineMapLabels.Add(pipeline4Decode);
            pipelineMapLabels.Add(pipeline5Decode);
            pipelineMapLabels.Add(pipeline1Execute);
            pipelineMapLabels.Add(pipeline2Execute);
            pipelineMapLabels.Add(pipeline3Execute);
            pipelineMapLabels.Add(pipeline4Execute);
            pipelineMapLabels.Add(pipeline5Execute);
            pipelineMapLabels.Add(pipeline1Store);
            pipelineMapLabels.Add(pipeline2Store);
            pipelineMapLabels.Add(pipeline3Store);
            pipelineMapLabels.Add(pipeline4Store);
            pipelineMapLabels.Add(pipeline5Store);

            foreach(Label plabel in pipelineMapLabels){
                plabel.Text = pipelineText;
                plabel.ForeColor = label47.ForeColor;
            }

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

        private void label4_Click(object sender, EventArgs e)
        {

        }

 
        

    }
}
