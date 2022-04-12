namespace WindowsForm
{
    partial class host
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(host));
            this.hostIPLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hostIPLabel
            // 
            this.hostIPLabel.AutoSize = true;
            this.hostIPLabel.Location = new System.Drawing.Point(25, 68);
            this.hostIPLabel.Name = "hostIPLabel";
            this.hostIPLabel.Size = new System.Drawing.Size(27, 13);
            this.hostIPLabel.TabIndex = 0;
            this.hostIPLabel.Text = "host";
            // 
            // host
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.hostIPLabel);
            this.Name = "host";
            this.Size = new System.Drawing.Size(200, 183);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label hostIPLabel;
    }
}
