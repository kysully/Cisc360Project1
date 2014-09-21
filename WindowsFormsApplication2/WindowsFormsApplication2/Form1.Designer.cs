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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.loadBinaryButton = new System.Windows.Forms.Button();
            this.aLabel = new System.Windows.Forms.Label();
            this.bLabel = new System.Windows.Forms.Label();
            this.zeroLabel = new System.Windows.Forms.Label();
            this.oneLabel = new System.Windows.Forms.Label();
            this.pcLabel = new System.Windows.Forms.Label();
            this.marLabel = new System.Windows.Forms.Label();
            this.mdrLabel = new System.Windows.Forms.Label();
            this.tempLabel = new System.Windows.Forms.Label();
            this.irLabel = new System.Windows.Forms.Label();
            this.ccLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.runAllButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
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
            this.accLabel.Location = new System.Drawing.Point(125, 30);
            this.accLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.accLabel.Name = "accLabel";
            this.accLabel.Size = new System.Drawing.Size(70, 19);
            this.accLabel.TabIndex = 1;
            this.accLabel.Text = "0x0000000";
            this.accLabel.Click += new System.EventHandler(this.accLabel_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(334, 36);
            this.loadFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(100, 23);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Load Assembly";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // nextInstructionButton
            // 
            this.nextInstructionButton.Location = new System.Drawing.Point(234, 287);
            this.nextInstructionButton.Margin = new System.Windows.Forms.Padding(2);
            this.nextInstructionButton.Name = "nextInstructionButton";
            this.nextInstructionButton.Size = new System.Drawing.Size(100, 19);
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
            // loadBinaryButton
            // 
            this.loadBinaryButton.Location = new System.Drawing.Point(234, 36);
            this.loadBinaryButton.Name = "loadBinaryButton";
            this.loadBinaryButton.Size = new System.Drawing.Size(99, 23);
            this.loadBinaryButton.TabIndex = 5;
            this.loadBinaryButton.Text = "Load Binary";
            this.loadBinaryButton.UseVisualStyleBackColor = true;
            // 
            // aLabel
            // 
            this.aLabel.AutoSize = true;
            this.aLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aLabel.Location = new System.Drawing.Point(125, 49);
            this.aLabel.Name = "aLabel";
            this.aLabel.Size = new System.Drawing.Size(70, 16);
            this.aLabel.TabIndex = 13;
            this.aLabel.Text = "0x0000000";
            this.aLabel.Click += new System.EventHandler(this.aLabel_Click);
            // 
            // bLabel
            // 
            this.bLabel.AutoSize = true;
            this.bLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLabel.Location = new System.Drawing.Point(125, 71);
            this.bLabel.Name = "bLabel";
            this.bLabel.Size = new System.Drawing.Size(70, 16);
            this.bLabel.TabIndex = 14;
            this.bLabel.Text = "0x0000000";
            // 
            // zeroLabel
            // 
            this.zeroLabel.AutoSize = true;
            this.zeroLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zeroLabel.Location = new System.Drawing.Point(125, 93);
            this.zeroLabel.Name = "zeroLabel";
            this.zeroLabel.Size = new System.Drawing.Size(70, 16);
            this.zeroLabel.TabIndex = 15;
            this.zeroLabel.Text = "0x0000000";
            // 
            // oneLabel
            // 
            this.oneLabel.AutoSize = true;
            this.oneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oneLabel.Location = new System.Drawing.Point(125, 116);
            this.oneLabel.Name = "oneLabel";
            this.oneLabel.Size = new System.Drawing.Size(70, 16);
            this.oneLabel.TabIndex = 16;
            this.oneLabel.Text = "0x0000000";
            // 
            // pcLabel
            // 
            this.pcLabel.AutoSize = true;
            this.pcLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pcLabel.Location = new System.Drawing.Point(125, 138);
            this.pcLabel.Name = "pcLabel";
            this.pcLabel.Size = new System.Drawing.Size(70, 16);
            this.pcLabel.TabIndex = 17;
            this.pcLabel.Text = "0x0000000";
            // 
            // marLabel
            // 
            this.marLabel.AutoSize = true;
            this.marLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marLabel.Location = new System.Drawing.Point(125, 162);
            this.marLabel.Name = "marLabel";
            this.marLabel.Size = new System.Drawing.Size(70, 16);
            this.marLabel.TabIndex = 18;
            this.marLabel.Text = "0x0000000";
            // 
            // mdrLabel
            // 
            this.mdrLabel.AutoSize = true;
            this.mdrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mdrLabel.Location = new System.Drawing.Point(125, 185);
            this.mdrLabel.Name = "mdrLabel";
            this.mdrLabel.Size = new System.Drawing.Size(70, 16);
            this.mdrLabel.TabIndex = 19;
            this.mdrLabel.Text = "0x0000000";
            // 
            // tempLabel
            // 
            this.tempLabel.AutoSize = true;
            this.tempLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempLabel.Location = new System.Drawing.Point(125, 205);
            this.tempLabel.Name = "tempLabel";
            this.tempLabel.Size = new System.Drawing.Size(70, 16);
            this.tempLabel.TabIndex = 20;
            this.tempLabel.Text = "0x0000000";
            // 
            // irLabel
            // 
            this.irLabel.AutoSize = true;
            this.irLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.irLabel.Location = new System.Drawing.Point(125, 228);
            this.irLabel.Name = "irLabel";
            this.irLabel.Size = new System.Drawing.Size(70, 16);
            this.irLabel.TabIndex = 21;
            this.irLabel.Text = "0x0000000";
            // 
            // ccLabel
            // 
            this.ccLabel.AutoSize = true;
            this.ccLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ccLabel.Location = new System.Drawing.Point(125, 250);
            this.ccLabel.Name = "ccLabel";
            this.ccLabel.Size = new System.Drawing.Size(70, 16);
            this.ccLabel.TabIndex = 22;
            this.ccLabel.Text = "0x0000000";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(138, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(159, 24);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Title Place Holder";
            // 
            // runAllButton
            // 
            this.runAllButton.Location = new System.Drawing.Point(334, 285);
            this.runAllButton.Name = "runAllButton";
            this.runAllButton.Size = new System.Drawing.Size(99, 23);
            this.runAllButton.TabIndex = 7;
            this.runAllButton.Text = "Run All ";
            this.runAllButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 325);
            this.Controls.Add(this.runAllButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.loadBinaryButton);
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
        private System.Windows.Forms.Button loadBinaryButton;
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
    }
}

