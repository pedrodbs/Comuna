namespace CommunitiesVisualizer
{
    partial class MainForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.exportBtn = new System.Windows.Forms.Button();
            this.generateBtn = new System.Windows.Forms.Button();
            this.algorithmWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.minModularityNumUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numPassesNumUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.colorSchemeCBox = new System.Windows.Forms.ComboBox();
            this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveImageWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minModularityNumUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPassesNumUD)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.pictureBox);
            this.groupBox5.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(200, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(807, 555);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Output";
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 22);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(801, 530);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.exportBtn);
            this.groupBox4.Controls.Add(this.generateBtn);
            this.groupBox4.Enabled = false;
            this.groupBox4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 426);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(182, 121);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
            // 
            // exportBtn
            // 
            this.exportBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.exportBtn.Enabled = false;
            this.exportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportBtn.Location = new System.Drawing.Point(13, 73);
            this.exportBtn.Margin = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(156, 31);
            this.exportBtn.TabIndex = 11;
            this.exportBtn.Text = "&Export...";
            this.exportBtn.UseVisualStyleBackColor = false;
            this.exportBtn.Click += new System.EventHandler(this.ExportBtnClick);
            // 
            // generateBtn
            // 
            this.generateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generateBtn.AutoSize = true;
            this.generateBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.generateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generateBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateBtn.Location = new System.Drawing.Point(13, 27);
            this.generateBtn.Margin = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(156, 31);
            this.generateBtn.TabIndex = 10;
            this.generateBtn.Text = "&Generate";
            this.generateBtn.UseVisualStyleBackColor = false;
            this.generateBtn.Click += new System.EventHandler(this.GenerateBtnClick);
            // 
            // algorithmWorker
            // 
            this.algorithmWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AlgorithmDoWork);
            this.algorithmWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AlgorithmWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.minModularityNumUD);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numPassesNumUD);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 158);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Algorithm options";
            // 
            // minModularityNumUD
            // 
            this.minModularityNumUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.minModularityNumUD.BackColor = System.Drawing.Color.White;
            this.minModularityNumUD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minModularityNumUD.DecimalPlaces = 4;
            this.minModularityNumUD.ForeColor = System.Drawing.Color.Black;
            this.minModularityNumUD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.minModularityNumUD.Location = new System.Drawing.Point(13, 117);
            this.minModularityNumUD.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.minModularityNumUD.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minModularityNumUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.minModularityNumUD.Name = "minModularityNumUD";
            this.minModularityNumUD.Size = new System.Drawing.Size(156, 26);
            this.minModularityNumUD.TabIndex = 9;
            this.minModularityNumUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.minModularityNumUD.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.minModularityNumUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Min. modularity:";
            // 
            // numPassesNumUD
            // 
            this.numPassesNumUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numPassesNumUD.BackColor = System.Drawing.Color.White;
            this.numPassesNumUD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPassesNumUD.ForeColor = System.Drawing.Color.Black;
            this.numPassesNumUD.Location = new System.Drawing.Point(13, 54);
            this.numPassesNumUD.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.numPassesNumUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPassesNumUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numPassesNumUD.Name = "numPassesNumUD";
            this.numPassesNumUD.Size = new System.Drawing.Size(156, 26);
            this.numPassesNumUD.TabIndex = 7;
            this.numPassesNumUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPassesNumUD.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numPassesNumUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Num passes:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.colorSchemeCBox);
            this.groupBox3.Controls.Add(this.showLabelsCheckBox);
            this.groupBox3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 299);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 121);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visual options";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 59);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 19);
            this.label5.TabIndex = 9;
            this.label5.Text = "Color scheme:";
            // 
            // colorSchemeCBox
            // 
            this.colorSchemeCBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorSchemeCBox.FormattingEnabled = true;
            this.colorSchemeCBox.Items.AddRange(new object[] {
            "Tol Diverging",
            "Tol Rainbow"});
            this.colorSchemeCBox.Location = new System.Drawing.Point(13, 86);
            this.colorSchemeCBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.colorSchemeCBox.Name = "colorSchemeCBox";
            this.colorSchemeCBox.Size = new System.Drawing.Size(156, 26);
            this.colorSchemeCBox.TabIndex = 1;
            this.colorSchemeCBox.SelectedIndexChanged += new System.EventHandler(this.ColorSchemeCBoxIndexChanged);
            // 
            // showLabelsCheckBox
            // 
            this.showLabelsCheckBox.AutoSize = true;
            this.showLabelsCheckBox.Checked = true;
            this.showLabelsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLabelsCheckBox.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showLabelsCheckBox.Location = new System.Drawing.Point(13, 26);
            this.showLabelsCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.showLabelsCheckBox.Name = "showLabelsCheckBox";
            this.showLabelsCheckBox.Size = new System.Drawing.Size(109, 23);
            this.showLabelsCheckBox.TabIndex = 0;
            this.showLabelsCheckBox.Text = "Show labels";
            this.showLabelsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileNameLabel);
            this.groupBox1.Controls.Add(this.openFileBtn);
            this.groupBox1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 115);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameLabel.BackColor = System.Drawing.Color.White;
            this.fileNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fileNameLabel.Location = new System.Drawing.Point(13, 22);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(156, 31);
            this.fileNameLabel.TabIndex = 0;
            this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileBtn
            // 
            this.openFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openFileBtn.AutoSize = true;
            this.openFileBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.openFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openFileBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFileBtn.Location = new System.Drawing.Point(13, 68);
            this.openFileBtn.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(156, 31);
            this.openFileBtn.TabIndex = 1;
            this.openFileBtn.Text = "&Open...";
            this.openFileBtn.UseVisualStyleBackColor = false;
            this.openFileBtn.Click += new System.EventHandler(this.OpenFileBtnClick);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.BackColor = System.Drawing.Color.White;
            this.groupBox6.Controls.Add(this.infoLabel);
            this.groupBox6.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(200, 573);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(807, 46);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Info";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.BackColor = System.Drawing.Color.White;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabel.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(3, 22);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(5);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.infoLabel.Size = new System.Drawing.Size(10, 19);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "csv";
            this.openFileDialog.FileName = "network.csv";
            this.openFileDialog.Filter = "CSV files|*.csv";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Title = "Select the file containing the network";
            // 
            // saveImageFileDialog
            // 
            this.saveImageFileDialog.DefaultExt = "png";
            this.saveImageFileDialog.Filter = "Portable Network Graphics|*.png|Portable Document Format|*.pdf|Scalable Vector Gr" +
    "aphics|*.svg|Jpeg image|*.jpg|Bitmap image|*.bmp|D3 Json file|*.json";
            this.saveImageFileDialog.RestoreDirectory = true;
            this.saveImageFileDialog.Title = "Choose a file to export the community graph";
            // 
            // saveImageWorker
            // 
            this.saveImageWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SaveImageDoWork);
            this.saveImageWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SaveImageCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1019, 629);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Comuna.NET - Communities Visualizer";
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minModularityNumUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPassesNumUD)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.ComponentModel.BackgroundWorker algorithmWorker;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown minModularityNumUD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPassesNumUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox colorSchemeCBox;
        private System.Windows.Forms.CheckBox showLabelsCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog;
        private System.ComponentModel.BackgroundWorker saveImageWorker;
    }
}

