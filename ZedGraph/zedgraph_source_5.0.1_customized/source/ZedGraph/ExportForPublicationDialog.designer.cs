namespace ZedGraph
{
    partial class ExportForPublicationDialog
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
			  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForPublicationDialog));
			  this.label1 = new System.Windows.Forms.Label();
			  this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			  this.m_DpiUpDown = new System.Windows.Forms.NumericUpDown();
			  this.label2 = new System.Windows.Forms.Label();
			  this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			  this.m_WidthUpDown = new System.Windows.Forms.NumericUpDown();
			  this.label4 = new System.Windows.Forms.Label();
			  this.m_HeightUpDown = new System.Windows.Forms.NumericUpDown();
			  this.m_InchesRadio = new System.Windows.Forms.RadioButton();
			  this.m_CmRadio = new System.Windows.Forms.RadioButton();
			  this.m_SaveButton = new System.Windows.Forms.Button();
			  this.tableLayoutPanel1.SuspendLayout();
			  ((System.ComponentModel.ISupportInitialize)(this.m_DpiUpDown)).BeginInit();
			  this.flowLayoutPanel1.SuspendLayout();
			  ((System.ComponentModel.ISupportInitialize)(this.m_WidthUpDown)).BeginInit();
			  ((System.ComponentModel.ISupportInitialize)(this.m_HeightUpDown)).BeginInit();
			  this.SuspendLayout();
			  // 
			  // label1
			  // 
			  resources.ApplyResources(this.label1, "label1");
			  this.label1.Name = "label1";
			  // 
			  // tableLayoutPanel1
			  // 
			  resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			  this.tableLayoutPanel1.Controls.Add(this.m_DpiUpDown, 1, 1);
			  this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			  this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
			  this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			  this.tableLayoutPanel1.Controls.Add(this.m_SaveButton, 1, 3);
			  this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			  // 
			  // m_DpiUpDown
			  // 
			  this.m_DpiUpDown.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
			  resources.ApplyResources(this.m_DpiUpDown, "m_DpiUpDown");
			  this.m_DpiUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			  this.m_DpiUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			  this.m_DpiUpDown.Name = "m_DpiUpDown";
			  this.m_DpiUpDown.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
			  // 
			  // label2
			  // 
			  resources.ApplyResources(this.label2, "label2");
			  this.label2.Name = "label2";
			  // 
			  // flowLayoutPanel1
			  // 
			  this.flowLayoutPanel1.Controls.Add(this.m_WidthUpDown);
			  this.flowLayoutPanel1.Controls.Add(this.label4);
			  this.flowLayoutPanel1.Controls.Add(this.m_HeightUpDown);
			  this.flowLayoutPanel1.Controls.Add(this.m_InchesRadio);
			  this.flowLayoutPanel1.Controls.Add(this.m_CmRadio);
			  resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			  this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			  // 
			  // m_WidthUpDown
			  // 
			  this.m_WidthUpDown.DecimalPlaces = 2;
			  resources.ApplyResources(this.m_WidthUpDown, "m_WidthUpDown");
			  this.m_WidthUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			  this.m_WidthUpDown.Name = "m_WidthUpDown";
			  this.m_WidthUpDown.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			  // 
			  // label4
			  // 
			  resources.ApplyResources(this.label4, "label4");
			  this.label4.Name = "label4";
			  // 
			  // m_HeightUpDown
			  // 
			  this.m_HeightUpDown.DecimalPlaces = 2;
			  resources.ApplyResources(this.m_HeightUpDown, "m_HeightUpDown");
			  this.m_HeightUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			  this.m_HeightUpDown.Name = "m_HeightUpDown";
			  this.m_HeightUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			  // 
			  // m_InchesRadio
			  // 
			  resources.ApplyResources(this.m_InchesRadio, "m_InchesRadio");
			  this.m_InchesRadio.Checked = true;
			  this.m_InchesRadio.Name = "m_InchesRadio";
			  this.m_InchesRadio.TabStop = true;
			  this.m_InchesRadio.UseVisualStyleBackColor = true;
			  // 
			  // m_CmRadio
			  // 
			  resources.ApplyResources(this.m_CmRadio, "m_CmRadio");
			  this.m_CmRadio.Name = "m_CmRadio";
			  this.m_CmRadio.UseVisualStyleBackColor = true;
			  // 
			  // m_SaveButton
			  // 
			  resources.ApplyResources(this.m_SaveButton, "m_SaveButton");
			  this.tableLayoutPanel1.SetColumnSpan(this.m_SaveButton, 2);
			  this.m_SaveButton.Name = "m_SaveButton";
			  this.m_SaveButton.UseVisualStyleBackColor = true;
			  this.m_SaveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
			  // 
			  // ExportForPublicationDialog
			  // 
			  resources.ApplyResources(this, "$this");
			  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			  this.Controls.Add(this.tableLayoutPanel1);
			  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			  this.MaximizeBox = false;
			  this.MinimizeBox = false;
			  this.Name = "ExportForPublicationDialog";
			  this.tableLayoutPanel1.ResumeLayout(false);
			  this.tableLayoutPanel1.PerformLayout();
			  ((System.ComponentModel.ISupportInitialize)(this.m_DpiUpDown)).EndInit();
			  this.flowLayoutPanel1.ResumeLayout(false);
			  this.flowLayoutPanel1.PerformLayout();
			  ((System.ComponentModel.ISupportInitialize)(this.m_WidthUpDown)).EndInit();
			  ((System.ComponentModel.ISupportInitialize)(this.m_HeightUpDown)).EndInit();
			  this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		  private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown m_WidthUpDown;
        private System.Windows.Forms.NumericUpDown m_HeightUpDown;
        private System.Windows.Forms.RadioButton m_InchesRadio;
        private System.Windows.Forms.RadioButton m_CmRadio;
        private System.Windows.Forms.Label label4;
		  private System.Windows.Forms.NumericUpDown m_DpiUpDown;
        private System.Windows.Forms.Button m_SaveButton;
    }
}