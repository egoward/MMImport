namespace Edonica.XMLImport
{
    partial class FormReproject
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.radioButtonWGS84toOS = new System.Windows.Forms.RadioButton();
            this.radioButtonOStoWGS84 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSourceA = new System.Windows.Forms.TextBox();
            this.buttonSourceBrowse = new System.Windows.Forms.Button();
            this.comboBoxSourceMap = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxTargetA = new System.Windows.Forms.TextBox();
            this.buttonTargetBrowse = new System.Windows.Forms.Button();
            this.comboBoxTargetMap = new System.Windows.Forms.ComboBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxSourceB = new System.Windows.Forms.TextBox();
            this.textBoxTargetB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(26, 32);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(309, 38);
            label1.TabIndex = 2;
            label1.Text = "WGS84 is the Latitude Longitude based coordinate system used by GPS units.  Also " +
                "known as EPSG:4396.";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(26, 93);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(309, 37);
            label2.TabIndex = 3;
            label2.Text = "OSGB1936 is the meter based coordinate system used by Ordnance Survey in the UK. " +
                " Also known as EPSG:27700";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(46, 130);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(170, 16);
            label3.TabIndex = 4;
            label3.Text = "Conversions done via OSTN02\r\n";
            // 
            // radioButtonWGS84toOS
            // 
            this.radioButtonWGS84toOS.AutoSize = true;
            this.radioButtonWGS84toOS.Location = new System.Drawing.Point(12, 12);
            this.radioButtonWGS84toOS.Name = "radioButtonWGS84toOS";
            this.radioButtonWGS84toOS.Size = new System.Drawing.Size(132, 17);
            this.radioButtonWGS84toOS.TabIndex = 0;
            this.radioButtonWGS84toOS.TabStop = true;
            this.radioButtonWGS84toOS.Text = "WGS84 to OSGB1936";
            this.radioButtonWGS84toOS.UseVisualStyleBackColor = true;
            // 
            // radioButtonOStoWGS84
            // 
            this.radioButtonOStoWGS84.AutoSize = true;
            this.radioButtonOStoWGS84.Location = new System.Drawing.Point(12, 73);
            this.radioButtonOStoWGS84.Name = "radioButtonOStoWGS84";
            this.radioButtonOStoWGS84.Size = new System.Drawing.Size(132, 17);
            this.radioButtonOStoWGS84.TabIndex = 1;
            this.radioButtonOStoWGS84.TabStop = true;
            this.radioButtonOStoWGS84.Text = "OSGB1936 to WGS84";
            this.radioButtonOStoWGS84.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxSourceB);
            this.groupBox1.Controls.Add(this.textBoxSourceA);
            this.groupBox1.Controls.Add(this.buttonSourceBrowse);
            this.groupBox1.Controls.Add(this.comboBoxSourceMap);
            this.groupBox1.Location = new System.Drawing.Point(12, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 104);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Map";
            // 
            // textBoxSourceA
            // 
            this.textBoxSourceA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceA.Location = new System.Drawing.Point(7, 48);
            this.textBoxSourceA.Name = "textBoxSourceA";
            this.textBoxSourceA.Size = new System.Drawing.Size(400, 20);
            this.textBoxSourceA.TabIndex = 2;
            // 
            // buttonSourceBrowse
            // 
            this.buttonSourceBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSourceBrowse.Location = new System.Drawing.Point(413, 48);
            this.buttonSourceBrowse.Name = "buttonSourceBrowse";
            this.buttonSourceBrowse.Size = new System.Drawing.Size(34, 46);
            this.buttonSourceBrowse.TabIndex = 1;
            this.buttonSourceBrowse.Text = "...";
            this.buttonSourceBrowse.UseVisualStyleBackColor = true;
            this.buttonSourceBrowse.Click += new System.EventHandler(this.buttonSourceBrowse_Click);
            // 
            // comboBoxSourceMap
            // 
            this.comboBoxSourceMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSourceMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSourceMap.FormattingEnabled = true;
            this.comboBoxSourceMap.Location = new System.Drawing.Point(7, 20);
            this.comboBoxSourceMap.Name = "comboBoxSourceMap";
            this.comboBoxSourceMap.Size = new System.Drawing.Size(441, 21);
            this.comboBoxSourceMap.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxTargetB);
            this.groupBox2.Controls.Add(this.textBoxTargetA);
            this.groupBox2.Controls.Add(this.buttonTargetBrowse);
            this.groupBox2.Controls.Add(this.comboBoxTargetMap);
            this.groupBox2.Location = new System.Drawing.Point(12, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(454, 104);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination Map";
            // 
            // textBoxTargetA
            // 
            this.textBoxTargetA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetA.Location = new System.Drawing.Point(7, 48);
            this.textBoxTargetA.Name = "textBoxTargetA";
            this.textBoxTargetA.Size = new System.Drawing.Size(400, 20);
            this.textBoxTargetA.TabIndex = 2;
            // 
            // buttonTargetBrowse
            // 
            this.buttonTargetBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTargetBrowse.Location = new System.Drawing.Point(413, 48);
            this.buttonTargetBrowse.Name = "buttonTargetBrowse";
            this.buttonTargetBrowse.Size = new System.Drawing.Size(34, 46);
            this.buttonTargetBrowse.TabIndex = 1;
            this.buttonTargetBrowse.Text = "...";
            this.buttonTargetBrowse.UseVisualStyleBackColor = true;
            this.buttonTargetBrowse.Click += new System.EventHandler(this.buttonTargetBrowse_Click);
            // 
            // comboBoxTargetMap
            // 
            this.comboBoxTargetMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTargetMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetMap.FormattingEnabled = true;
            this.comboBoxTargetMap.Location = new System.Drawing.Point(7, 20);
            this.comboBoxTargetMap.Name = "comboBoxTargetMap";
            this.comboBoxTargetMap.Size = new System.Drawing.Size(441, 21);
            this.comboBoxTargetMap.TabIndex = 0;
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.Location = new System.Drawing.Point(398, 374);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(68, 27);
            this.buttonGo.TabIndex = 7;
            this.buttonGo.Text = "GO";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(324, 374);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(68, 27);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxSourceB
            // 
            this.textBoxSourceB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceB.Location = new System.Drawing.Point(6, 74);
            this.textBoxSourceB.Name = "textBoxSourceB";
            this.textBoxSourceB.Size = new System.Drawing.Size(400, 20);
            this.textBoxSourceB.TabIndex = 3;
            // 
            // textBoxTargetB
            // 
            this.textBoxTargetB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetB.Location = new System.Drawing.Point(7, 74);
            this.textBoxTargetB.Name = "textBoxTargetB";
            this.textBoxTargetB.Size = new System.Drawing.Size(400, 20);
            this.textBoxTargetB.TabIndex = 3;
            // 
            // FormReproject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(478, 409);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.radioButtonOStoWGS84);
            this.Controls.Add(this.radioButtonWGS84toOS);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(494, 445);
            this.Name = "FormReproject";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Reproject Map";
            this.Load += new System.EventHandler(this.FormReproject_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonWGS84toOS;
        private System.Windows.Forms.RadioButton radioButtonOStoWGS84;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxSourceA;
        private System.Windows.Forms.Button buttonSourceBrowse;
        private System.Windows.Forms.ComboBox comboBoxSourceMap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxTargetA;
        private System.Windows.Forms.Button buttonTargetBrowse;
        private System.Windows.Forms.ComboBox comboBoxTargetMap;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxSourceB;
        private System.Windows.Forms.TextBox textBoxTargetB;
    }
}