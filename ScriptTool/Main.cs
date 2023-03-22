using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScriptTool
{
	public partial class Main : Form
	{
		public Main()
		{
			this.InitializeComponent();
		}

		private void openToolStripMenuItem_Click(object _sender, EventArgs _args)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "hscript files|*.hx";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.Open(openFileDialog.FileName);
			}
		}

		private void Open(string _fullFileName)
		{
			this.m_CurrentFileName = _fullFileName;
			this.lblCurrentFileName.Text = new FileInfo(this.m_CurrentFileName).Name;
			string text = File.ReadAllText(_fullFileName);
			this.richTextBox.Text = text;
			this.richTextBox.Enabled = true;
			this.panel.Enabled = true;
		}

		private void Main_Load(object _sender, EventArgs _args)
		{
			base.Resize += this.MainResized;

			try
			{
				this.m_Options = Options.FromJson(File.ReadAllText("options.json"));
			}

			catch (FileNotFoundException)
			{
				this.m_Options = null;
			}

			if (this.m_Options == null)
			{
				this.m_Options = new Options();
				this.SetRefCDBPath();
			}

			while (this.m_Options.refCDBPath == "" || !File.Exists(this.m_Options.refCDBPath))
			{
				this.SetRefCDBPath();
			}

			this.m_CDBJson = File.ReadAllText(this.m_Options.refCDBPath);
			MobList.InitMobList(this.m_CDBJson);
			this.MainResized(null, null);
		}

		private void SetRefCDBPath()
		{
			MessageBox.Show("The reference CDB has not be found, please pick the reference CDB in the next dialog", "CDB not found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "data.cdb|data.cdb";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.m_Options.refCDBPath = openFileDialog.FileName;
				this.SaveOptions();
			}
		}

		private void SaveOptions()
		{
			if (this.m_Options != null)
			{
				File.WriteAllText("options.json", this.m_Options.ToJson());
			}
		}
		
		private void MainResized(object _sender, EventArgs _args)
		{
			int num = base.Width / 8;
			this.panel.Width = num - this.panel.Left - 2;
			int right = this.richTextBox.Right;
			this.richTextBox.Left = num + 2;
			this.richTextBox.Width = right - this.richTextBox.Left;
		}
		
		private void quitToolStripMenuItem_Click(object _sender, EventArgs _args)
		{
			Application.Exit();
		}
		
		private void newToolStripMenuItem_Click(object _sender, EventArgs _args)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "hscript files|*.hx";

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				File.WriteAllText(saveFileDialog.FileName, ScriptWriter.instance.WriteWholeScript());
				this.Open(saveFileDialog.FileName);
			}
		}
		
		private void generateDefaultScript_Click(object _sender, EventArgs _args)
		{
			this.lblCurrentFileName.Text = "";
			this.richTextBox.Text = ScriptWriter.instance.WriteWholeScript();
		}
		
		private void cmdGenerateMobRoster_Click(object _sender, EventArgs _args)
		{
			this.lblCurrentFileName.Text = "";
			new LevelMobForm(false).ShowDialog(this);
			this.richTextBox.Text = ScriptWriter.instance.WriteWholeScript();
		}
		
		private void cmdCopyToClipboard_Click(object _sender, EventArgs _args)
		{
			Clipboard.SetText(this.richTextBox.Text);
		}
		
		private void cmdPickColor_Click(object _sender, EventArgs _args)
		{
			if (this.colorDialog.ShowDialog() == DialogResult.OK)
			{
				Color color = this.colorDialog.Color;
				uint num = (uint)(((int)color.R << 16) | ((int)color.G << 8) | (int)color.B);
				Clipboard.SetText(num.ToString());
				MessageBox.Show("The value is " + num + " and has been copied to the clipboard. Use ctrl + V in your script to put the value", "Color copied", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		
		private Options m_Options;
		private string m_CDBJson = "";
		private string m_CurrentFileName = "";
	}
}
