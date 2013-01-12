using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSplashScreenBlend;
using PMedLib;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;



namespace P_MedianForm
{
    public partial class Form1 : Form
    {
        private SplashWindow sw;
        private PMed3 pm;

        private PMed.WriteDelegate WriteTextCallBack;
        private delegate void _ConsoleAppendTextCallBack(String str);
        private _ConsoleAppendTextCallBack ConsoleAppendTextCallBack;
        private delegate void _UpdateStatusBarCallBack();
        private _UpdateStatusBarCallBack UpdateStatusBarCallBack;
        private delegate void _UpdateSolutionVieverCallBack(uint index);
        private _UpdateSolutionVieverCallBack UpdateSolutionVieverCallBack;
        private delegate void _SolutionViewerClearCallBack();
        private _SolutionViewerClearCallBack SolutionViewerClearCallBack;

        private class backgroundWorkerParameter
        {
            public bool Parallel { get; set; }
            public bool Clusters { get; set; }
        }

        private const String solutio_str = "Solution";
        private const String cluster_str = "Clusters";
        private const String wrapper_str = "Wrapper";
        private const string TitleForm = "P-Median Project";

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            sw = new SplashWindow(true, true,true);
            sw.Show();
           
            computeToolStripMenuItem.Enabled = false;
            WriteTextCallBack = new PMed.WriteDelegate(ConsoleAppendText);
            ConsoleAppendTextCallBack = new _ConsoleAppendTextCallBack(ConsoleAppendText);
            UpdateStatusBarCallBack = new _UpdateStatusBarCallBack(UpdateStatusBar);
            UpdateSolutionVieverCallBack = new _UpdateSolutionVieverCallBack(UpdateSolutionViever);
            SolutionViewerClearCallBack = new _SolutionViewerClearCallBack(SolutionViewerClear);

            try
            {
                pm = new PMed3(WriteTextCallBack, 1, "PMedProb", false, false);
                pm.Progress += OnProgressReport;
            }
            catch (DllNotFoundException e)
            {
                MessageBox.Show(String.Format("Dll CoinOR not found!!!\r\nCannot Compute Set Covering (Step 3)\r\n\r\n{0}",e), "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(e.Message);
                this.Close();
            }
       }


        private void OnProgressReport(uint index)
        {
            if (statusStrip1.InvokeRequired)
                statusStrip1.Invoke(UpdateStatusBarCallBack);
            else UpdateStatusBar();

            //se si fa qui rallenta la computazione in parallelo....
            //if (SolutionViewer.InvokeRequired)
            //    SolutionViewer.Invoke(UpdateSolutionVieverCallBack, index);
            //else
            //    UpdateSolutionViever(index);
        }

        private void UpdateSolutionViever(uint index)
        {
            TreeNode node = new TreeNode(solutio_str + " " + index.ToString());
            for (uint i = 0; i < pm.p; i++)
                node.Nodes.Add(cluster_str + " " + i.ToString());
            
                this.SolutionViewer.Nodes.Add(node);
            
        }

        private void UpdateStatusBar()
        {
            if (this.toolStripProgressBar1.Value < this.toolStripProgressBar1.Maximum)
                this.toolStripProgressBar1.Value++;
            //qui sotto non lo fa mai...
            //else 
            //{
            //    for (uint i = 0; i < pm.Nsolution; i++)
            //    {
            //        if (SolutionViewer.InvokeRequired)
            //            SolutionViewer.Invoke(UpdateSolutionVieverCallBack, i);
            //        else
            //            UpdateSolutionViever(i);
            //    }
            //}
        }

        private void SolutionViewerClear()
        {
            this.SolutionViewer.Nodes.Clear();
        }

        //private void ConsoleAppendText(String str)
        //{
        //    this.ConsoleTextBox.ConsoleAppendText(str);
        //}

        private void ConsoleAppendText(String str)
        {
            if (this.ConsoleTextBox.InvokeRequired)
                this.Invoke(ConsoleAppendTextCallBack, str);
            else
                ConsoleTextBox.AppendText(str);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if((sw!=null)&&(!sw.IsFadedOut))
                e.Cancel=true;
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("You have to wait until the computation is finished!", "You cannot quit now!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            else
            {
                //if ((sw != null) && (!sw.IsFadedOut))
                //{
                //    if (sw.IsSplashScreen)
                //    {
                //        while (!sw.IsFadedOut)
                //        {
                //            Thread.Sleep(100);
                //        }
                //    }
                //    else 
                //        sw.Close();
                //}
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(1000,"P-Median Project","By Raffaello Bertini",ToolTipIcon.Info);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sw = new SplashWindow(true, true,false);
            sw.Topmost = false;
            sw.ShowInTaskbar = true;
            sw.Title = "AboutBox";
            sw.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //questa sotto è per debuggure e testare l'algoritmo con "test creati ad hoc" !!!
                //quindi lasciarla commentata a meno che non si deva testare casi particolari per algoritmo, il file è strutturato diversamente.
                //if (!pm.LoadTest(openFileDialog1.FileName))
                int M=0;
                if (!pm.Load(openFileDialog1.FileName,ref M))
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not a valid P-Median file.", "Load Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    notifyIcon1.ShowBalloonTip(500, "P-Median Project", "File not loaded!", ToolTipIcon.Error);
                    this.showMatrixCostToolStripMenuItem.Enabled = false;
                }
                else
                {
                    if (M >= 0)
                    {
                        CPMedFormInsertionNumProblem ChooseMForm = new CPMedFormInsertionNumProblem(M);
                        if (ChooseMForm.ShowDialog() == DialogResult.OK)
                        {
                            SolutionViewer.Nodes.Clear();
                            this.ClusterTextBox.Clear();
                            this.ClusterTextBox.AppendText("P-Median Project");
                            int m = ChooseMForm.M;

                            if (!pm.Load(openFileDialog1.FileName, ref m, false))
                            {
                                MessageBox.Show(openFileDialog1.FileName + " is not a valid P-Median file.", "Load Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                notifyIcon1.ShowBalloonTip(500, "P-Median Project", "File not loaded!", ToolTipIcon.Error);
                                this.showMatrixCostToolStripMenuItem.Enabled = false;
                                return;
                            }
                            else
                                this.Text = TitleForm +" - "+ openFileDialog1.SafeFileName + " - m="+m.ToString();
                        }
                        else
                            return;
                    }
                    else
                    {
                        SolutionViewer.Nodes.Clear();
                        this.ClusterTextBox.Clear();
                        this.ClusterTextBox.AppendText("P-Median Project");
                        this.Text = TitleForm + " - " + openFileDialog1.SafeFileName;

                    }
                    computeToolStripMenuItem.Enabled = true;
                    notifyIcon1.ShowBalloonTip(500, "P-Median Project", "File loaded.", ToolTipIcon.Info);
                    this.showMatrixCostToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = TitleForm;
        }

        private void computeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputeForm cf = new ComputeForm();
            if (cf.ShowDialog() == DialogResult.OK)
            {
                this.UseWaitCursor = true;
                pm.SetSolutions(cf.NSol);
                pm.Seed = cf.SeedVal;
                pm.ProblemType = cf.ProblemType;
                pm.MaxIterSecond = cf.MaxIterSec;
                pm.Verbose = cf.Verbose;

                ConsoleAppendText("\r\nStarting computation...");
                ConsoleAppendText("\r\nNumber of solution          : " + pm.Nsolution);
                ConsoleAppendText("\r\nCompute As Parallel         : " + cf.AsParSol.ToString());
                ConsoleAppendText("\r\nRefine Clusters As Parallel : " + cf.AsParClu.ToString());
                ConsoleAppendText("\r\nVerbose                     : " + pm.Verbose.ToString() + "\r\n");

                backgroundWorkerParameter param = new backgroundWorkerParameter();
                param.Parallel = cf.AsParSol;
                param.Clusters = cf.AsParClu;
                computeToolStripMenuItem.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                viewToolStripMenuItem.Enabled = false;
                toolStripProgressBar1.Maximum = (int)pm.Nsolution;
                toolStripProgressBar1.Value = 0;
                SolutionViewer.Nodes.Clear();
                ClusterTextBox.Text = "";
                SolutionViewer.Enabled = false;
                SolutionViewer.Visible = false;
                toolStripStatusLabel1.Enabled = false;
                backgroundWorker1.RunWorkerAsync(param);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorkerParameter param = e.Argument as backgroundWorkerParameter;

            if (pm is PMed3)
            {
                if (!pm.ComputeSolutions(param.Parallel, param.Clusters))
                {
                    MessageBox.Show("An Error as occured, check Console Text Box for info.\r\n Maybe out of memory exception.", "PMedLib.dll error");
                    
                    //MessageBox.Show("It's Better Close program and restart it!!!", "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
 //               else //tutto ok...
 //               {
 //               }
            }
            else
                pm.ComputeSolutions(param.Parallel, param.Clusters);

            //metto qui l'aggiornamenti della form e vari...

            ConsoleAppendText("\r\nFinished Computing Solutions...");
            
            ConsoleAppendText(String.Format("\r\n--- Best    Solution is {0} = {1}", pm.BestSolutionIndex, pm.Solutions[pm.BestSolutionIndex]));
            if (pm.WrapperSolution > 0)
                ConsoleAppendText(String.Format("\r\n--- Wrapper Solution is     = {0}", pm.WrapperSolution));
            pm.ShowElapsedTime();

            ConsoleAppendText(String.Format("\r\nPopulating solution viewer..."));
            Stopwatch solw = new Stopwatch();
            solw.Start();
            //aggiorno la lista delle soluzioni alla fine... così da rendere l'algortimo più efficente...
            
            try
            {
                for (uint i = 0; i < pm.Nsolution; i++)
                {
                    if (SolutionViewer.InvokeRequired)
                        SolutionViewer.Invoke(UpdateSolutionVieverCallBack, i);
                    else
                        UpdateSolutionViever(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PMedian Project - Solution Viewer");
                //cancello la lista altrimenti può crashare tutto!!!
                //e metto solo il meglio trovato...
                if (SolutionViewer.InvokeRequired)
                {
                    SolutionViewer.Invoke(SolutionViewerClearCallBack);
                    SolutionViewer.Invoke(UpdateSolutionVieverCallBack, pm.BestSolutionIndex);
                }
                else
                {
                    SolutionViewer.Nodes.Clear();
                    UpdateSolutionViever(pm.BestSolutionIndex);
                }
            }

            solw.Stop();
            ConsoleAppendText(String.Format("\r\nTime Elapsed Pouplation Solution Viewer {0}", solw.Elapsed));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            //ConsoleAppendText("\r\nFinished Computing Solutions...");
            //computeToolStripMenuItem.Enabled = true;
            //openToolStripMenuItem.Enabled = true;
            //ConsoleAppendText(String.Format("\r\n--- Best    Solution is {0} = {1}", pm.BestSolutionIndex, pm.Solutions[pm.BestSolutionIndex]));
            //if(pm.WrapperSolution>0)
            //    ConsoleAppendText(String.Format("\r\n--- Wrapper Solution is     = {0}", pm.WrapperSolution));
            //pm.ShowElapsedTime();
            
            //ConsoleAppendText(String.Format("\r\nPopulating solution viewer..."));
            //Stopwatch solw = new Stopwatch();
            //solw.Start();
            ////aggiorno la lista delle soluzioni alla fine... così da rendere l'algortimo più efficente...
            //try
            //{
            //    for (uint i = 0; i < pm.Nsolution; i++)
            //    {
            //        if (SolutionViewer.InvokeRequired)
            //            SolutionViewer.Invoke(UpdateSolutionVieverCallBack, i);
            //        else
            //            UpdateSolutionViever(i);
            //    }
            //    if (pm.WrapperSolution > 0)
            //        SolutionViewerSetUpBestSolution((pm.WrapperSolution <= /*pm.Solutions[pm.BestSolutionIndex]*/ pm.solution) ? true : false);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message,"PMedian Project - Solution Viewer");
            //    //cancello la lista altrimenti può crashare tutto!!!
            //    SolutionViewer.Nodes.Clear();
            //    //e metto solo il meglio trovato...
            //    UpdateSolutionViever(pm.BestSolutionIndex);
            //    SolutionViewerSetUpBestSolution(pm.WrapperSolution>0?true:false);
            //}

            //solw.Stop();
            //ConsoleAppendText(String.Format("\r\nTime Elapsed Pouplation Solution Viewer {0}",solw.Elapsed));

            if (pm.WrapperSolution > 0)
                SolutionViewerSetUpBestSolution((pm.WrapperSolution <= /*pm.Solutions[pm.BestSolutionIndex]*/ pm.solution) ? true : false);
            computeToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
            this.viewToolStripMenuItem.Enabled = true;
            this.showTableauToolStripMenuItem.Enabled = true;
            this.showPOfSolutionsToolStripMenuItem.Enabled = true;
            this.showCCSToolStripMenuItem.Enabled = true;
            this.UseWaitCursor = false;
            SolutionViewer.Enabled = true;
            SolutionViewer.Visible = true;
            toolStripStatusLabel1.Enabled = true;
        }

        private void SolutionViewerSetUpBestSolution(bool wrapper=false)
        {
            TreeNodeCollection nodes = SolutionViewer.Nodes;
            String str = solutio_str + " " + pm.BestSolutionIndex;
            int i;
            Font font = new Font(SolutionViewer.Font, FontStyle.Bold);

            //aggiungo anche la sol del wrapper se esite...
            if (pm is PMed3)
            {
                if (pm.WrapperSolutionClusters != null)
                {
                    TreeNode node = new TreeNode("Solution Wrapper");
                    if (wrapper)
                        node.NodeFont = font;
                    for (i = 0; i < pm.p; i++)
                    {
                        node.Nodes.Add(cluster_str + " " + i.ToString());
                    }

                    SolutionViewer.Nodes.Insert(0, node);

                }
            }

            if (!wrapper)
            {
                for (i = 0; i < nodes.Count; i++)
                    if (nodes[i].Text == str)
                        break;
                if (i < nodes.Count)
                //if (nodes.Length == 1)
                {
                    //Font font = new Font(nodes[i].NodeFont.FontFamily, nodes[i].NodeFont.Size, FontStyle.Bold);
                    nodes[i].NodeFont = font;
                    nodes[i].Text = str;
                }
                else
                    MessageBox.Show("Error in SolutionViewerSetUpBestSolution()", "c'è più di un nodo!!");
            }
           
            SolutionViewer.Refresh();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            ConsoleTextBox.Text = "";
        }

        private void SolutionViewer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //node.Level==0 soluzione
            //node.level==1 cluster

            TreeNode node = e.Node;
            String str;
            String[] nodeText = node.Text.Split(' ');
            String[] str2;
            uint index, index2;
            bool wrap=false; //false perchè se è vera la prima cond, non fa la seconda e se è vera la prima la seconda è per forza falsa.

            if((uint.TryParse(nodeText[1], out index))||
                (wrap = (nodeText[1] == "Wrapper")))
            { 
                str = node.Text + " = ";
                switch (nodeText[0])
                {
                    case solutio_str :
                        if (wrap)
                            str += pm.WrapperSolution.ToString();
                        else
                            str += pm.Solutions[index].ToString();
                    break;

                    case cluster_str :
                            str2 = node.Parent.Text.Split(' ');
                            wrap = (str2[1] == "Wrapper");
                            if (wrap)
                            {
                                str += pm.ComputeWrapperClusterCost(index).ToString() +
                                    " --- Length = " + pm.WrapperSolutionClusters[index].Count +
                                    //"\r\nRefined Solution Gap : " + pm.GapClusters[index2][index] +
                                    "\r\nNodes:\r\n";
                                foreach (uint l in pm.WrapperSolutionClusters[index])
                                    str += l.ToString() + " ";
                            }
                            else if (uint.TryParse(str2[1], out index2))
                            {
                                str += pm.ComputeClusterCost(index2, index).ToString() +
                                    " --- Length = " + pm.SolutionsClusters[index2][index].Count +
                                    "\r\nRefined Solution Gap : " + pm.GapClusters[index2][index] +
                                    "\r\nNodes:\r\n";

                                foreach (uint l in pm.SolutionsClusters[index2][index])
                                    str += l.ToString() + " ";
                            }
                            else
                                str += str2[1] + "error on parsing!";
                        
                        break;

                    default          : str += "Which node is it?";
                                       break;
                }
            }
            else 
               str = "Error on parsing text = " + nodeText[1] + " entire nodeText = " + node.Text;

            ClusterTextBox.Text = str;
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showTableauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.ShowTableau();
            //ora lo stampo su file.
            //pm.ShowTableau(true);
        }

        private void showMatrixCostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.ShowCostMatrix();
        }

        private void showPOfSolutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.ShowPofSolutions();
        }

        private void showCCSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.ShowCCS();
        }

        private void checkPSetsOfSolutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.CheckPofSolutions();
        }

        private void checkWrapperSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pm.WrapperSolutionClusters == null)
            {
                ConsoleAppendText("\r\nNo Wrapper Solution Found!!!");
                return;
            }
  
            if (!pm.CheckWrapperClustersSolution())
                ConsoleAppendText("\r\nWrapper Solution is Wrong!!!");
            else
                ConsoleAppendText("\r\nWrapper Solution is OK!!");
        }

        private void showWrapperSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pm.ShowWrapperSolution(ConsoleAppendText);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
 
        }

    }
}
