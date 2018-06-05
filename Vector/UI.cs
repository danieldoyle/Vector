using Payload.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Vector
{
	public partial class UI : Form
	{
		private IDictionary<string, InjectionPayload> payloadCache;

		private const string ProcessColumnName = "Name";
		private const string ProcessColumnPID = "Id";

		private const string PayloadColumnName = "Name";
		private const string PayloadColumnDesc = "Description";
		

		public UI()
		{
			InitializeComponent();
			RefreshProcessGrid();
			RefreshPayloadGrid();
		}

		private void BuildPayloadCache()
		{
			payloadCache = new Dictionary<string, InjectionPayload>();
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var di = new DirectoryInfo(path);
			foreach (var file in di.GetFiles("*.dll"))
			{
				try
				{
					var nextAssembly = Assembly.LoadFrom(file.FullName);					
					Dictionary<string, MethodInfo> methods = nextAssembly
							.GetTypes()
							.SelectMany(x => x.GetMethods())
							.Where(y => y.GetCustomAttributes().OfType<PayloadAttribute>().Any())
							.ToDictionary(z => z.Name);

					foreach (var method in methods.Values)
					{
						var payload = (PayloadAttribute)method.GetCustomAttributes(typeof(PayloadAttribute), true)[0];

						payloadCache[payload.Name] = new InjectionPayload()
						{
							Name = payload.Name,
							Description = payload.Description,
							CanLoadMultiple = payload.CanLoadMultiple,
							Assembly = method.DeclaringType.Assembly,
							Class = method.DeclaringType.FullName,
							Method = method.Name
						};						
					}					
				}
				catch (BadImageFormatException)
				{
					// Not a .net assembly  - ignore
				}
			}
		}

		private void RefreshPayloadGrid()
		{
			BuildPayloadCache();

			BindingSource source = new BindingSource();
			var table = new DataTable();

			table.Columns.Add(PayloadColumnName);
			table.Columns.Add(PayloadColumnDesc);

			foreach (var item in payloadCache.Values)
			{
				table.Rows.Add(new object[] { item.Name, item.Description });
			}

			table.AcceptChanges();
			DataView view = new DataView(table);
			source.DataSource = view;

			int scroll = payloadGrid.FirstDisplayedScrollingRowIndex;
			payloadGrid.DataSource = source;

			if (scroll != -1)
				payloadGrid.FirstDisplayedScrollingRowIndex = scroll;
		}

		private void RefreshProcessGrid()
		{
			BindingSource source = new BindingSource();
			var table = new DataTable();

			Process[] processes = Process.GetProcesses();

			table.Columns.Add(ProcessColumnName);
			table.Columns.Add(ProcessColumnPID);

			for (int i = 0; i < processes.Length; ++i)
			{
				table.Rows.Add(new object[] { processes[i].ProcessName, processes[i].Id });
			}

			table.AcceptChanges();
			DataView view = new DataView(table, "Name LIKE '%Eze%' OR Name = 'Analytics'", "Name Desc", DataViewRowState.CurrentRows);
			source.DataSource = view;

			int scroll = processGrid.FirstDisplayedScrollingRowIndex;
			processGrid.DataSource = source;

			if (scroll != -1)
				processGrid.FirstDisplayedScrollingRowIndex = scroll;
		}

		private InjectionPayload GetSelectedPayload()
		{
			try
			{
				if (payloadGrid.SelectedRows.Count > 0)
				{
					if (payloadCache.TryGetValue(payloadGrid.SelectedRows[0].Cells[PayloadColumnName].Value.ToString(), out InjectionPayload result))
					{
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging Hell: {0}", ex));
			}
			return null;
		}

		private Process GetSelectedProcess()
		{
			try
			{
				if (processGrid.SelectedRows.Count > 0)
				{
					if (int.TryParse(processGrid.SelectedRows[0].Cells[ProcessColumnPID].Value.ToString(), out int pid))
					{
						return Process.GetProcessById(pid);
					}
				}

			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging Hell: {0}", ex));
			}
			return null;
		}

		private void btnInject_Click(object sender, EventArgs e)
		{
			string resultMessage;
			if (InjectionHelper.Inject(GetSelectedProcess(), GetSelectedPayload(), out resultMessage))
			{
				MessageBox.Show(resultMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show(resultMessage, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnRefreshProcess_Click(object sender, EventArgs e)
		{
			RefreshProcessGrid();
		}

		private void btnRefreshPayload_Click(object sender, EventArgs e)
		{
			RefreshPayloadGrid();
		}
	}
}
