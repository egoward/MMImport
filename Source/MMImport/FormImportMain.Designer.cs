namespace Edonica.XMLImport
{
    partial class FormImportMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportMain));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.propertyGridGeneral = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxExportTarget = new System.Windows.Forms.ComboBox();
            this.propertyGridOutput = new System.Windows.Forms.PropertyGrid();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.resetConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemImportOSTNData = new System.Windows.Forms.ToolStripMenuItem();
            this.reprojectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.createTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dropTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.importFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importADirectoryFullOfFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.removeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.createIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dropIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.applyLoadedUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.propertyGridGeneral);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.label2);
            this.splitContainer.Panel2.Controls.Add(this.comboBoxExportTarget);
            this.splitContainer.Panel2.Controls.Add(this.propertyGridOutput);
            this.splitContainer.Size = new System.Drawing.Size(918, 457);
            this.splitContainer.SplitterDistance = 397;
            this.splitContainer.TabIndex = 0;
            // 
            // propertyGridGeneral
            // 
            this.propertyGridGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridGeneral.Location = new System.Drawing.Point(3, 0);
            this.propertyGridGeneral.Name = "propertyGridGeneral";
            this.propertyGridGeneral.Size = new System.Drawing.Size(391, 454);
            this.propertyGridGeneral.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Export Target";
            // 
            // comboBoxExportTarget
            // 
            this.comboBoxExportTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxExportTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExportTarget.FormattingEnabled = true;
            this.comboBoxExportTarget.Location = new System.Drawing.Point(80, 3);
            this.comboBoxExportTarget.Name = "comboBoxExportTarget";
            this.comboBoxExportTarget.Size = new System.Drawing.Size(434, 21);
            this.comboBoxExportTarget.TabIndex = 2;
            this.comboBoxExportTarget.SelectionChangeCommitted += new System.EventHandler(this.comboBoxExportTarget_SelectionChangeCommitted);
            // 
            // propertyGridOutput
            // 
            this.propertyGridOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridOutput.Location = new System.Drawing.Point(2, 22);
            this.propertyGridOutput.Name = "propertyGridOutput";
            this.propertyGridOutput.Size = new System.Drawing.Size(512, 432);
            this.propertyGridOutput.TabIndex = 1;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(918, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfigurationToolStripMenuItem,
            this.loadConfigurationToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveDefaultsToolStripMenuItem,
            this.loadDefaultsToolStripMenuItem,
            this.toolStripSeparator3,
            this.resetConfigurationToolStripMenuItem,
            this.toolStripSeparator9,
            this.toolStripMenuItemImportOSTNData,
            this.reprojectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveConfigurationToolStripMenuItem
            // 
            this.saveConfigurationToolStripMenuItem.Name = "saveConfigurationToolStripMenuItem";
            this.saveConfigurationToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.saveConfigurationToolStripMenuItem.Text = "Save Configuration...";
            this.saveConfigurationToolStripMenuItem.Click += new System.EventHandler(this.saveConfigurationToolStripMenuItem_Click);
            // 
            // loadConfigurationToolStripMenuItem
            // 
            this.loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
            this.loadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.loadConfigurationToolStripMenuItem.Text = "Load Configuration...";
            this.loadConfigurationToolStripMenuItem.Click += new System.EventHandler(this.loadConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // saveDefaultsToolStripMenuItem
            // 
            this.saveDefaultsToolStripMenuItem.Name = "saveDefaultsToolStripMenuItem";
            this.saveDefaultsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.saveDefaultsToolStripMenuItem.Text = "Save defaults";
            this.saveDefaultsToolStripMenuItem.Click += new System.EventHandler(this.saveDefaultsToolStripMenuItem_Click);
            // 
            // loadDefaultsToolStripMenuItem
            // 
            this.loadDefaultsToolStripMenuItem.Name = "loadDefaultsToolStripMenuItem";
            this.loadDefaultsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.loadDefaultsToolStripMenuItem.Text = "Load defaults";
            this.loadDefaultsToolStripMenuItem.Click += new System.EventHandler(this.loadDefaultsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(232, 6);
            // 
            // resetConfigurationToolStripMenuItem
            // 
            this.resetConfigurationToolStripMenuItem.Name = "resetConfigurationToolStripMenuItem";
            this.resetConfigurationToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.resetConfigurationToolStripMenuItem.Text = "Reset to system defaults";
            this.resetConfigurationToolStripMenuItem.Click += new System.EventHandler(this.resetConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(232, 6);
            // 
            // toolStripMenuItemImportOSTNData
            // 
            this.toolStripMenuItemImportOSTNData.Name = "toolStripMenuItemImportOSTNData";
            this.toolStripMenuItemImportOSTNData.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemImportOSTNData.Text = "Import OSTN02 data from CSV";
            this.toolStripMenuItemImportOSTNData.Click += new System.EventHandler(this.toolStripMenuItemImportOSTNData_Click);
            // 
            // reprojectToolStripMenuItem
            // 
            this.reprojectToolStripMenuItem.Name = "reprojectToolStripMenuItem";
            this.reprojectToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.reprojectToolStripMenuItem.Text = "Reproject map";
            this.reprojectToolStripMenuItem.Click += new System.EventHandler(this.reprojectToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testConfigurationToolStripMenuItem,
            this.queryInfoToolStripMenuItem,
            this.toolStripSeparator4,
            this.createTablesToolStripMenuItem,
            this.dropTablesToolStripMenuItem,
            this.toolStripSeparator5,
            this.importFilesToolStripMenuItem,
            this.importADirectoryFullOfFilesToolStripMenuItem,
            this.toolStripSeparator6,
            this.removeDuplicatesToolStripMenuItem,
            this.toolStripSeparator7,
            this.createIndexesToolStripMenuItem,
            this.dropIndexesToolStripMenuItem,
            this.toolStripSeparator8,
            this.applyLoadedUpdatesToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // testConfigurationToolStripMenuItem
            // 
            this.testConfigurationToolStripMenuItem.Name = "testConfigurationToolStripMenuItem";
            this.testConfigurationToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.testConfigurationToolStripMenuItem.Text = "Test Configuration / Connection";
            this.testConfigurationToolStripMenuItem.Click += new System.EventHandler(this.testConfigurationToolStripMenuItem_Click);
            // 
            // queryInfoToolStripMenuItem
            // 
            this.queryInfoToolStripMenuItem.Name = "queryInfoToolStripMenuItem";
            this.queryInfoToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.queryInfoToolStripMenuItem.Text = "Query Info";
            this.queryInfoToolStripMenuItem.Click += new System.EventHandler(this.queryInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(243, 6);
            // 
            // createTablesToolStripMenuItem
            // 
            this.createTablesToolStripMenuItem.Name = "createTablesToolStripMenuItem";
            this.createTablesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.createTablesToolStripMenuItem.Text = "Create tables";
            this.createTablesToolStripMenuItem.Click += new System.EventHandler(this.createTablesToolStripMenuItem_Click);
            // 
            // dropTablesToolStripMenuItem
            // 
            this.dropTablesToolStripMenuItem.Name = "dropTablesToolStripMenuItem";
            this.dropTablesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.dropTablesToolStripMenuItem.Text = "Drop tables";
            this.dropTablesToolStripMenuItem.Click += new System.EventHandler(this.dropTablesToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(243, 6);
            // 
            // importFilesToolStripMenuItem
            // 
            this.importFilesToolStripMenuItem.Name = "importFilesToolStripMenuItem";
            this.importFilesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.importFilesToolStripMenuItem.Text = "Import files...";
            this.importFilesToolStripMenuItem.Click += new System.EventHandler(this.importFilesToolStripMenuItem_Click);
            // 
            // importADirectoryFullOfFilesToolStripMenuItem
            // 
            this.importADirectoryFullOfFilesToolStripMenuItem.Name = "importADirectoryFullOfFilesToolStripMenuItem";
            this.importADirectoryFullOfFilesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.importADirectoryFullOfFilesToolStripMenuItem.Text = "Import directory...";
            this.importADirectoryFullOfFilesToolStripMenuItem.Click += new System.EventHandler(this.importADirectoryFullOfFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(243, 6);
            // 
            // removeDuplicatesToolStripMenuItem
            // 
            this.removeDuplicatesToolStripMenuItem.Name = "removeDuplicatesToolStripMenuItem";
            this.removeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.removeDuplicatesToolStripMenuItem.Text = "Remove Duplicates";
            this.removeDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.removeDuplicatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(243, 6);
            // 
            // createIndexesToolStripMenuItem
            // 
            this.createIndexesToolStripMenuItem.Name = "createIndexesToolStripMenuItem";
            this.createIndexesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.createIndexesToolStripMenuItem.Text = "Create Indexes";
            this.createIndexesToolStripMenuItem.Click += new System.EventHandler(this.createIndexesToolStripMenuItem_Click);
            // 
            // dropIndexesToolStripMenuItem
            // 
            this.dropIndexesToolStripMenuItem.Name = "dropIndexesToolStripMenuItem";
            this.dropIndexesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.dropIndexesToolStripMenuItem.Text = "Drop Indexes";
            this.dropIndexesToolStripMenuItem.Click += new System.EventHandler(this.dropIndexesToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(243, 6);
            // 
            // applyLoadedUpdatesToolStripMenuItem
            // 
            this.applyLoadedUpdatesToolStripMenuItem.Name = "applyLoadedUpdatesToolStripMenuItem";
            this.applyLoadedUpdatesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.applyLoadedUpdatesToolStripMenuItem.Text = "Apply loaded updates";
            this.applyLoadedUpdatesToolStripMenuItem.Click += new System.EventHandler(this.applyLoadedUpdatesToolStripMenuItem_Click);
            // 
            // FormImportMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 481);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportMain";
            this.Text = "XML / GML Import Parameters";
            this.Load += new System.EventHandler(this.FormImportParams_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportParams_FormClosing);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxExportTarget;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dropTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importADirectoryFullOfFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveDefaultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDefaultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dropIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.PropertyGrid propertyGridGeneral;
        private System.Windows.Forms.PropertyGrid propertyGridOutput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem applyLoadedUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportOSTNData;
        private System.Windows.Forms.ToolStripMenuItem reprojectToolStripMenuItem;
    }
}