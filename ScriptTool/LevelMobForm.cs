using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ScriptTool
{
	public partial class LevelMobForm : Form
	{
		public static BindingList<Mob> mobRoster { get; private set; } = new BindingList<Mob>();

		public LevelMobForm(bool _resetRoster)
		{
			this.InitializeComponent();

			if (_resetRoster)
			{
				LevelMobForm.mobRoster.Clear();
			}
		}

		private void LevelMobForm_Load(object _sender, EventArgs _args)
		{
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
			dataGridViewComboBoxColumn.DataSource = MobList.ids;
			dataGridViewComboBoxColumn.HeaderText = "mobName";
			dataGridViewComboBoxColumn.DataPropertyName = "mobName";
			this.dataGridView.Columns.Add(dataGridViewComboBoxColumn);
			this.dataGridView.DataSource = LevelMobForm.mobRoster;
			this.dataGridView.Refresh();
		}

		private void cmdClear_Click(object _sender, EventArgs _args)
		{
			if (MessageBox.Show("You are about to clear every mobs from the current roster, are you sure you want to remove everything (cannot revert)?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				LevelMobForm.mobRoster.Clear();
				this.dataGridView.Refresh();
			}
		}
		
		private void cmdOk_Click(object _sender, EventArgs _args)
		{
			base.Close();
			base.Dispose();
		}
	}
}
