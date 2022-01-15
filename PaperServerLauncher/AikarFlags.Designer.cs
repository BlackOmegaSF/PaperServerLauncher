namespace PaperServerLauncher
{
    partial class AikarFlagsForm
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
            this.dgvFlags = new System.Windows.Forms.DataGridView();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlags)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFlags
            // 
            this.dgvFlags.AllowUserToAddRows = false;
            this.dgvFlags.AllowUserToDeleteRows = false;
            this.dgvFlags.AllowUserToResizeColumns = false;
            this.dgvFlags.AllowUserToResizeRows = false;
            this.dgvFlags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Enable,
            this.Flag,
            this.Value});
            this.dgvFlags.Location = new System.Drawing.Point(12, 40);
            this.dgvFlags.Name = "dgvFlags";
            this.dgvFlags.Size = new System.Drawing.Size(373, 150);
            this.dgvFlags.TabIndex = 0;
            // 
            // Enable
            // 
            this.Enable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            this.Enable.Width = 46;
            // 
            // Flag
            // 
            this.Flag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Flag.HeaderText = "Flag";
            this.Flag.Name = "Flag";
            this.Flag.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // AikarFlagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 459);
            this.Controls.Add(this.dgvFlags);
            this.Name = "AikarFlagsForm";
            this.Text = "Customize Aikar\'s Flags";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlags)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFlags;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}