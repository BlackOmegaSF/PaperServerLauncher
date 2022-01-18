namespace PaperServerLauncher
{
    partial class ServerLauncher
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
            this.btnStartServer = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblServerJar = new System.Windows.Forms.Label();
            this.txtServerJar = new System.Windows.Forms.TextBox();
            this.btnBrowseJar = new System.Windows.Forms.Button();
            this.grpJavaFlags = new System.Windows.Forms.GroupBox();
            this.cbxAikarsFlags = new System.Windows.Forms.CheckBox();
            this.lblCurrentRam = new System.Windows.Forms.Label();
            this.cbRamUnits = new System.Windows.Forms.ComboBox();
            this.numRAM = new System.Windows.Forms.NumericUpDown();
            this.lblRAM = new System.Windows.Forms.Label();
            this.grpPlugins = new System.Windows.Forms.GroupBox();
            this.txtPluginStatus = new System.Windows.Forms.TextBox();
            this.cbxUpdatePlugins = new System.Windows.Forms.CheckBox();
            this.lblMinRam = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.grpJavaFlags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRAM)).BeginInit();
            this.grpPlugins.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(12, 339);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(320, 30);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(344, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // lblServerJar
            // 
            this.lblServerJar.AutoSize = true;
            this.lblServerJar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerJar.Location = new System.Drawing.Point(12, 27);
            this.lblServerJar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblServerJar.Name = "lblServerJar";
            this.lblServerJar.Size = new System.Drawing.Size(69, 16);
            this.lblServerJar.TabIndex = 2;
            this.lblServerJar.Text = "Server Jar";
            // 
            // txtServerJar
            // 
            this.txtServerJar.Location = new System.Drawing.Point(12, 46);
            this.txtServerJar.Name = "txtServerJar";
            this.txtServerJar.Size = new System.Drawing.Size(254, 20);
            this.txtServerJar.TabIndex = 3;
            this.txtServerJar.TextChanged += new System.EventHandler(this.txtServerJar_TextChanged);
            // 
            // btnBrowseJar
            // 
            this.btnBrowseJar.Location = new System.Drawing.Point(272, 46);
            this.btnBrowseJar.Name = "btnBrowseJar";
            this.btnBrowseJar.Size = new System.Drawing.Size(60, 20);
            this.btnBrowseJar.TabIndex = 4;
            this.btnBrowseJar.Text = "Browse...";
            this.btnBrowseJar.UseVisualStyleBackColor = true;
            this.btnBrowseJar.Click += new System.EventHandler(this.btnBrowseJar_Click);
            // 
            // grpJavaFlags
            // 
            this.grpJavaFlags.Controls.Add(this.lblMinRam);
            this.grpJavaFlags.Controls.Add(this.cbxAikarsFlags);
            this.grpJavaFlags.Controls.Add(this.lblCurrentRam);
            this.grpJavaFlags.Controls.Add(this.cbRamUnits);
            this.grpJavaFlags.Controls.Add(this.numRAM);
            this.grpJavaFlags.Controls.Add(this.lblRAM);
            this.grpJavaFlags.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpJavaFlags.Location = new System.Drawing.Point(12, 73);
            this.grpJavaFlags.Name = "grpJavaFlags";
            this.grpJavaFlags.Size = new System.Drawing.Size(320, 98);
            this.grpJavaFlags.TabIndex = 5;
            this.grpJavaFlags.TabStop = false;
            this.grpJavaFlags.Text = "Java Flags";
            // 
            // cbxAikarsFlags
            // 
            this.cbxAikarsFlags.AutoSize = true;
            this.cbxAikarsFlags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAikarsFlags.Location = new System.Drawing.Point(6, 72);
            this.cbxAikarsFlags.Name = "cbxAikarsFlags";
            this.cbxAikarsFlags.Size = new System.Drawing.Size(107, 17);
            this.cbxAikarsFlags.TabIndex = 9;
            this.cbxAikarsFlags.Text = "Use Aikar\'s Flags";
            this.cbxAikarsFlags.UseVisualStyleBackColor = true;
            // 
            // lblCurrentRam
            // 
            this.lblCurrentRam.AutoSize = true;
            this.lblCurrentRam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRam.Location = new System.Drawing.Point(7, 46);
            this.lblCurrentRam.Name = "lblCurrentRam";
            this.lblCurrentRam.Size = new System.Drawing.Size(138, 13);
            this.lblCurrentRam.TabIndex = 8;
            this.lblCurrentRam.Text = "Recommend ? of ? installed";
            // 
            // cbRamUnits
            // 
            this.cbRamUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRamUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRamUnits.FormattingEnabled = true;
            this.cbRamUnits.Items.AddRange(new object[] {
            "MB",
            "GB"});
            this.cbRamUnits.Location = new System.Drawing.Point(123, 22);
            this.cbRamUnits.Name = "cbRamUnits";
            this.cbRamUnits.Size = new System.Drawing.Size(48, 21);
            this.cbRamUnits.TabIndex = 7;
            this.cbRamUnits.SelectedIndexChanged += new System.EventHandler(this.cbRamUnits_SelectedIndexChanged);
            // 
            // numRAM
            // 
            this.numRAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numRAM.Location = new System.Drawing.Point(44, 22);
            this.numRAM.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.numRAM.Name = "numRAM";
            this.numRAM.Size = new System.Drawing.Size(73, 20);
            this.numRAM.TabIndex = 6;
            this.numRAM.ValueChanged += new System.EventHandler(this.numRAM_ValueChanged);
            this.numRAM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numRAM_KeyPress);
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRAM.Location = new System.Drawing.Point(7, 24);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(31, 13);
            this.lblRAM.TabIndex = 0;
            this.lblRAM.Text = "RAM";
            // 
            // grpPlugins
            // 
            this.grpPlugins.Controls.Add(this.txtPluginStatus);
            this.grpPlugins.Controls.Add(this.cbxUpdatePlugins);
            this.grpPlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPlugins.Location = new System.Drawing.Point(12, 178);
            this.grpPlugins.Name = "grpPlugins";
            this.grpPlugins.Size = new System.Drawing.Size(320, 155);
            this.grpPlugins.TabIndex = 6;
            this.grpPlugins.TabStop = false;
            this.grpPlugins.Text = "Plugins";
            // 
            // txtPluginStatus
            // 
            this.txtPluginStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPluginStatus.Location = new System.Drawing.Point(6, 43);
            this.txtPluginStatus.Multiline = true;
            this.txtPluginStatus.Name = "txtPluginStatus";
            this.txtPluginStatus.ReadOnly = true;
            this.txtPluginStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPluginStatus.Size = new System.Drawing.Size(308, 106);
            this.txtPluginStatus.TabIndex = 1;
            this.txtPluginStatus.TextChanged += new System.EventHandler(this.txtPluginStatus_TextChanged);
            this.txtPluginStatus.GotFocus += new System.EventHandler(this.txtPluginStatus_GotFocus);
            // 
            // cbxUpdatePlugins
            // 
            this.cbxUpdatePlugins.AutoSize = true;
            this.cbxUpdatePlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxUpdatePlugins.Location = new System.Drawing.Point(7, 20);
            this.cbxUpdatePlugins.Name = "cbxUpdatePlugins";
            this.cbxUpdatePlugins.Size = new System.Drawing.Size(210, 17);
            this.cbxUpdatePlugins.TabIndex = 0;
            this.cbxUpdatePlugins.Text = "Automatically update supported plugins";
            this.cbxUpdatePlugins.UseVisualStyleBackColor = true;
            // 
            // lblMinRam
            // 
            this.lblMinRam.AutoSize = true;
            this.lblMinRam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinRam.ForeColor = System.Drawing.Color.Red;
            this.lblMinRam.Location = new System.Drawing.Point(178, 24);
            this.lblMinRam.Name = "lblMinRam";
            this.lblMinRam.Size = new System.Drawing.Size(112, 13);
            this.lblMinRam.TabIndex = 10;
            this.lblMinRam.Text = "Minimum RAM is 2GB!";
            this.lblMinRam.Visible = false;
            // 
            // ServerLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 381);
            this.Controls.Add(this.grpPlugins);
            this.Controls.Add(this.grpJavaFlags);
            this.Controls.Add(this.btnBrowseJar);
            this.Controls.Add(this.txtServerJar);
            this.Controls.Add(this.lblServerJar);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServerLauncher";
            this.Text = "Paper Server Launcher";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpJavaFlags.ResumeLayout(false);
            this.grpJavaFlags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRAM)).EndInit();
            this.grpPlugins.ResumeLayout(false);
            this.grpPlugins.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblServerJar;
        private System.Windows.Forms.TextBox txtServerJar;
        private System.Windows.Forms.Button btnBrowseJar;
        private System.Windows.Forms.GroupBox grpJavaFlags;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.NumericUpDown numRAM;
        private System.Windows.Forms.ComboBox cbRamUnits;
        private System.Windows.Forms.Label lblCurrentRam;
        private System.Windows.Forms.CheckBox cbxAikarsFlags;
        private System.Windows.Forms.GroupBox grpPlugins;
        private System.Windows.Forms.TextBox txtPluginStatus;
        private System.Windows.Forms.CheckBox cbxUpdatePlugins;
        private System.Windows.Forms.Label lblMinRam;
    }
}

