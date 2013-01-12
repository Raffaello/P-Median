using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace P_MedianForm
{
    public partial class ComputeForm : Form
    {
        public bool AsParSol { get; private set; }
        public bool AsParClu { get; private set; }
        public uint NSol { get; private set; }
        public bool Verbose { get; private set; }
        public double MaxIterSec { get; private set; }
        public int SeedVal { get; private set; }
        public PMedLib.eProblemType ProblemType { get; private set; }
        public ComputeForm()
        {
            InitializeComponent();
            AsParSol = this.checkBoxParSol.Checked;
            AsParClu = this.checkBoxParClu.Checked;
            NSol     = (uint) this.numericUpDown_NSol.Value;         
            Verbose  = this.checkBoxVerbose.Checked;
            MaxIterSec = (double)this.numericUpDownMaxIterSec.Value;
            toolTip1.SetToolTip(numericUpDownMaxIterSec, "Set to Zero for infinity computation.");
            SeedVal = (int)this.numericUpDownSeed.Value;
            ProblemType = PMedLib.eProblemType.SetCovering;
         }


        private void numericUpDown_NSol_ValueChanged(object sender, EventArgs e)
        {
                NSol = (uint)this.numericUpDown_NSol.Value;
        }

        private void button_compute_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void checkBoxParSol_CheckedChanged(object sender, EventArgs e)
        {
            AsParSol =  this.checkBoxParSol.Checked;
        }

        private void checkBoxParClu_CheckedChanged(object sender, EventArgs e)
        {
            AsParClu = this.checkBoxParClu.Checked;
        }

        private void checkBoxVerbose_CheckedChanged(object sender, EventArgs e)
        {
            Verbose = this.checkBoxVerbose.Checked;                       
        }

        private void numericUpDownMaxIterSec_ValueChanged(object sender, EventArgs e)
        {
            MaxIterSec = (double)this.numericUpDownMaxIterSec.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SeedVal = (int)this.numericUpDownSeed.Value;
        }

        private void radioButtonSC_CheckedChanged(object sender, EventArgs e)
        {
            ProblemType = PMedLib.eProblemType.SetCovering;
        }

        private void radioButtonSP_CheckedChanged(object sender, EventArgs e)
        {
            ProblemType = PMedLib.eProblemType.SetPartitioning;
        }
    }
}
