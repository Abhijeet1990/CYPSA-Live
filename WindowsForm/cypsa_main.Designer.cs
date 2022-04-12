namespace WindowsForm
{
    partial class cypsa_main
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.vulnTrackBar = new System.Windows.Forms.TrackBar();
            this.lbPatchTargets = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labLimitedTargets = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbLimitedTargets = new System.Windows.Forms.ListBox();
            this.algoSelectionCB = new System.Windows.Forms.ComboBox();
            this.btnCalculatePaths = new System.Windows.Forms.Button();
            this.accessPathStatusLabel = new System.Windows.Forms.Label();
            this.loadCaseButton = new System.Windows.Forms.Button();
            this.displayCaseTB = new System.Windows.Forms.TextBox();
            this.graphViewPanel = new System.Windows.Forms.Panel();
            this.atvButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.populateAccessNodes = new System.Windows.Forms.Button();
            this.accessNodesGridView = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.openAllCyberBtn = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.populateAccessPaths = new System.Windows.Forms.Button();
            this.accessPathsGridView = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.old_Button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tgTextBox = new System.Windows.Forms.TextBox();
            this.piLabel = new System.Windows.Forms.Label();
            this.contingencyList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbBranchItems = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBarGenerateAGT = new System.Windows.Forms.ProgressBar();
            this.relaysDataGridView = new System.Windows.Forms.DataGridView();
            this.breakerGridView = new System.Windows.Forms.DataGridView();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.progressLabel = new System.Windows.Forms.Label();
            this.btnGetRelaysFromPW = new System.Windows.Forms.Button();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.labItems = new System.Windows.Forms.Label();
            this.lbGenItems = new System.Windows.Forms.ListBox();
            this.headerText = new System.Windows.Forms.Label();
            this.lbBusList = new System.Windows.Forms.Label();
            this.lbBusItems = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vulnTrackBar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accessNodesGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accessPathsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.relaysDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.breakerGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.vulnTrackBar);
            this.groupBox2.Controls.Add(this.lbPatchTargets);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Location = new System.Drawing.Point(30, 577);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 153);
            this.groupBox2.TabIndex = 114;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Hosts To Patch";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label9.Location = new System.Drawing.Point(6, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 20);
            this.label9.TabIndex = 20;
            this.label9.Text = "Vuln % to Patch";
            // 
            // vulnTrackBar
            // 
            this.vulnTrackBar.Location = new System.Drawing.Point(174, 100);
            this.vulnTrackBar.Name = "vulnTrackBar";
            this.vulnTrackBar.Size = new System.Drawing.Size(104, 45);
            this.vulnTrackBar.TabIndex = 19;
            // 
            // lbPatchTargets
            // 
            this.lbPatchTargets.FormattingEnabled = true;
            this.lbPatchTargets.ItemHeight = 20;
            this.lbPatchTargets.Location = new System.Drawing.Point(63, 25);
            this.lbPatchTargets.Name = "lbPatchTargets";
            this.lbPatchTargets.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbPatchTargets.Size = new System.Drawing.Size(215, 64);
            this.lbPatchTargets.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(6, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Hosts";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labLimitedTargets);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbLimitedTargets);
            this.groupBox1.Controls.Add(this.algoSelectionCB);
            this.groupBox1.Controls.Add(this.btnCalculatePaths);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Location = new System.Drawing.Point(320, 584);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 153);
            this.groupBox1.TabIndex = 113;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AGT Processing";
            // 
            // labLimitedTargets
            // 
            this.labLimitedTargets.AutoSize = true;
            this.labLimitedTargets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labLimitedTargets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLimitedTargets.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labLimitedTargets.Location = new System.Drawing.Point(6, 36);
            this.labLimitedTargets.Name = "labLimitedTargets";
            this.labLimitedTargets.Size = new System.Drawing.Size(133, 20);
            this.labLimitedTargets.TabIndex = 16;
            this.labLimitedTargets.Text = "Limited Targets";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(6, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 20);
            this.label7.TabIndex = 48;
            this.label7.Text = "Graph Algorithm";
            // 
            // lbLimitedTargets
            // 
            this.lbLimitedTargets.FormattingEnabled = true;
            this.lbLimitedTargets.ItemHeight = 20;
            this.lbLimitedTargets.Location = new System.Drawing.Point(157, 14);
            this.lbLimitedTargets.Name = "lbLimitedTargets";
            this.lbLimitedTargets.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLimitedTargets.Size = new System.Drawing.Size(231, 64);
            this.lbLimitedTargets.TabIndex = 15;
            // 
            // algoSelectionCB
            // 
            this.algoSelectionCB.FormattingEnabled = true;
            this.algoSelectionCB.Items.AddRange(new object[] {
            "BFS",
            "DFS",
            "Dijkstra",
            "Bellman-Ford"});
            this.algoSelectionCB.Location = new System.Drawing.Point(205, 84);
            this.algoSelectionCB.Name = "algoSelectionCB";
            this.algoSelectionCB.Size = new System.Drawing.Size(183, 28);
            this.algoSelectionCB.TabIndex = 47;
            // 
            // btnCalculatePaths
            // 
            this.btnCalculatePaths.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculatePaths.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCalculatePaths.Location = new System.Drawing.Point(205, 115);
            this.btnCalculatePaths.Name = "btnCalculatePaths";
            this.btnCalculatePaths.Size = new System.Drawing.Size(183, 32);
            this.btnCalculatePaths.TabIndex = 10;
            this.btnCalculatePaths.Text = "3. Calculate Paths";
            this.btnCalculatePaths.UseVisualStyleBackColor = true;
            this.btnCalculatePaths.Click += new System.EventHandler(this.btnCalculatePaths_Click);
            // 
            // accessPathStatusLabel
            // 
            this.accessPathStatusLabel.AutoSize = true;
            this.accessPathStatusLabel.Location = new System.Drawing.Point(1020, 0);
            this.accessPathStatusLabel.Name = "accessPathStatusLabel";
            this.accessPathStatusLabel.Size = new System.Drawing.Size(0, 13);
            this.accessPathStatusLabel.TabIndex = 112;
            // 
            // loadCaseButton
            // 
            this.loadCaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadCaseButton.Location = new System.Drawing.Point(290, 3);
            this.loadCaseButton.Name = "loadCaseButton";
            this.loadCaseButton.Size = new System.Drawing.Size(113, 32);
            this.loadCaseButton.TabIndex = 111;
            this.loadCaseButton.Text = "Load Case";
            this.loadCaseButton.UseVisualStyleBackColor = true;
            this.loadCaseButton.Click += new System.EventHandler(this.loadCaseButton_Click);
            // 
            // displayCaseTB
            // 
            this.displayCaseTB.Location = new System.Drawing.Point(138, 6);
            this.displayCaseTB.Name = "displayCaseTB";
            this.displayCaseTB.Size = new System.Drawing.Size(146, 20);
            this.displayCaseTB.TabIndex = 110;
            // 
            // graphViewPanel
            // 
            this.graphViewPanel.Location = new System.Drawing.Point(782, 595);
            this.graphViewPanel.Name = "graphViewPanel";
            this.graphViewPanel.Size = new System.Drawing.Size(576, 77);
            this.graphViewPanel.TabIndex = 109;
            // 
            // atvButton
            // 
            this.atvButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.atvButton.Location = new System.Drawing.Point(1195, 53);
            this.atvButton.Name = "atvButton";
            this.atvButton.Size = new System.Drawing.Size(160, 29);
            this.atvButton.TabIndex = 108;
            this.atvButton.Text = "Attack Tree View";
            this.atvButton.UseVisualStyleBackColor = true;
            this.atvButton.Click += new System.EventHandler(this.AtvButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged_1);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.populateAccessNodes);
            this.tabPage1.Controls.Add(this.accessNodesGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Access Nodes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // populateAccessNodes
            // 
            this.populateAccessNodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.populateAccessNodes.Location = new System.Drawing.Point(6, 6);
            this.populateAccessNodes.Name = "populateAccessNodes";
            this.populateAccessNodes.Size = new System.Drawing.Size(755, 34);
            this.populateAccessNodes.TabIndex = 1;
            this.populateAccessNodes.Text = "4. Populate";
            this.populateAccessNodes.UseVisualStyleBackColor = true;
            this.populateAccessNodes.Click += new System.EventHandler(this.populateAccessNodes_Click);
            // 
            // accessNodesGridView
            // 
            this.accessNodesGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.accessNodesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accessNodesGridView.Location = new System.Drawing.Point(6, 46);
            this.accessNodesGridView.Name = "accessNodesGridView";
            this.accessNodesGridView.Size = new System.Drawing.Size(755, 408);
            this.accessNodesGridView.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(26, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 20);
            this.label6.TabIndex = 107;
            this.label6.Text = "Select Case";
            // 
            // openAllCyberBtn
            // 
            this.openAllCyberBtn.Location = new System.Drawing.Point(666, 589);
            this.openAllCyberBtn.Name = "openAllCyberBtn";
            this.openAllCyberBtn.Size = new System.Drawing.Size(10, 12);
            this.openAllCyberBtn.TabIndex = 102;
            this.openAllCyberBtn.Text = "3. OpenAllCyber";
            this.openAllCyberBtn.UseVisualStyleBackColor = true;
            this.openAllCyberBtn.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.populateAccessPaths);
            this.tabPage2.Controls.Add(this.accessPathsGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 462);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Access Paths";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // populateAccessPaths
            // 
            this.populateAccessPaths.Location = new System.Drawing.Point(7, 7);
            this.populateAccessPaths.Name = "populateAccessPaths";
            this.populateAccessPaths.Size = new System.Drawing.Size(754, 33);
            this.populateAccessPaths.TabIndex = 1;
            this.populateAccessPaths.Text = "6. Populate";
            this.populateAccessPaths.UseVisualStyleBackColor = true;
            this.populateAccessPaths.Click += new System.EventHandler(this.populateAccessPaths_Click);
            // 
            // accessPathsGridView
            // 
            this.accessPathsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accessPathsGridView.Location = new System.Drawing.Point(6, 46);
            this.accessPathsGridView.Name = "accessPathsGridView";
            this.accessPathsGridView.Size = new System.Drawing.Size(755, 410);
            this.accessPathsGridView.TabIndex = 0;
            // 
            // Select
            // 
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.Visible = false;
            // 
            // old_Button
            // 
            this.old_Button.Location = new System.Drawing.Point(813, 686);
            this.old_Button.Name = "old_Button";
            this.old_Button.Size = new System.Drawing.Size(151, 33);
            this.old_Button.TabIndex = 106;
            this.old_Button.Text = "One-Line Diagram";
            this.old_Button.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(998, 699);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 105;
            this.label5.Text = "TigerGraph IP";
            // 
            // tgTextBox
            // 
            this.tgTextBox.Location = new System.Drawing.Point(1112, 693);
            this.tgTextBox.Name = "tgTextBox";
            this.tgTextBox.Size = new System.Drawing.Size(90, 20);
            this.tgTextBox.TabIndex = 104;
            // 
            // piLabel
            // 
            this.piLabel.AutoSize = true;
            this.piLabel.Location = new System.Drawing.Point(604, 598);
            this.piLabel.Name = "piLabel";
            this.piLabel.Size = new System.Drawing.Size(0, 13);
            this.piLabel.TabIndex = 103;
            // 
            // contingencyList
            // 
            this.contingencyList.FormattingEnabled = true;
            this.contingencyList.Location = new System.Drawing.Point(348, 169);
            this.contingencyList.Name = "contingencyList";
            this.contingencyList.Size = new System.Drawing.Size(209, 56);
            this.contingencyList.TabIndex = 100;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(344, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 20);
            this.label4.TabIndex = 101;
            this.label4.Text = "Contingencies";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(33, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 20);
            this.label3.TabIndex = 99;
            this.label3.Text = "Breaker Control";
            // 
            // lbBranchItems
            // 
            this.lbBranchItems.FormattingEnabled = true;
            this.lbBranchItems.Location = new System.Drawing.Point(348, 61);
            this.lbBranchItems.Name = "lbBranchItems";
            this.lbBranchItems.Size = new System.Drawing.Size(207, 56);
            this.lbBranchItems.TabIndex = 93;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(344, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 92;
            this.label1.Text = "Branches";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(286, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 95;
            this.label2.Text = "Relays";
            // 
            // progressBarGenerateAGT
            // 
            this.progressBarGenerateAGT.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.progressBarGenerateAGT.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.progressBarGenerateAGT.Location = new System.Drawing.Point(687, 21);
            this.progressBarGenerateAGT.Name = "progressBarGenerateAGT";
            this.progressBarGenerateAGT.Size = new System.Drawing.Size(668, 24);
            this.progressBarGenerateAGT.TabIndex = 96;
            // 
            // relaysDataGridView
            // 
            this.relaysDataGridView.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            this.relaysDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.relaysDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select});
            this.relaysDataGridView.Location = new System.Drawing.Point(30, 431);
            this.relaysDataGridView.Name = "relaysDataGridView";
            this.relaysDataGridView.Size = new System.Drawing.Size(527, 140);
            this.relaysDataGridView.TabIndex = 94;
            // 
            // breakerGridView
            // 
            this.breakerGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.breakerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.breakerGridView.Location = new System.Drawing.Point(33, 289);
            this.breakerGridView.Name = "breakerGridView";
            this.breakerGridView.Size = new System.Drawing.Size(524, 86);
            this.breakerGridView.TabIndex = 98;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.Location = new System.Drawing.Point(1208, 699);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(27, 13);
            this.linkLabel1.TabIndex = 97;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Link";
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.ForeColor = System.Drawing.Color.White;
            this.progressLabel.Location = new System.Drawing.Point(618, -6);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 86;
            // 
            // btnGetRelaysFromPW
            // 
            this.btnGetRelaysFromPW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetRelaysFromPW.Location = new System.Drawing.Point(33, 381);
            this.btnGetRelaysFromPW.Name = "btnGetRelaysFromPW";
            this.btnGetRelaysFromPW.Size = new System.Drawing.Size(229, 44);
            this.btnGetRelaysFromPW.TabIndex = 88;
            this.btnGetRelaysFromPW.Text = "1. Get Relays from PW";
            this.btnGetRelaysFromPW.UseVisualStyleBackColor = true;
            this.btnGetRelaysFromPW.Click += new System.EventHandler(this.btnGetRelaysFromPW_Click);
            // 
            // btnAddToCart
            // 
            this.btnAddToCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddToCart.Location = new System.Drawing.Point(389, 379);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(168, 48);
            this.btnAddToCart.TabIndex = 87;
            this.btnAddToCart.Text = "2. Add to Selection ->";
            this.btnAddToCart.UseVisualStyleBackColor = true;
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);
            // 
            // labItems
            // 
            this.labItems.AutoSize = true;
            this.labItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labItems.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labItems.Location = new System.Drawing.Point(33, 40);
            this.labItems.Name = "labItems";
            this.labItems.Size = new System.Drawing.Size(100, 20);
            this.labItems.TabIndex = 85;
            this.labItems.Text = "Generators";
            // 
            // lbGenItems
            // 
            this.lbGenItems.FormattingEnabled = true;
            this.lbGenItems.Location = new System.Drawing.Point(30, 61);
            this.lbGenItems.Name = "lbGenItems";
            this.lbGenItems.Size = new System.Drawing.Size(312, 56);
            this.lbGenItems.TabIndex = 84;
            // 
            // headerText
            // 
            this.headerText.AutoSize = true;
            this.headerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerText.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.headerText.Location = new System.Drawing.Point(468, 9);
            this.headerText.Name = "headerText";
            this.headerText.Size = new System.Drawing.Size(184, 37);
            this.headerText.TabIndex = 83;
            this.headerText.Text = "Cypsa-Live";
            // 
            // lbBusList
            // 
            this.lbBusList.AutoSize = true;
            this.lbBusList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBusList.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbBusList.Location = new System.Drawing.Point(33, 148);
            this.lbBusList.Name = "lbBusList";
            this.lbBusList.Size = new System.Drawing.Size(59, 20);
            this.lbBusList.TabIndex = 90;
            this.lbBusList.Text = "Buses";
            // 
            // lbBusItems
            // 
            this.lbBusItems.FormattingEnabled = true;
            this.lbBusItems.Location = new System.Drawing.Point(33, 169);
            this.lbBusItems.Name = "lbBusItems";
            this.lbBusItems.Size = new System.Drawing.Size(309, 56);
            this.lbBusItems.TabIndex = 89;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(576, 88);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 495);
            this.tabControl1.TabIndex = 91;
            // 
            // cypsa_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(1359, 749);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.accessPathStatusLabel);
            this.Controls.Add(this.loadCaseButton);
            this.Controls.Add(this.displayCaseTB);
            this.Controls.Add(this.graphViewPanel);
            this.Controls.Add(this.atvButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.openAllCyberBtn);
            this.Controls.Add(this.old_Button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tgTextBox);
            this.Controls.Add(this.piLabel);
            this.Controls.Add(this.contingencyList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbBranchItems);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBarGenerateAGT);
            this.Controls.Add(this.relaysDataGridView);
            this.Controls.Add(this.breakerGridView);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.btnGetRelaysFromPW);
            this.Controls.Add(this.btnAddToCart);
            this.Controls.Add(this.labItems);
            this.Controls.Add(this.lbGenItems);
            this.Controls.Add(this.headerText);
            this.Controls.Add(this.lbBusList);
            this.Controls.Add(this.lbBusItems);
            this.Controls.Add(this.tabControl1);
            this.Name = "cypsa_main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.cypsa_main_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vulnTrackBar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accessNodesGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accessPathsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.relaysDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.breakerGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar vulnTrackBar;
        public System.Windows.Forms.ListBox lbPatchTargets;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labLimitedTargets;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ListBox lbLimitedTargets;
        private System.Windows.Forms.ComboBox algoSelectionCB;
        private System.Windows.Forms.Button btnCalculatePaths;
        private System.Windows.Forms.Label accessPathStatusLabel;
        private System.Windows.Forms.Button loadCaseButton;
        private System.Windows.Forms.TextBox displayCaseTB;
        private System.Windows.Forms.Panel graphViewPanel;
        private System.Windows.Forms.Button atvButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button populateAccessNodes;
        private System.Windows.Forms.DataGridView accessNodesGridView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button openAllCyberBtn;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button populateAccessPaths;
        private System.Windows.Forms.DataGridView accessPathsGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.Button old_Button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tgTextBox;
        private System.Windows.Forms.Label piLabel;
        private System.Windows.Forms.ListBox contingencyList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbBranchItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBarGenerateAGT;
        private System.Windows.Forms.DataGridView relaysDataGridView;
        private System.Windows.Forms.DataGridView breakerGridView;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button btnGetRelaysFromPW;
        private System.Windows.Forms.Button btnAddToCart;
        private System.Windows.Forms.Label labItems;
        private System.Windows.Forms.ListBox lbGenItems;
        private System.Windows.Forms.Label headerText;
        private System.Windows.Forms.Label lbBusList;
        private System.Windows.Forms.ListBox lbBusItems;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

