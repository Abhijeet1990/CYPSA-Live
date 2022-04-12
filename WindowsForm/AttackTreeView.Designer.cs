namespace WindowsForm
{
    partial class AttackTreeView
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
            this.vulnStatusBox = new System.Windows.Forms.TextBox();
            this.scoreDGV = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scoreDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // vulnStatusBox
            // 
            this.vulnStatusBox.Location = new System.Drawing.Point(653, 13);
            this.vulnStatusBox.Multiline = true;
            this.vulnStatusBox.Name = "vulnStatusBox";
            this.vulnStatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.vulnStatusBox.Size = new System.Drawing.Size(282, 183);
            this.vulnStatusBox.TabIndex = 0;
            // 
            // scoreDGV
            // 
            this.scoreDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.scoreDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scoreDGV.Location = new System.Drawing.Point(653, 232);
            this.scoreDGV.Name = "scoreDGV";
            this.scoreDGV.Size = new System.Drawing.Size(282, 379);
            this.scoreDGV.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick_1);
            // 
            // AttackTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(947, 623);
            this.Controls.Add(this.scoreDGV);
            this.Controls.Add(this.vulnStatusBox);
            this.Name = "AttackTreeView";
            this.Text = "AttackTreeView";
            this.Load += new System.EventHandler(this.AttackTreeView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scoreDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox vulnStatusBox;
        private System.Windows.Forms.DataGridView scoreDGV;
        private System.Windows.Forms.Timer timer1;
    }
}