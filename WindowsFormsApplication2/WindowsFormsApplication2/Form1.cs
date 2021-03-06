﻿/**
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
        List<Label> branchLabels = new List<Label>();
        List<BHTWrapper> bhtBranches = new List<BHTWrapper>();       
        bool fetchDone, decodeDone, executeDone, storeDone, programDone;

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
            myCPU.OnBranch += myCPU_OnBranch;
            myCPU.OnStageDone += myCPU_OnStageDone;
            fetchDone = false;
            decodeDone = false;
            executeDone = false;
            storeDone = false;
            programDone = false;
            
            instructionsInPipeline = new Queue<PipelineInstruction>(5);

            fillCacheIndexComboBox();

            this.zeroLabel.Text = this.myCPU.ZERO.ToString();
            this.oneLabel.Text = this.myCPU.ONE.ToString();

            //Starts with 1 way mode highlighted
            button2_Click(null, null);
            button6_Click(null, null);
            button3_Click(null, null);



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
          Console.WriteLine("New intr in pipeline: " + instructionText + ", index: " + instructionIndex);
            }
        }

        public class BHTWrapper
        {
            public String instrLabel { get; set; }
            public int takenCount { get; set; }
            public int notTakenCount { get; set; }
            public BHTWrapper(String label)
            {
                this.instrLabel = label;
                this.takenCount = 0;
                this.notTakenCount = 0;
            }
        }

        void myCPU_OnFetchDone(object sender, FetchEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Fetch Done in GUI ");
                if (args.CurrentInstructionIndex < Memory.getAssemblyInstructions().Count && args.CurrentInstructionIndex >= 0)
                {
                    //this.irLabel.Text = args.CurrentIR.ToString();
                    //Console.WriteLine("Just set IR label to:" + irLabel.Text, " and index was" + args.CurrentInstructionIndex);
                    String instructionText = Memory.getAssemblyInstructions().ElementAt(args.CurrentInstructionIndex);
                    if (this.instructionsInPipeline.Count > 4)
                    {
                        var temp = this.instructionsInPipeline.Dequeue();
                        Debug.WriteLine("Just dequeued " + temp);
                    }
                    this.instructionsInPipeline.Enqueue(new PipelineInstruction(instructionText, args.CurrentInstructionIndex));
                    this.setFetchPipelineLabel(args.CurrentInstructionIndex);
                    fetchDone = true;
                    /*if (fetchDone && decodeDone && executeDone && storeDone)
                    {
                        Console.WriteLine("Fetch was last to finish, setting labels.");
                        setPipelineValuesToView();
                    } */                
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
                //setPipelineValuesToView();
                if (myCPU.currBranchInstr != "")
                {
                    String currBranch = myCPU.currBranchInstr;
                    Console.WriteLine("Current Branch Instr: " + currBranch);
                    bool inBHT = false;
                    foreach(var branch in bhtBranches){
                        if(branch.instrLabel.CompareTo(currBranch) == 0){
                            //branch.notTakenCount++;
                            inBHT = true;
                        }
                    }
                    if(!inBHT){
                        bhtBranches.Add(new BHTWrapper(currBranch));
                    }
                }
                setBHTToView();
                decodeDone = true;
                /*if (fetchDone && decodeDone && executeDone && storeDone)
                {
                    Console.WriteLine("Decode was last to finish, setting labels.");
                    setPipelineValuesToView();
                }*/
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

        private void myCPU_OnExecuteDone(object sender,ExecuteEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("Execute Done in GUI ");
                if (this.myCPU.mdHazard)
                {
                    this.label43.Text = "A mul or div has been excuted, 4 cycle penalties!";
                    this.myCPU.mdHazard = false;
                }
                this.setExecutePipelineLabel(args.CurrentInstructionIndex);
                this.setCPUValuesToView();
                executeDone = true;
                /*if (fetchDone && decodeDone && executeDone && storeDone)
                {
                    Console.WriteLine("Execute was last to finish, setting labels.");
                    setPipelineValuesToView();
                } */             
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
                if (args.programDone)
                {
                    programDone = true;
                    this.previousInstructionLabel.Text = this.currentInstructionLabel.Text;
                }
                else
                {
                    this.setStorePipelineLabel(args.CurrentInstructionIndex);
                    //setPipelineValuesToView();
                    //in case we need to update memory box
                    ComboBox1_SelectedIndexChanged(this, new EventArgs());
                    ComboBox3_SelectedIndexChanged(this, new EventArgs());
                    this.previousInstructionLabel.Text = this.currentInstructionLabel.Text;
                    if (this.myCPU.PC < Memory.getBinaryInstructions().Count())
                    {
                        this.irLabel.Text = Memory.getBinaryInstructions().ElementAt(myCPU.PC).ToString();//elementat(pc) to elementat(fetch counter)                
                        this.currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(myCPU.PC);
                    }
                    else
                    {
                        this.currentInstructionLabel.Text = "--------------------------------";
                    }

                    storeDone = true;
                }                
                /*if (fetchDone && decodeDone && executeDone && storeDone)
                {
                    Console.WriteLine("Store was last to finish, setting labels.");
                    setPipelineValuesToView();
                }*/
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

        void myCPU_OnBranch(object sender, BranchEventArgs args)
        {
            MethodInvoker method = delegate
            {
                if (args.taken)
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
                        Console.WriteLine(count + ") " + instr.instructionText + ", stage is " + instr.stage);
                        count++;
                    }

                    PipelineInstruction[] temp = new PipelineInstruction[instructionsInPipeline.Count()];
                    instructionsInPipeline.CopyTo(temp, 0);
                    instructionsInPipeline.Clear();
                    int takenBranchIndex = args.CurrentInstrIndex;
                    Console.WriteLine("Taken branch index is " + takenBranchIndex);
                    for (int i = 0; i < temp.Count(); i++)
                    {
                        //if the index is larger than the branch that was taken, it needs to be flushed out
                        if ((temp[i]).instructionIndex < takenBranchIndex)
                        {
                            instructionsInPipeline.Enqueue(temp[i]);
                        }
                        else if (temp[i].instructionIndex == takenBranchIndex)
                        {
                            temp[i].stage++;//this is to correct for the special branch case
                            instructionsInPipeline.Enqueue(temp[i]);
                        }
                    }

                    count = 0;
                    Console.WriteLine("Queue contents after:");
                    foreach (var instr in instructionsInPipeline)
                    {
                        Console.WriteLine(count + ") " + instr.instructionText + ", stage is " + instr.stage);
                        count++;
                    }
                    //this.setPipelineValuesToView();

                }          

                foreach(var branch in bhtBranches){
                    String currBranchName = Memory.getAssemblyInstructions().ElementAt(args.CurrentInstrIndex);
                    if (currBranchName.CompareTo(branch.instrLabel) == 0)
                    {
                        if (args.taken)
                        {
                            branch.takenCount++;
                        }
                        else
                        {
                            branch.notTakenCount++;
                        }
                    }
                }
                //updateGUI();
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

        void myCPU_OnStageDone(object sender, StageDoneEventArgs args)
        {
            MethodInvoker method = delegate
            {
                Console.WriteLine("inside on stage done in the GUI");
                this.updateGUI();
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

        //Updates the entire screen, all labels ect...
        public void updateGUI()
        {
            this.setPipelineValuesToView();
            this.setCPUValuesToView();
            this.setCacheLabelsToView();
            this.setBHTToView();
            int count = this.myCPU.PC + 1;
            this.currInstructionCountLabel.Text = (this.myCPU.PC + 1).ToString();
            if (count > Memory.getBinaryInstructions().Count)
            {
                this.currInstructionCountLabel.Text = Memory.getBinaryInstructions().Count.ToString();
            }
            this.cyclesElapsed.Text = myCPU.cycles_elapsed.ToString();
            this.cyclePenalties.Text = myCPU.cycle_penalties.ToString();
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
            this.label43.Text = "--------------";
            if (programDone)
            {
                this.currentInstructionLabel.Text = "--------------------------------";
                this.setCPUValuesToView();
                this.setCacheLabelsToView();
                updateGUI();
                this.currInstructionCountLabel.Text = this.totalInstructionCountLabel.Text;
                //this.myCPU.nextInstructionPipeline();
                MessageBox.Show("The loaded assembly program has finished.");
            }
            else
            {
                //Makes the call to the CPU to run the next instruction
                this.myCPU.nextInstructionPipeline();//used to be nextInstruction prior to pipelining

                //updates memory label
                if (this.previousInstructionLabel.Text.Length > 2)
                {
                    var temp = (this.previousInstructionLabel.Text).Substring(0, 3);
                    if (temp.CompareTo("sta") == 0)
                    {
                        this.currMemValueLabel.Text = Memory.stack[this.memComboBox.SelectedIndex].ToString();
                    }

                }
                bool found = false;
                foreach (var instr in instructionsInPipeline)
                {
                    if (instr.stage == 3)
                    {
                        this.currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(instr.instructionIndex);
                        found = true;
                    }
                }
                if (!found)
                {
                    this.currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(this.myCPU.PC + 1);
                }
                currInstructionCountLabel.Text = (this.myCPU.PC + 1).ToString();

            }
          
            
        }

        private void runAllButton_Click(object sender, EventArgs e)
        {
             
        }

       private void resetButton_Click(object sender, EventArgs e)
       {
           resetGUI();
       }

          //Updates Branch History Table
       public void setBHTToView()
       {
           int count = 0;
           if (bhtBranches != null)
           {
               foreach (var branch in bhtBranches)
               {
                   String bString = branch.instrLabel;
                   int numNotTaken = branch.notTakenCount;
                   int numTaken = branch.takenCount;
                   Console.WriteLine("CURRENT BSTRING " + bString);
                   switch (count)
                   {
                       case 0:
                           branch1.Text = bString;
                           branch1NumNotTaken.Text = numNotTaken.ToString();
                           branch1NumTaken.Text = numTaken.ToString();
                           break;
                       case 1:
                           branch2.Text = bString;
                           branch2NumNotTaken.Text = numNotTaken.ToString();
                           branch2NumTaken.Text = numTaken.ToString();
                           break;
                       case 2:
                           branch3.Text = bString;
                           branch3NumNotTaken.Text = numNotTaken.ToString();
                           branch3NumTaken.Text = numTaken.ToString();
                           break;
                       case 3:
                           branch4.Text = bString;
                           break;
                       case 4:
                           branch5.Text = bString;
                           break;
                   }
                   count++;
               }
           }
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
                   pFetch.ForeColor = label47.ForeColor;
                   pDecode.ForeColor = label47.ForeColor;
                   pExecute.ForeColor = label47.ForeColor;
                   pStore.ForeColor = label47.ForeColor;
                   pNum.ForeColor = label47.ForeColor;

                   break;
               case 1:
                   pFetch.Text = "F";
                   pDecode.Text = nullLine;
                   pExecute.Text = nullLine;
                   pStore.Text = nullLine;
                   break;
                   pFetch.ForeColor = label47.ForeColor;
                   pDecode.ForeColor = label47.ForeColor;
                   pExecute.ForeColor = label47.ForeColor;
                   pStore.ForeColor = label47.ForeColor;
                   pNum.ForeColor = label47.ForeColor;

               case 0:
                   pFetch.Text = nullLine;
                   pDecode.Text = nullLine;
                   pExecute.Text = nullLine;
                   pStore.Text = nullLine;
                   pFetch.ForeColor = label47.ForeColor;
                   pDecode.ForeColor = label47.ForeColor;
                   pExecute.ForeColor = label47.ForeColor;
                   pStore.ForeColor = label47.ForeColor;
                   pNum.ForeColor = label47.ForeColor;

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
           int count = 0;
           foreach (var instr in instructionsInPipeline)
           {
               Console.WriteLine("Pipeline(" + count + ") is index " + instr.instructionIndex + " and stored instruction index is " + instrIndex);
               //if the instruction at the label is the same instruction that was just decoded
               //this might break if theres multiple of the same instruction being pipelined
               if (instr.instructionIndex == instrIndex)
               {
                   instr.stage = 4;//fetch stage
                   Console.WriteLine(instr.instructionText + " was set to stage " + instr.stage);
               }
               count++;
           }
       }

        public void setCPUValuesToView()
        {

            this.accLabel.Text = this.myCPU.ACC.ToString();
            this.pcLabel.Text = this.myCPU.PC.ToString();
            this.tempLabel.Text = this.myCPU.TEMP.ToString();
            if (myCPU.PC < Memory.getBinaryInstructions().Count)
            {
                this.irLabel.Text = Memory.getBinaryInstructions().ElementAt(myCPU.PC).ToString();//elementat(pc) to elementat(fetch counter)
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
        {/*
            Form1 newForm = new Form1();
            this.Hide();
            newForm.Show();
            */
            memory = new Memory( (int)(this.cacheSizeBox.SelectedItem), (int)(this.blockSizeBox.SelectedItem), addressMode );
            this.myCPU.memory = memory;
            this.myCPU.reset();//this won't work with pipelines
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
            programDone = false;

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

        //Bypass On
        private void button3_Click(object sender, EventArgs e)
        {
            this.myCPU.bypassing = true;
            button3.BackColor = Color.LightYellow;
            button4.BackColor = resetButton.BackColor;
        }

       //Bypass Off
        private void button4_Click(object sender, EventArgs e)
        {
            this.myCPU.bypassing = false;
            button4.BackColor = Color.LightYellow;
            button3.BackColor = resetButton.BackColor;
        }

        //BranchPrediction On
        private void button6_Click(object sender, EventArgs e)
        {
            this.myCPU.branchPrediction = true;
            I.BackColor = Color.LightYellow;
            button5.BackColor = resetButton.BackColor;
        }

        //Branch Prediction Off
        private void button5_Click(object sender, EventArgs e)
        {
            this.myCPU.branchPrediction = false;
            this.myCPU.branchPrediction = true;
            button5.BackColor = Color.LightYellow;
            I.BackColor = resetButton.BackColor;
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
 
 
        

    }
}
