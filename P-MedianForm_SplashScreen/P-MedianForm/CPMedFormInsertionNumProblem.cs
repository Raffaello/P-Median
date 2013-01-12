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
    public partial class CPMedFormInsertionNumProblem : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public int M { get; private set; }
        public CPMedFormInsertionNumProblem(int max_m)
        {
            InitializeComponent();
            numericUpDown1.Maximum = (decimal)max_m;
            label1.Text += max_m.ToString();

        }

        private void button1_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            M = (int) numericUpDown1.Value;
        }
    }
}
