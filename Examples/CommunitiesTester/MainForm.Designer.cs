﻿namespace CommunitiesTester
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.maxUpdatesNumUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numNodesNumUD = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.generateBtn = new System.Windows.Forms.Button();
            this.algorithmWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minModularityNumUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numPassesNumUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.colorSchemeCBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxUpdatesNumUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNodesNumUD)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minModularityNumUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPassesNumUD)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pictureBox);
            this.groupBox2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(200, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(810, 553);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 22);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(804, 528);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.maxUpdatesNumUD);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.numNodesNumUD);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 158);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Network options";
            // 
            // maxUpdatesNumUD
            // 
            this.maxUpdatesNumUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxUpdatesNumUD.BackColor = System.Drawing.Color.White;
            this.maxUpdatesNumUD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxUpdatesNumUD.ForeColor = System.Drawing.Color.Black;
            this.maxUpdatesNumUD.Location = new System.Drawing.Point(13, 117);
            this.maxUpdatesNumUD.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.maxUpdatesNumUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxUpdatesNumUD.Name = "maxUpdatesNumUD";
            this.maxUpdatesNumUD.Size = new System.Drawing.Size(156, 26);
            this.maxUpdatesNumUD.TabIndex = 9;
            this.maxUpdatesNumUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.maxUpdatesNumUD.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.maxUpdatesNumUD.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Max. updates:";
            // 
            // numNodesNumUD
            // 
            this.numNodesNumUD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numNodesNumUD.BackColor = System.Drawing.Color.White;
            this.numNodesNumUD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numNodesNumUD.ForeColor = System.Drawing.Color.Black;
            this.numNodesNumUD.Location = new System.Drawing.Point(13, 54);
            this.numNodesNumUD.Margin = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.numNodesNumUD.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numNodesNumUD.Name = "numNodesNumUD";
            this.numNodesNumUD.Size = new System.Drawing.Size(156, 26);
            this.numNodesNumUD.TabIndex = 7;
            this.numNodesNumUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNodesNumUD.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numNodesNumUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Num nodes:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.generateBtn);
            this.groupBox4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 467);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(182, 73);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
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
            this.generateBtn.Size = new System.Drawing.Size(159, 31);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.minModularityNumUD);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numPassesNumUD);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 158);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Algorithm options";
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
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.infoLabel);
            this.groupBox5.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(200, 571);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(807, 46);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Info";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabel.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(3, 22);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(5);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 19);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.colorSchemeCBox);
            this.groupBox6.Controls.Add(this.showLabelsCheckBox);
            this.groupBox6.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(12, 340);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(182, 121);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Visual oprions";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1019, 629);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CommunityGrapher.NET - Communities Tester";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxUpdatesNumUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNodesNumUD)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minModularityNumUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPassesNumUD)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker algorithmWorker;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.NumericUpDown maxUpdatesNumUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numNodesNumUD;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown minModularityNumUD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPassesNumUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox colorSchemeCBox;
        private System.Windows.Forms.CheckBox showLabelsCheckBox;
    }
}

