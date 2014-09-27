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

using GeminiCore;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public CPU myCPU;
        int incrementCounter = 1;

        public Form1()
        {
            myCPU = new CPU();

            InitializeComponent();

            this.zeroLabel.Text = "0x" + this.myCPU.ZERO.ToString("X7");
            this.oneLabel.Text = "0x" + this.myCPU.ONE.ToString("X7");

#if DEBUG
            loadFileButton.Text = "Load Assembly";
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
                        var ipe = new IPE(ofd.FileName);
                        List<string> assemblyLines = ipe.ParseFile();
                        if (assemblyLines.Count == 0)
                        {
                            //maybe make a popup here or something
                            return;
                        }
                        short[] binaryLines = ipe.AssemblytoBinary(assemblyLines);
                        Memory.setBinaryInstructions(binaryLines.ToList());
                        ipe.WriteBinarytoFile(binaryLines);
                        currentInstructionLabel.Text = Memory.getAssemblyInstructions().ElementAt(0);
                        totalInstructionCountLabel.Text = (Memory.getAssemblyInstructions().Count).ToString();
                        incrementCounter = 0;
                        currInstructionCountLabel.Text = this.incrementCounter.ToString();
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
            this.myCPU.nextInstruction();
            this.setCPUValuesToView();
            if (incrementCounter < Memory.getAssemblyInstructions().Count)
                this.incrementCounter++;
            currInstructionCountLabel.Text = incrementCounter.ToString();

        }

        public void setCPUValuesToView()
        {
            this.accLabel.Text = "0x" + this.myCPU.ACC.ToString("X7");
            this.aLabel.Text = "0x" + this.myCPU.ACC.ToString("X7");
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
            CPU.executeBinary(0);
            CPU.executeBinary(9472);//sta
            CPU.executeBinary(4352);//lda
            CPU.executeBinary(17152);//add#
            CPU.executeBinary(16896);//add
            CPU.executeBinary(17920);//m
            CPU.executeBinary(25856);//0r#
            CPU.executeBinary(-30720);//bg

        }
    }
}
