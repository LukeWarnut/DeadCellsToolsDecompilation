namespace ScriptTool
{
	public partial class LevelMobForm : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.dataGridView = new global::System.Windows.Forms.DataGridView();
			this.cmdClear = new global::System.Windows.Forms.Button();
			this.cmdOk = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView).BeginInit();
			base.SuspendLayout();
			this.dataGridView.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.dataGridView.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Location = new global::System.Drawing.Point(13, 13);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.Size = new global::System.Drawing.Size(965, 369);
			this.dataGridView.TabIndex = 0;
			this.cmdClear.Location = new global::System.Drawing.Point(903, 388);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new global::System.Drawing.Size(75, 23);
			this.cmdClear.TabIndex = 1;
			this.cmdClear.Text = "Clear";
			this.cmdClear.UseVisualStyleBackColor = true;
			this.cmdClear.Click += new global::System.EventHandler(this.cmdClear_Click);
			this.cmdOk.Location = new global::System.Drawing.Point(822, 388);
			this.cmdOk.Name = "cmdOk";
			this.cmdOk.Size = new global::System.Drawing.Size(75, 23);
			this.cmdOk.TabIndex = 2;
			this.cmdOk.Text = "Ok";
			this.cmdOk.UseVisualStyleBackColor = true;
			this.cmdOk.Click += new global::System.EventHandler(this.cmdOk_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(990, 421);
			base.Controls.Add(this.cmdOk);
			base.Controls.Add(this.cmdClear);
			base.Controls.Add(this.dataGridView);
			base.Name = "LevelMobForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LevelMobForm";
			base.Load += new global::System.EventHandler(this.LevelMobForm_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView).EndInit();
			base.ResumeLayout(false);
		}
		
		private global::System.ComponentModel.IContainer components;
		private global::System.Windows.Forms.DataGridView dataGridView;
		private global::System.Windows.Forms.Button cmdClear;
		private global::System.Windows.Forms.Button cmdOk;
	}
}
