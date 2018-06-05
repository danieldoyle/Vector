using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UIThreadMonitor
{
	public partial class InspectorUI : Form
	{
		private string lastSelectedKey;
		private BindingSource bindingSource;
		private bool updatingBindingSource;
		private bool paused;

		public InspectorUI()
		{
			InitializeComponent();
		}

		public void Update(IEnumerable<UIDetails> stacks)
		{
			if (!updatingBindingSource && !paused)
			{
				updatingBindingSource = true;
				bindingSource = new BindingSource();
				var table = new DataTable("Stack List");
				table.Columns.Add("Key");
				table.Columns.Add("Count");
				table.Columns.Add("Stack");
				table.Columns.Add("Min");
				table.Columns.Add("Max");
				table.Columns.Add("Avg");


				foreach (var stackinfo in stacks)
				{
					table.Rows.Add(new object[] { stackinfo.Key, stackinfo.Count, stackinfo.Stack, stackinfo.Minimum, stackinfo.Maximum, stackinfo.Average });
				}

				table.AcceptChanges();
				DataView view = new DataView(table);
				
				bindingSource.DataSource = view;

				dataGridView1.SuspendLayout();
				dataGridView1.DataSource = bindingSource;
				dataGridView1.Refresh();
				dataGridView1.ResumeLayout();
			}
		}

		public void Update(IEnumerable<GCDetails> updates)
		{
			foreach(var update in updates)
			{
				listBox.Items.Add(string.Format("Garbage collection Notification at {0} - {1}\n", update.Timestamp, update.Message));
			}			
		}

		public int Threshold
		{
			get { return Convert.ToInt32(sampleRate.Value); }
		}

		public int UpdateRate
		{
			get { return Convert.ToInt32(updateRate.Value); }
		}

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (!updatingBindingSource)
			{
				lastSelectedKey = dataGridView1.Rows[e.RowIndex].Cells["Key"].Value.ToString();
				textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Stack"].Value.ToString();
			}

		}

		private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			updatingBindingSource = false;
			if (!String.IsNullOrEmpty(lastSelectedKey) && e.ListChangedType == ListChangedType.Reset)
			{
				int row = bindingSource.Find("Key", lastSelectedKey);
				dataGridView1.BeginInvoke((MethodInvoker)delegate ()
				{
					dataGridView1.Rows[row].Selected = true;
					dataGridView1.CurrentCell = dataGridView1[0, row];
				});
			}
		}

		private void btnPause_Click(object sender, EventArgs e)
		{
			paused = !paused;
			btnPause.ImageIndex = paused ? 1 : 0;
		}

		private void UIThreadInspector_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = (MessageBox.Show("Closing will stop capturing UI Thread usage information, do you want to continue?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No);			
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveToCSV(dataGridView1);
		}

		private void SaveToCSV(DataGridView DGV)
		{
			string filename = "";
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "CSV (*.csv)|*.csv";
			sfd.FileName = string.Format("UIThreadInspector-{0}.csv", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(filename))
				{
					try
					{
						File.Delete(filename);
					}
					catch (IOException ex)
					{
						MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
					}
				}

				var sb = new StringBuilder();

				var headers = dataGridView1.Columns.Cast<DataGridViewColumn>();
				sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					var cells = row.Cells.Cast<DataGridViewCell>();
					sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
				}
				
				System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
				MessageBox.Show("Your file was created and is ready for use.");
			}
		}
	}
}
