namespace P_MedianForm
{
    /// <summary>
    /// 
    /// </summary>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMatrixCostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTableauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPOfSolutionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCCSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkPSetsOfSolutionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkWrapperSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showWrapperSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitVContainerMain = new System.Windows.Forms.SplitContainer();
            this.SolutionViewer = new System.Windows.Forms.TreeView();
            this.splitHContainer = new System.Windows.Forms.SplitContainer();
            this.ClusterTextBox = new System.Windows.Forms.TextBox();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitVContainerMain)).BeginInit();
            this.splitVContainerMain.Panel1.SuspendLayout();
            this.splitVContainerMain.Panel2.SuspendLayout();
            this.splitVContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHContainer)).BeginInit();
            this.splitHContainer.Panel1.SuspendLayout();
            this.splitHContainer.Panel2.SuspendLayout();
            this.splitHContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 398);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(609, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoToolTip = true;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(492, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Clear Console";
            this.toolStripStatusLabel1.ToolTipText = "Clear the TextBoxConsole";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.computeToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(609, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit.";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // computeToolStripMenuItem
            // 
            this.computeToolStripMenuItem.Name = "computeToolStripMenuItem";
            this.computeToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.computeToolStripMenuItem.Text = "Compute...";
            this.computeToolStripMenuItem.Click += new System.EventHandler(this.computeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMatrixCostToolStripMenuItem,
            this.showTableauToolStripMenuItem,
            this.showPOfSolutionsToolStripMenuItem,
            this.showCCSToolStripMenuItem,
            this.checkPSetsOfSolutionsToolStripMenuItem,
            this.checkWrapperSolutionToolStripMenuItem,
            this.showWrapperSolutionToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showMatrixCostToolStripMenuItem
            // 
            this.showMatrixCostToolStripMenuItem.Enabled = false;
            this.showMatrixCostToolStripMenuItem.Name = "showMatrixCostToolStripMenuItem";
            this.showMatrixCostToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showMatrixCostToolStripMenuItem.Text = "Show Matrix Cost";
            this.showMatrixCostToolStripMenuItem.Click += new System.EventHandler(this.showMatrixCostToolStripMenuItem_Click);
            // 
            // showTableauToolStripMenuItem
            // 
            this.showTableauToolStripMenuItem.Enabled = false;
            this.showTableauToolStripMenuItem.Name = "showTableauToolStripMenuItem";
            this.showTableauToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showTableauToolStripMenuItem.Text = "Show Tableau";
            this.showTableauToolStripMenuItem.Click += new System.EventHandler(this.showTableauToolStripMenuItem_Click);
            // 
            // showPOfSolutionsToolStripMenuItem
            // 
            this.showPOfSolutionsToolStripMenuItem.Enabled = false;
            this.showPOfSolutionsToolStripMenuItem.Name = "showPOfSolutionsToolStripMenuItem";
            this.showPOfSolutionsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showPOfSolutionsToolStripMenuItem.Text = "Show P of Solutions";
            this.showPOfSolutionsToolStripMenuItem.Click += new System.EventHandler(this.showPOfSolutionsToolStripMenuItem_Click);
            // 
            // showCCSToolStripMenuItem
            // 
            this.showCCSToolStripMenuItem.Enabled = false;
            this.showCCSToolStripMenuItem.Name = "showCCSToolStripMenuItem";
            this.showCCSToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showCCSToolStripMenuItem.Text = "Show CCS";
            this.showCCSToolStripMenuItem.Click += new System.EventHandler(this.showCCSToolStripMenuItem_Click);
            // 
            // checkPSetsOfSolutionsToolStripMenuItem
            // 
            this.checkPSetsOfSolutionsToolStripMenuItem.Name = "checkPSetsOfSolutionsToolStripMenuItem";
            this.checkPSetsOfSolutionsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.checkPSetsOfSolutionsToolStripMenuItem.Text = "Check P Sets of Solutions";
            this.checkPSetsOfSolutionsToolStripMenuItem.Click += new System.EventHandler(this.checkPSetsOfSolutionsToolStripMenuItem_Click);
            // 
            // checkWrapperSolutionToolStripMenuItem
            // 
            this.checkWrapperSolutionToolStripMenuItem.Name = "checkWrapperSolutionToolStripMenuItem";
            this.checkWrapperSolutionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.checkWrapperSolutionToolStripMenuItem.Text = "Check Wrapper Solution";
            this.checkWrapperSolutionToolStripMenuItem.Click += new System.EventHandler(this.checkWrapperSolutionToolStripMenuItem_Click);
            // 
            // showWrapperSolutionToolStripMenuItem
            // 
            this.showWrapperSolutionToolStripMenuItem.Name = "showWrapperSolutionToolStripMenuItem";
            this.showWrapperSolutionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showWrapperSolutionToolStripMenuItem.Text = "Show Wrapper Solution";
            this.showWrapperSolutionToolStripMenuItem.Click += new System.EventHandler(this.showWrapperSolutionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(52, 20);
            this.toolStripMenuItem1.Text = "About";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.txt";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text File|*.txt";
            this.openFileDialog1.Title = "Open P-Median File";
            // 
            // splitVContainerMain
            // 
            this.splitVContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitVContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitVContainerMain.Name = "splitVContainerMain";
            // 
            // splitVContainerMain.Panel1
            // 
            this.splitVContainerMain.Panel1.Controls.Add(this.SolutionViewer);
            // 
            // splitVContainerMain.Panel2
            // 
            this.splitVContainerMain.Panel2.Controls.Add(this.splitHContainer);
            this.splitVContainerMain.Size = new System.Drawing.Size(609, 374);
            this.splitVContainerMain.SplitterDistance = 139;
            this.splitVContainerMain.TabIndex = 2;
            // 
            // SolutionViewer
            // 
            this.SolutionViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SolutionViewer.Location = new System.Drawing.Point(0, 0);
            this.SolutionViewer.Name = "SolutionViewer";
            this.SolutionViewer.Size = new System.Drawing.Size(139, 374);
            this.SolutionViewer.TabIndex = 0;
            this.SolutionViewer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SolutionViewer_AfterSelect);
            // 
            // splitHContainer
            // 
            this.splitHContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitHContainer.Location = new System.Drawing.Point(0, 0);
            this.splitHContainer.Name = "splitHContainer";
            this.splitHContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitHContainer.Panel1
            // 
            this.splitHContainer.Panel1.Controls.Add(this.ClusterTextBox);
            // 
            // splitHContainer.Panel2
            // 
            this.splitHContainer.Panel2.Controls.Add(this.ConsoleTextBox);
            this.splitHContainer.Size = new System.Drawing.Size(466, 374);
            this.splitHContainer.SplitterDistance = 120;
            this.splitHContainer.TabIndex = 0;
            // 
            // ClusterTextBox
            // 
            this.ClusterTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClusterTextBox.Location = new System.Drawing.Point(0, 0);
            this.ClusterTextBox.Multiline = true;
            this.ClusterTextBox.Name = "ClusterTextBox";
            this.ClusterTextBox.ReadOnly = true;
            this.ClusterTextBox.Size = new System.Drawing.Size(466, 120);
            this.ClusterTextBox.TabIndex = 0;
            this.ClusterTextBox.Text = "Solution Detailed information Time, cost etc...";
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.ConsoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.ConsoleTextBox.Location = new System.Drawing.Point(0, 0);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ReadOnly = true;
            this.ConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConsoleTextBox.Size = new System.Drawing.Size(466, 250);
            this.ConsoleTextBox.TabIndex = 0;
            this.ConsoleTextBox.WordWrap = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 420);
            this.Controls.Add(this.splitVContainerMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "P-Median Project";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitVContainerMain.Panel1.ResumeLayout(false);
            this.splitVContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitVContainerMain)).EndInit();
            this.splitVContainerMain.ResumeLayout(false);
            this.splitHContainer.Panel1.ResumeLayout(false);
            this.splitHContainer.Panel1.PerformLayout();
            this.splitHContainer.Panel2.ResumeLayout(false);
            this.splitHContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHContainer)).EndInit();
            this.splitHContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitVContainerMain;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TreeView SolutionViewer;
        private System.Windows.Forms.SplitContainer splitHContainer;
        private System.Windows.Forms.TextBox ClusterTextBox;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.ToolStripMenuItem computeToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTableauToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMatrixCostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPOfSolutionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCCSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkPSetsOfSolutionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkWrapperSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showWrapperSolutionToolStripMenuItem;
    }
}

