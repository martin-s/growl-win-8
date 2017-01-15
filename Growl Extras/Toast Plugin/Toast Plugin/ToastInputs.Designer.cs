namespace Toast_Plugin
{
    partial class ToastInputs
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.highlightTextBoxAppId = new Growl.Destinations.HighlightTextBox();
            this.labelAppId = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDetails
            // 
            this.panelDetails.Controls.Add(this.tableLayoutPanel1);
            // 
            // highlightTextBoxAppId
            // 
            this.highlightTextBoxAppId.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(250)))), ((int)(((byte)(184)))));
            this.highlightTextBoxAppId.Location = new System.Drawing.Point(47, 3);
            this.highlightTextBoxAppId.Name = "highlightTextBoxAppId";
            this.highlightTextBoxAppId.Size = new System.Drawing.Size(196, 20);
            this.highlightTextBoxAppId.TabIndex = 0;
            // 
            // labelAppId
            // 
            this.labelAppId.Location = new System.Drawing.Point(3, 0);
            this.labelAppId.Name = "labelAppId";
            this.labelAppId.Size = new System.Drawing.Size(38, 20);
            this.labelAppId.TabIndex = 1;
            this.labelAppId.Text = "AppId:";
            this.labelAppId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.highlightTextBoxAppId, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelAppId, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 168);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // ToastInputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Name = "ToastInputs";
            this.Size = new System.Drawing.Size(341, 171);
            this.panelDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAppId;
        private Growl.Destinations.HighlightTextBox highlightTextBoxAppId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
