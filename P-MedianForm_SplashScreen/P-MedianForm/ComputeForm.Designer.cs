namespace P_MedianForm
{
    partial class ComputeForm
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
            this.components = new System.ComponentModel.Container();
            this.button_compute = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.checkBoxParSol = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxParClu = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownSeed = new System.Windows.Forms.NumericUpDown();
            this.checkBoxVerbose = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_NSol = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxIterSec = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonSC = new System.Windows.Forms.RadioButton();
            this.radioButtonSP = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NSol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxIterSec)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_compute
            // 
            this.button_compute.Location = new System.Drawing.Point(108, 253);
            this.button_compute.Name = "button_compute";
            this.button_compute.Size = new System.Drawing.Size(75, 23);
            this.button_compute.TabIndex = 0;
            this.button_compute.Text = "Compute";
            this.button_compute.UseVisualStyleBackColor = true;
            this.button_compute.Click += new System.EventHandler(this.button_compute_Click);
            // 
            // button_back
            // 
            this.button_back.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_back.Location = new System.Drawing.Point(197, 253);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(75, 23);
            this.button_back.TabIndex = 1;
            this.button_back.Text = "Back";
            this.button_back.UseVisualStyleBackColor = true;
            // 
            // checkBoxParSol
            // 
            this.checkBoxParSol.AutoSize = true;
            this.checkBoxParSol.Checked = true;
            this.checkBoxParSol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxParSol.Location = new System.Drawing.Point(6, 19);
            this.checkBoxParSol.Name = "checkBoxParSol";
            this.checkBoxParSol.Size = new System.Drawing.Size(101, 17);
            this.checkBoxParSol.TabIndex = 2;
            this.checkBoxParSol.Text = "Parallel Solution";
            this.checkBoxParSol.UseVisualStyleBackColor = true;
            this.checkBoxParSol.CheckedChanged += new System.EventHandler(this.checkBoxParSol_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxParClu);
            this.groupBox1.Controls.Add(this.checkBoxParSol);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 71);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parallel Options";
            // 
            // checkBoxParClu
            // 
            this.checkBoxParClu.AutoSize = true;
            this.checkBoxParClu.Checked = true;
            this.checkBoxParClu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxParClu.Location = new System.Drawing.Point(6, 42);
            this.checkBoxParClu.Name = "checkBoxParClu";
            this.checkBoxParClu.Size = new System.Drawing.Size(138, 17);
            this.checkBoxParClu.TabIndex = 3;
            this.checkBoxParClu.Text = "Internal Parallel Clusters";
            this.checkBoxParClu.UseVisualStyleBackColor = true;
            this.checkBoxParClu.CheckedChanged += new System.EventHandler(this.checkBoxParClu_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericUpDownSeed);
            this.groupBox2.Controls.Add(this.checkBoxVerbose);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown_NSol);
            this.groupBox2.Location = new System.Drawing.Point(197, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 109);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Solutions Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Seed";
            // 
            // numericUpDownSeed
            // 
            this.numericUpDownSeed.Location = new System.Drawing.Point(6, 83);
            this.numericUpDownSeed.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownSeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownSeed.Name = "numericUpDownSeed";
            this.numericUpDownSeed.Size = new System.Drawing.Size(122, 20);
            this.numericUpDownSeed.TabIndex = 3;
            this.numericUpDownSeed.ThousandsSeparator = true;
            this.numericUpDownSeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownSeed.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // checkBoxVerbose
            // 
            this.checkBoxVerbose.AutoSize = true;
            this.checkBoxVerbose.Location = new System.Drawing.Point(6, 19);
            this.checkBoxVerbose.Name = "checkBoxVerbose";
            this.checkBoxVerbose.Size = new System.Drawing.Size(65, 17);
            this.checkBoxVerbose.TabIndex = 2;
            this.checkBoxVerbose.Text = "Verbose";
            this.checkBoxVerbose.UseVisualStyleBackColor = true;
            this.checkBoxVerbose.CheckedChanged += new System.EventHandler(this.checkBoxVerbose_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(87, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "N° of Solutions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // numericUpDown_NSol
            // 
            this.numericUpDown_NSol.Location = new System.Drawing.Point(6, 44);
            this.numericUpDown_NSol.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_NSol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_NSol.Name = "numericUpDown_NSol";
            this.numericUpDown_NSol.Size = new System.Drawing.Size(75, 20);
            this.numericUpDown_NSol.TabIndex = 0;
            this.numericUpDown_NSol.ThousandsSeparator = true;
            this.numericUpDown_NSol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_NSol.ValueChanged += new System.EventHandler(this.numericUpDown_NSol_ValueChanged);
            // 
            // numericUpDownMaxIterSec
            // 
            this.numericUpDownMaxIterSec.DecimalPlaces = 2;
            this.numericUpDownMaxIterSec.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMaxIterSec.Location = new System.Drawing.Point(6, 19);
            this.numericUpDownMaxIterSec.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownMaxIterSec.Name = "numericUpDownMaxIterSec";
            this.numericUpDownMaxIterSec.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownMaxIterSec.TabIndex = 5;
            this.numericUpDownMaxIterSec.ValueChanged += new System.EventHandler(this.numericUpDownMaxIterSec_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Max Iter sec";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Set to Zero for infinity time.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDownMaxIterSec);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(197, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(173, 53);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscelanous Options";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonSP);
            this.groupBox4.Controls.Add(this.radioButtonSC);
            this.groupBox4.Location = new System.Drawing.Point(12, 90);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(171, 90);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Problem Type";
            // 
            // radioButtonSC
            // 
            this.radioButtonSC.AutoSize = true;
            this.radioButtonSC.Checked = true;
            this.radioButtonSC.Location = new System.Drawing.Point(6, 31);
            this.radioButtonSC.Name = "radioButtonSC";
            this.radioButtonSC.Size = new System.Drawing.Size(86, 17);
            this.radioButtonSC.TabIndex = 0;
            this.radioButtonSC.TabStop = true;
            this.radioButtonSC.Text = "Set-Covering";
            this.radioButtonSC.UseVisualStyleBackColor = true;
            this.radioButtonSC.CheckedChanged += new System.EventHandler(this.radioButtonSC_CheckedChanged);
            // 
            // radioButtonSP
            // 
            this.radioButtonSP.AutoSize = true;
            this.radioButtonSP.Location = new System.Drawing.Point(6, 54);
            this.radioButtonSP.Name = "radioButtonSP";
            this.radioButtonSP.Size = new System.Drawing.Size(96, 17);
            this.radioButtonSP.TabIndex = 1;
            this.radioButtonSP.Text = "Set-Partitioning";
            this.radioButtonSP.UseVisualStyleBackColor = true;
            this.radioButtonSP.CheckedChanged += new System.EventHandler(this.radioButtonSP_CheckedChanged);
            // 
            // ComputeForm
            // 
            this.AcceptButton = this.button_compute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_back;
            this.ClientSize = new System.Drawing.Size(382, 288);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_compute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComputeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compute Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NSol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxIterSec)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_compute;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.CheckBox checkBoxParSol;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxParClu;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_NSol;
        private System.Windows.Forms.CheckBox checkBoxVerbose;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxIterSec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownSeed;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonSP;
        private System.Windows.Forms.RadioButton radioButtonSC;
    }
}