namespace CardReader
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
            this.HNumber = new System.Windows.Forms.NumericUpDown();
            this.SNumber = new System.Windows.Forms.NumericUpDown();
            this.VNumber = new System.Windows.Forms.NumericUpDown();
            this.SharpenBox = new System.Windows.Forms.CheckBox();
            this.CardContentLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // HNumber
            // 
            this.HNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HNumber.Location = new System.Drawing.Point(668, 12);
            this.HNumber.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.HNumber.Name = "HNumber";
            this.HNumber.Size = new System.Drawing.Size(120, 26);
            this.HNumber.TabIndex = 0;
            this.HNumber.ValueChanged += new System.EventHandler(this.HNumber_ValueChanged);
            // 
            // SNumber
            // 
            this.SNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SNumber.Location = new System.Drawing.Point(668, 44);
            this.SNumber.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SNumber.Name = "SNumber";
            this.SNumber.Size = new System.Drawing.Size(120, 26);
            this.SNumber.TabIndex = 1;
            this.SNumber.ValueChanged += new System.EventHandler(this.SNumber_ValueChanged);
            // 
            // VNumber
            // 
            this.VNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VNumber.Location = new System.Drawing.Point(668, 76);
            this.VNumber.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.VNumber.Name = "VNumber";
            this.VNumber.Size = new System.Drawing.Size(120, 26);
            this.VNumber.TabIndex = 2;
            this.VNumber.ValueChanged += new System.EventHandler(this.VNumber_ValueChanged);
            // 
            // SharpenBox
            // 
            this.SharpenBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SharpenBox.AutoSize = true;
            this.SharpenBox.Location = new System.Drawing.Point(674, 109);
            this.SharpenBox.Name = "SharpenBox";
            this.SharpenBox.Size = new System.Drawing.Size(96, 24);
            this.SharpenBox.TabIndex = 3;
            this.SharpenBox.Text = "Sharpen";
            this.SharpenBox.UseVisualStyleBackColor = true;
            // 
            // CardContentLabel
            // 
            this.CardContentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CardContentLabel.AutoSize = true;
            this.CardContentLabel.Location = new System.Drawing.Point(442, 137);
            this.CardContentLabel.Name = "CardContentLabel";
            this.CardContentLabel.Size = new System.Drawing.Size(104, 20);
            this.CardContentLabel.TabIndex = 4;
            this.CardContentLabel.Text = "Card Content";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CardContentLabel);
            this.Controls.Add(this.SharpenBox);
            this.Controls.Add(this.VNumber);
            this.Controls.Add(this.SNumber);
            this.Controls.Add(this.HNumber);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.HNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown HNumber;
        private System.Windows.Forms.NumericUpDown SNumber;
        private System.Windows.Forms.NumericUpDown VNumber;
        private System.Windows.Forms.CheckBox SharpenBox;
        private System.Windows.Forms.Label CardContentLabel;
    }
}

