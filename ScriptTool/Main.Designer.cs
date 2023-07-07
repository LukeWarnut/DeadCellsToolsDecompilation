namespace ScriptTool
{
	public partial class Main : global::System.Windows.Forms.Form
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
			this.panel = new global::System.Windows.Forms.Panel();
			this.cmdCopyToClipboard = new global::System.Windows.Forms.Button();
			this.cmdGenerateMobRoster = new global::System.Windows.Forms.Button();
			this.cmdNewScript = new global::System.Windows.Forms.Button();
			this.richTextBox = new global::System.Windows.Forms.RichTextBox();
			this.menuStrip = new global::System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.quitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.lblCurrentFileName = new global::System.Windows.Forms.Label();
			this.cmdPickColor = new global::System.Windows.Forms.Button();
			this.colorDialog = new global::System.Windows.Forms.ColorDialog();
			this.panel.SuspendLayout();
			this.menuStrip.SuspendLayout();
			base.SuspendLayout();
			this.panel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left;
			this.panel.Controls.Add(this.cmdPickColor);
			this.panel.Controls.Add(this.cmdCopyToClipboard);
			this.panel.Controls.Add(this.cmdGenerateMobRoster);
			this.panel.Controls.Add(this.cmdNewScript);
			this.panel.Location = new global::System.Drawing.Point(12, 60);
			this.panel.Name = "panel";
			this.panel.Size = new global::System.Drawing.Size(396, 609);
			this.panel.TabIndex = 0;
			this.cmdCopyToClipboard.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.cmdCopyToClipboard.Location = new global::System.Drawing.Point(256, 583);
			this.cmdCopyToClipboard.Name = "cmdCopyToClipboard";
			this.cmdCopyToClipboard.Size = new global::System.Drawing.Size(137, 23);
			this.cmdCopyToClipboard.TabIndex = 2;
			this.cmdCopyToClipboard.Text = "Copy script to clipboard";
			this.cmdCopyToClipboard.UseVisualStyleBackColor = true;
			this.cmdCopyToClipboard.Click += new global::System.EventHandler(this.cmdCopyToClipboard_Click);
			this.cmdGenerateMobRoster.Location = new global::System.Drawing.Point(5, 33);
			this.cmdGenerateMobRoster.Name = "cmdGenerateMobRoster";
			this.cmdGenerateMobRoster.Size = new global::System.Drawing.Size(141, 23);
			this.cmdGenerateMobRoster.TabIndex = 1;
			this.cmdGenerateMobRoster.Text = "Generate mob roster";
			this.cmdGenerateMobRoster.UseVisualStyleBackColor = true;
			this.cmdGenerateMobRoster.Click += new global::System.EventHandler(this.cmdGenerateMobRoster_Click);
			this.cmdNewScript.Location = new global::System.Drawing.Point(5, 4);
			this.cmdNewScript.Name = "cmdNewScript";
			this.cmdNewScript.Size = new global::System.Drawing.Size(141, 23);
			this.cmdNewScript.TabIndex = 0;
			this.cmdNewScript.Text = "Generate default script";
			this.cmdNewScript.UseVisualStyleBackColor = true;
			this.cmdNewScript.Click += new global::System.EventHandler(this.generateDefaultScript_Click);
			this.richTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.richTextBox.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.richTextBox.Location = new global::System.Drawing.Point(418, 60);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.ReadOnly = true;
			this.richTextBox.Size = new global::System.Drawing.Size(834, 609);
			this.richTextBox.TabIndex = 1;
			this.richTextBox.Text = "";
			this.richTextBox.WordWrap = false;
			this.menuStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.fileToolStripMenuItem });
			this.menuStrip.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new global::System.Drawing.Size(1264, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.newToolStripMenuItem, this.openToolStripMenuItem, this.quitToolStripMenuItem });
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new global::System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			//this.newToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131150;
			this.newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
			this.newToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new global::System.EventHandler(this.newToolStripMenuItem_Click);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            //this.openToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131151;
            this.openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            this.openToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new global::System.EventHandler(this.openToolStripMenuItem_Click);
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new global::System.EventHandler(this.quitToolStripMenuItem_Click);
			this.lblCurrentFileName.AutoSize = true;
			this.lblCurrentFileName.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.lblCurrentFileName.Location = new global::System.Drawing.Point(13, 33);
			this.lblCurrentFileName.Name = "lblCurrentFileName";
			this.lblCurrentFileName.Size = new global::System.Drawing.Size(0, 19);
			this.lblCurrentFileName.TabIndex = 3;
			this.cmdPickColor.Location = new global::System.Drawing.Point(5, 62);
			this.cmdPickColor.Name = "cmdPickColor";
			this.cmdPickColor.Size = new global::System.Drawing.Size(141, 23);
			this.cmdPickColor.TabIndex = 3;
			this.cmdPickColor.Text = "Get script color";
			this.cmdPickColor.UseVisualStyleBackColor = true;
			this.cmdPickColor.Click += new global::System.EventHandler(this.cmdPickColor_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1264, 681);
			base.Controls.Add(this.lblCurrentFileName);
			base.Controls.Add(this.richTextBox);
			base.Controls.Add(this.panel);
			base.Controls.Add(this.menuStrip);
			base.MainMenuStrip = this.menuStrip;
			base.Name = "Main";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ScriptTool";
			base.Load += new global::System.EventHandler(this.Main_Load);
			this.panel.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		
		private global::System.ComponentModel.IContainer components;
		private global::System.Windows.Forms.Panel panel;
		private global::System.Windows.Forms.RichTextBox richTextBox;
		private global::System.Windows.Forms.MenuStrip menuStrip;
		private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private global::System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private global::System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private global::System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private global::System.Windows.Forms.Label lblCurrentFileName;
		private global::System.Windows.Forms.Button cmdNewScript;
		private global::System.Windows.Forms.Button cmdGenerateMobRoster;
		private global::System.Windows.Forms.Button cmdCopyToClipboard;
		private global::System.Windows.Forms.Button cmdPickColor;
		private global::System.Windows.Forms.ColorDialog colorDialog;
	}
}
