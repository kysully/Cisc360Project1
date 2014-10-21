namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.accLabel = new System.Windows.Forms.Label();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.nextInstructionButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ccLabel = new System.Windows.Forms.Label();
            this.irLabel = new System.Windows.Forms.Label();
            this.tempLabel = new System.Windows.Forms.Label();
            this.mdrLabel = new System.Windows.Forms.Label();
            this.marLabel = new System.Windows.Forms.Label();
            this.pcLabel = new System.Windows.Forms.Label();
            this.oneLabel = new System.Windows.Forms.Label();
            this.zeroLabel = new System.Windows.Forms.Label();
            this.bLabel = new System.Windows.Forms.Label();
            this.aLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.runAllButton = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.totalInstructionCountLabel = new System.Windows.Forms.Label();
            this.currInstructionCountLabel = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.currentInstructionLabel = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.previousInstructionLabel = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.memComboBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.currMemValueLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.readMissLabel = new System.Windows.Forms.Label();
            this.readHitLabel = new System.Windows.Forms.Label();
            this.writeMissLabel = new System.Windows.Forms.Label();
            this.writeHitLabel = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.cacheSizeBox = new System.Windows.Forms.ComboBox();
            this.blockSizeBox = new System.Windows.Forms.ComboBox();
            this.hitOrMissLabel = new System.Windows.Forms.Label();
            this.totalMissesLabel = new System.Windows.Forms.Label();
            this.totalHitsLabel = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.cacheIndexComboBox = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.cacheValueLabel = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.cacheTagLabel = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.spatialHitsLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACC:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // accLabel
            // 
            this.accLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accLabel.Location = new System.Drawing.Point(120, 30);
            this.accLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.accLabel.Name = "accLabel";
            this.accLabel.Size = new System.Drawing.Size(77, 19);
            this.accLabel.TabIndex = 1;
            this.accLabel.Text = "0";
            this.accLabel.Click += new System.EventHandler(this.accLabel_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(407, 35);
            this.loadFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(100, 25);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // nextInstructionButton
            // 
            this.nextInstructionButton.Location = new System.Drawing.Point(234, 282);
            this.nextInstructionButton.Margin = new System.Windows.Forms.Padding(2);
            this.nextInstructionButton.Name = "nextInstructionButton";
            this.nextInstructionButton.Size = new System.Drawing.Size(100, 25);
            this.nextInstructionButton.TabIndex = 3;
            this.nextInstructionButton.Text = "Next";
            this.nextInstructionButton.UseVisualStyleBackColor = true;
            this.nextInstructionButton.Click += new System.EventHandler(this.nextInstructionButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.ccLabel);
            this.panel1.Controls.Add(this.irLabel);
            this.panel1.Controls.Add(this.tempLabel);
            this.panel1.Controls.Add(this.mdrLabel);
            this.panel1.Controls.Add(this.marLabel);
            this.panel1.Controls.Add(this.pcLabel);
            this.panel1.Controls.Add(this.oneLabel);
            this.panel1.Controls.Add(this.zeroLabel);
            this.panel1.Controls.Add(this.bLabel);
            this.panel1.Controls.Add(this.aLabel);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.accLabel);
            this.panel1.Location = new System.Drawing.Point(14, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 270);
            this.panel1.TabIndex = 4;
            // 
            // ccLabel
            // 
            this.ccLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ccLabel.Location = new System.Drawing.Point(120, 250);
            this.ccLabel.Name = "ccLabel";
            this.ccLabel.Size = new System.Drawing.Size(77, 16);
            this.ccLabel.TabIndex = 22;
            this.ccLabel.Text = "0";
            // 
            // irLabel
            // 
            this.irLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.irLabel.Location = new System.Drawing.Point(120, 228);
            this.irLabel.Name = "irLabel";
            this.irLabel.Size = new System.Drawing.Size(77, 16);
            this.irLabel.TabIndex = 21;
            this.irLabel.Text = "0";
            // 
            // tempLabel
            // 
            this.tempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempLabel.Location = new System.Drawing.Point(120, 205);
            this.tempLabel.Name = "tempLabel";
            this.tempLabel.Size = new System.Drawing.Size(77, 16);
            this.tempLabel.TabIndex = 20;
            this.tempLabel.Text = "0";
            this.tempLabel.Click += new System.EventHandler(this.tempLabel_Click);
            // 
            // mdrLabel
            // 
            this.mdrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mdrLabel.Location = new System.Drawing.Point(120, 185);
            this.mdrLabel.Name = "mdrLabel";
            this.mdrLabel.Size = new System.Drawing.Size(77, 16);
            this.mdrLabel.TabIndex = 19;
            this.mdrLabel.Text = "0";
            // 
            // marLabel
            // 
            this.marLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marLabel.Location = new System.Drawing.Point(120, 162);
            this.marLabel.Name = "marLabel";
            this.marLabel.Size = new System.Drawing.Size(77, 16);
            this.marLabel.TabIndex = 18;
            this.marLabel.Text = "0";
            // 
            // pcLabel
            // 
            this.pcLabel.AutoSize = true;
            this.pcLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pcLabel.Location = new System.Drawing.Point(120, 138);
            this.pcLabel.Name = "pcLabel";
            this.pcLabel.Size = new System.Drawing.Size(15, 16);
            this.pcLabel.TabIndex = 17;
            this.pcLabel.Text = "0";
            this.pcLabel.Click += new System.EventHandler(this.pcLabel_Click);
            // 
            // oneLabel
            // 
            this.oneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oneLabel.Location = new System.Drawing.Point(120, 116);
            this.oneLabel.Name = "oneLabel";
            this.oneLabel.Size = new System.Drawing.Size(77, 16);
            this.oneLabel.TabIndex = 16;
            this.oneLabel.Text = "0";
            // 
            // zeroLabel
            // 
            this.zeroLabel.AutoSize = true;
            this.zeroLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zeroLabel.Location = new System.Drawing.Point(120, 93);
            this.zeroLabel.Name = "zeroLabel";
            this.zeroLabel.Size = new System.Drawing.Size(15, 16);
            this.zeroLabel.TabIndex = 15;
            this.zeroLabel.Text = "0";
            // 
            // bLabel
            // 
            this.bLabel.AutoSize = true;
            this.bLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLabel.Location = new System.Drawing.Point(120, 71);
            this.bLabel.Name = "bLabel";
            this.bLabel.Size = new System.Drawing.Size(15, 16);
            this.bLabel.TabIndex = 14;
            this.bLabel.Text = "0";
            this.bLabel.Click += new System.EventHandler(this.bLabel_Click);
            // 
            // aLabel
            // 
            this.aLabel.AutoSize = true;
            this.aLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aLabel.Location = new System.Drawing.Point(120, 49);
            this.aLabel.Name = "aLabel";
            this.aLabel.Size = new System.Drawing.Size(15, 16);
            this.aLabel.TabIndex = 13;
            this.aLabel.Text = "0";
            this.aLabel.Click += new System.EventHandler(this.aLabel_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(134, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Value";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 250);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 16);
            this.label12.TabIndex = 11;
            this.label12.Text = "CC:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 228);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 16);
            this.label11.TabIndex = 10;
            this.label11.Text = "IR:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 16);
            this.label10.TabIndex = 9;
            this.label10.Text = "TEMP:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 185);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "MDR:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "MAR:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "PC:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "One:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Zero:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "B:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Register";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "A:";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(338, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(153, 24);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Gemini Simulator";
            // 
            // runAllButton
            // 
            this.runAllButton.Location = new System.Drawing.Point(334, 282);
            this.runAllButton.Name = "runAllButton";
            this.runAllButton.Size = new System.Drawing.Size(100, 25);
            this.runAllButton.TabIndex = 7;
            this.runAllButton.Text = "Run All ";
            this.runAllButton.UseVisualStyleBackColor = true;
            this.runAllButton.Click += new System.EventHandler(this.runAllButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(270, 264);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "Instruction Option:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.fileNameLabel);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.totalInstructionCountLabel);
            this.panel2.Controls.Add(this.currInstructionCountLabel);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.currentInstructionLabel);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.previousInstructionLabel);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Location = new System.Drawing.Point(233, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 172);
            this.panel2.TabIndex = 9;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(54, 14);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(16, 13);
            this.fileNameLabel.TabIndex = 9;
            this.fileNameLabel.Text = "...";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(20, 10);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(34, 17);
            this.label22.TabIndex = 8;
            this.label22.Text = "File:";
            // 
            // totalInstructionCountLabel
            // 
            this.totalInstructionCountLabel.AutoSize = true;
            this.totalInstructionCountLabel.Location = new System.Drawing.Point(150, 156);
            this.totalInstructionCountLabel.Name = "totalInstructionCountLabel";
            this.totalInstructionCountLabel.Size = new System.Drawing.Size(13, 13);
            this.totalInstructionCountLabel.TabIndex = 7;
            this.totalInstructionCountLabel.Text = "0";
            this.totalInstructionCountLabel.Click += new System.EventHandler(this.totalInstructionCountLabel_Click);
            // 
            // currInstructionCountLabel
            // 
            this.currInstructionCountLabel.AutoSize = true;
            this.currInstructionCountLabel.Location = new System.Drawing.Point(106, 156);
            this.currInstructionCountLabel.Name = "currInstructionCountLabel";
            this.currInstructionCountLabel.Size = new System.Drawing.Size(13, 13);
            this.currInstructionCountLabel.TabIndex = 6;
            this.currInstructionCountLabel.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(128, 156);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(16, 13);
            this.label19.TabIndex = 5;
            this.label19.Text = "of";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 156);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(90, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "Instruction Count:";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // currentInstructionLabel
            // 
            this.currentInstructionLabel.AutoSize = true;
            this.currentInstructionLabel.Location = new System.Drawing.Point(20, 113);
            this.currentInstructionLabel.Name = "currentInstructionLabel";
            this.currentInstructionLabel.Size = new System.Drawing.Size(103, 13);
            this.currentInstructionLabel.TabIndex = 3;
            this.currentInstructionLabel.Text = "--------------------------------";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(20, 89);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(129, 15);
            this.label17.TabIndex = 2;
            this.label17.Text = "Current Instruction:";
            // 
            // previousInstructionLabel
            // 
            this.previousInstructionLabel.AutoSize = true;
            this.previousInstructionLabel.Location = new System.Drawing.Point(20, 67);
            this.previousInstructionLabel.Name = "previousInstructionLabel";
            this.previousInstructionLabel.Size = new System.Drawing.Size(103, 13);
            this.previousInstructionLabel.TabIndex = 1;
            this.previousInstructionLabel.Text = "--------------------------------";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(20, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(137, 15);
            this.label16.TabIndex = 0;
            this.label16.Text = "Previous Instruction:";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(288, 66);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 16);
            this.label15.TabIndex = 10;
            this.label15.Text = "Instructions:";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(301, 35);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(100, 25);
            this.resetButton.TabIndex = 11;
            this.resetButton.Text = "Reset CPU ";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // memComboBox
            // 
            this.memComboBox.FormattingEnabled = true;
            this.memComboBox.Location = new System.Drawing.Point(778, 173);
            this.memComboBox.Name = "memComboBox";
            this.memComboBox.Size = new System.Drawing.Size(45, 21);
            this.memComboBox.TabIndex = 12;
            this.memComboBox.SelectedIndexChanged += new System.EventHandler(this.memComboBox_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(702, 177);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(67, 13);
            this.label20.TabIndex = 13;
            this.label20.Text = "Stack Index:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(701, 201);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 13);
            this.label21.TabIndex = 14;
            this.label21.Text = "Stack Value:";
            // 
            // currMemValueLabel
            // 
            this.currMemValueLabel.AutoSize = true;
            this.currMemValueLabel.Location = new System.Drawing.Point(779, 201);
            this.currMemValueLabel.Name = "currMemValueLabel";
            this.currMemValueLabel.Size = new System.Drawing.Size(13, 13);
            this.currMemValueLabel.TabIndex = 15;
            this.currMemValueLabel.Text = "0";
            this.currMemValueLabel.Click += new System.EventHandler(this.currMemValueLabel_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel3.Controls.Add(this.spatialHitsLabel);
            this.panel3.Controls.Add(this.label38);
            this.panel3.Controls.Add(this.readMissLabel);
            this.panel3.Controls.Add(this.readHitLabel);
            this.panel3.Controls.Add(this.writeMissLabel);
            this.panel3.Controls.Add(this.writeHitLabel);
            this.panel3.Controls.Add(this.label34);
            this.panel3.Controls.Add(this.label33);
            this.panel3.Controls.Add(this.label32);
            this.panel3.Controls.Add(this.label31);
            this.panel3.Controls.Add(this.cacheSizeBox);
            this.panel3.Controls.Add(this.blockSizeBox);
            this.panel3.Controls.Add(this.hitOrMissLabel);
            this.panel3.Controls.Add(this.totalMissesLabel);
            this.panel3.Controls.Add(this.totalHitsLabel);
            this.panel3.Controls.Add(this.label30);
            this.panel3.Controls.Add(this.label29);
            this.panel3.Controls.Add(this.label26);
            this.panel3.Controls.Add(this.label25);
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.label23);
            this.panel3.Location = new System.Drawing.Point(442, 85);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(254, 172);
            this.panel3.TabIndex = 10;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // readMissLabel
            // 
            this.readMissLabel.AutoSize = true;
            this.readMissLabel.Location = new System.Drawing.Point(221, 92);
            this.readMissLabel.Name = "readMissLabel";
            this.readMissLabel.Size = new System.Drawing.Size(22, 13);
            this.readMissLabel.TabIndex = 24;
            this.readMissLabel.Text = "-----";
            // 
            // readHitLabel
            // 
            this.readHitLabel.AutoSize = true;
            this.readHitLabel.Location = new System.Drawing.Point(221, 70);
            this.readHitLabel.Name = "readHitLabel";
            this.readHitLabel.Size = new System.Drawing.Size(22, 13);
            this.readHitLabel.TabIndex = 23;
            this.readHitLabel.Text = "-----";
            // 
            // writeMissLabel
            // 
            this.writeMissLabel.AutoSize = true;
            this.writeMissLabel.Location = new System.Drawing.Point(221, 44);
            this.writeMissLabel.Name = "writeMissLabel";
            this.writeMissLabel.Size = new System.Drawing.Size(22, 13);
            this.writeMissLabel.TabIndex = 22;
            this.writeMissLabel.Text = "-----";
            // 
            // writeHitLabel
            // 
            this.writeHitLabel.AutoSize = true;
            this.writeHitLabel.Location = new System.Drawing.Point(221, 14);
            this.writeHitLabel.Name = "writeHitLabel";
            this.writeHitLabel.Size = new System.Drawing.Size(22, 13);
            this.writeHitLabel.TabIndex = 21;
            this.writeHitLabel.Text = "-----";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(10, 80);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(49, 13);
            this.label34.TabIndex = 20;
            this.label34.Text = "Hit/Miss:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(10, 133);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(69, 13);
            this.label33.TabIndex = 19;
            this.label33.Text = "Total Misses:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(156, 43);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(59, 13);
            this.label32.TabIndex = 18;
            this.label32.Text = "Write Miss:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(156, 14);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(51, 13);
            this.label31.TabIndex = 17;
            this.label31.Text = "Write Hit:";
            // 
            // cacheSizeBox
            // 
            this.cacheSizeBox.FormattingEnabled = true;
            this.cacheSizeBox.Location = new System.Drawing.Point(87, 11);
            this.cacheSizeBox.Name = "cacheSizeBox";
            this.cacheSizeBox.Size = new System.Drawing.Size(45, 21);
            this.cacheSizeBox.TabIndex = 16;
            this.cacheSizeBox.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // blockSizeBox
            // 
            this.blockSizeBox.FormattingEnabled = true;
            this.blockSizeBox.Location = new System.Drawing.Point(87, 43);
            this.blockSizeBox.Name = "blockSizeBox";
            this.blockSizeBox.Size = new System.Drawing.Size(45, 21);
            this.blockSizeBox.TabIndex = 15;
            this.blockSizeBox.SelectedIndexChanged += new System.EventHandler(this.blockSizeBox_SelectedIndexChanged);
            // 
            // hitOrMissLabel
            // 
            this.hitOrMissLabel.AutoSize = true;
            this.hitOrMissLabel.Location = new System.Drawing.Point(89, 80);
            this.hitOrMissLabel.Name = "hitOrMissLabel";
            this.hitOrMissLabel.Size = new System.Drawing.Size(43, 13);
            this.hitOrMissLabel.TabIndex = 12;
            this.hitOrMissLabel.Text = "------------";
            this.hitOrMissLabel.Click += new System.EventHandler(this.label33_Click);
            // 
            // totalMissesLabel
            // 
            this.totalMissesLabel.AutoSize = true;
            this.totalMissesLabel.Location = new System.Drawing.Point(89, 133);
            this.totalMissesLabel.Name = "totalMissesLabel";
            this.totalMissesLabel.Size = new System.Drawing.Size(43, 13);
            this.totalMissesLabel.TabIndex = 13;
            this.totalMissesLabel.Text = "------------";
            this.totalMissesLabel.Click += new System.EventHandler(this.label34_Click);
            // 
            // totalHitsLabel
            // 
            this.totalHitsLabel.AutoSize = true;
            this.totalHitsLabel.Location = new System.Drawing.Point(89, 104);
            this.totalHitsLabel.Name = "totalHitsLabel";
            this.totalHitsLabel.Size = new System.Drawing.Size(43, 13);
            this.totalHitsLabel.TabIndex = 14;
            this.totalHitsLabel.Text = "------------";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(155, 92);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(60, 13);
            this.label30.TabIndex = 5;
            this.label30.Text = "Read Miss:";
            this.label30.Click += new System.EventHandler(this.label30_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(10, 104);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(55, 13);
            this.label29.TabIndex = 4;
            this.label29.Text = "Total Hits:";
            this.label29.Click += new System.EventHandler(this.label29_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(156, 70);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(52, 13);
            this.label26.TabIndex = 3;
            this.label26.Text = "Read Hit:";
            this.label26.Click += new System.EventHandler(this.label26_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(0, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(0, 13);
            this.label25.TabIndex = 2;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(10, 46);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(60, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Block Size:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 14);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(64, 13);
            this.label23.TabIndex = 0;
            this.label23.Text = "Cache Size:";
            this.label23.Click += new System.EventHandler(this.label23_Click_1);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(518, 66);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(56, 16);
            this.label27.TabIndex = 17;
            this.label27.Text = "Cache:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(498, 264);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(105, 16);
            this.label28.TabIndex = 18;
            this.label28.Text = "Cache Option:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(549, 282);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 25);
            this.button1.TabIndex = 19;
            this.button1.Text = "2-Way";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(445, 282);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 25);
            this.button2.TabIndex = 20;
            this.button2.Text = "1-Way";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(702, 87);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(70, 13);
            this.label35.TabIndex = 21;
            this.label35.Text = "Cache Index:";
            // 
            // cacheIndexComboBox
            // 
            this.cacheIndexComboBox.FormattingEnabled = true;
            this.cacheIndexComboBox.Location = new System.Drawing.Point(778, 85);
            this.cacheIndexComboBox.Name = "cacheIndexComboBox";
            this.cacheIndexComboBox.Size = new System.Drawing.Size(43, 21);
            this.cacheIndexComboBox.TabIndex = 22;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(702, 110);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(71, 13);
            this.label36.TabIndex = 23;
            this.label36.Text = "Cache Value:";
            // 
            // cacheValueLabel
            // 
            this.cacheValueLabel.AutoSize = true;
            this.cacheValueLabel.Location = new System.Drawing.Point(779, 110);
            this.cacheValueLabel.Name = "cacheValueLabel";
            this.cacheValueLabel.Size = new System.Drawing.Size(13, 13);
            this.cacheValueLabel.TabIndex = 24;
            this.cacheValueLabel.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(702, 132);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(115, 13);
            this.label37.TabIndex = 25;
            this.label37.Text = "Actual Index on Stack:";
            this.label37.Click += new System.EventHandler(this.label37_Click);
            // 
            // cacheTagLabel
            // 
            this.cacheTagLabel.AutoSize = true;
            this.cacheTagLabel.Location = new System.Drawing.Point(823, 132);
            this.cacheTagLabel.Name = "cacheTagLabel";
            this.cacheTagLabel.Size = new System.Drawing.Size(13, 13);
            this.cacheTagLabel.TabIndex = 26;
            this.cacheTagLabel.Text = "0";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(155, 116);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(63, 13);
            this.label38.TabIndex = 25;
            this.label38.Text = "Spatial Hits:";
            this.label38.Click += new System.EventHandler(this.label38_Click);
            // 
            // spatialHitsLabel
            // 
            this.spatialHitsLabel.AutoSize = true;
            this.spatialHitsLabel.Location = new System.Drawing.Point(221, 116);
            this.spatialHitsLabel.Name = "spatialHitsLabel";
            this.spatialHitsLabel.Size = new System.Drawing.Size(22, 13);
            this.spatialHitsLabel.TabIndex = 26;
            this.spatialHitsLabel.Text = "-----";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 382);
            this.Controls.Add(this.cacheTagLabel);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.cacheValueLabel);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.cacheIndexComboBox);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.currMemValueLabel);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.memComboBox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.runAllButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.nextInstructionButton);
            this.Controls.Add(this.loadFileButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Gemini Simulator ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label accLabel;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.Button nextInstructionButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label aLabel;
        private System.Windows.Forms.Label bLabel;
        private System.Windows.Forms.Label ccLabel;
        private System.Windows.Forms.Label irLabel;
        private System.Windows.Forms.Label tempLabel;
        private System.Windows.Forms.Label mdrLabel;
        private System.Windows.Forms.Label marLabel;
        private System.Windows.Forms.Label pcLabel;
        private System.Windows.Forms.Label oneLabel;
        private System.Windows.Forms.Label zeroLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button runAllButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label currentInstructionLabel;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label previousInstructionLabel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label totalInstructionCountLabel;
        private System.Windows.Forms.Label currInstructionCountLabel;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ComboBox memComboBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label currMemValueLabel;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label hitOrMissLabel;
        private System.Windows.Forms.Label totalMissesLabel;
        private System.Windows.Forms.Label totalHitsLabel;
        private System.Windows.Forms.ComboBox cacheSizeBox;
        private System.Windows.Forms.ComboBox blockSizeBox;
        private System.Windows.Forms.Label readMissLabel;
        private System.Windows.Forms.Label readHitLabel;
        private System.Windows.Forms.Label writeMissLabel;
        private System.Windows.Forms.Label writeHitLabel;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox cacheIndexComboBox;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label cacheValueLabel;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label cacheTagLabel;
        private System.Windows.Forms.Label spatialHitsLabel;
        private System.Windows.Forms.Label label38;
    }
}

