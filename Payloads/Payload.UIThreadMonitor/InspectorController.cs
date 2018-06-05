using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIThreadMonitor
{
	public sealed class InspectorController
	{		
		private InspectorUI inspectorForm;
		private Form mainForm;
		private bool running = true;
		private int updateRate = 1000;

		private UIObserver uiObserver;
		private GCObserver gcObserver;

		static readonly InspectorController _instance = new InspectorController();
		public static InspectorController Instance
		{
			get
			{
				return _instance;
			}
		}
		InspectorController()
		{
			if (!Initialize())
			{
				Trace.WriteLine("Fudging hell, missing the main UI form");				
			}
		}

		public void Start()
		{			
			Task.Factory.StartNew(() =>
			{

				StartObservers();
				ShowInspectorUI();

				DateTime lastReport = DateTime.UtcNow;
				while (running)
				{
					try
					{
						if ((DateTime.UtcNow - lastReport).TotalMilliseconds > updateRate)
						{
							UpdateInspectorUI();
							lastReport = DateTime.UtcNow;
						}
					}
					catch (Exception ex)
					{
						Trace.WriteLine(string.Format("Fudging hell {0}", ex));
					}
					Thread.Sleep(100);

					mainForm.Invoke((MethodInvoker)(() =>
					{
						updateRate = inspectorForm.UpdateRate;
						running = inspectorForm.Visible;
						uiObserver.Threshold = inspectorForm.Threshold;
					}));
				}

				StopObservers();
				CleanInspectorUI();

			}, TaskCreationOptions.LongRunning);
		}

		private void StartObservers()
		{
			try
			{
				uiObserver = new UIObserver(mainForm);
				gcObserver = new GCObserver();

				uiObserver.Start();
				gcObserver.Start();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging hell {0}", ex));
			}
		}

		private void StopObservers()
		{
			try
			{
				uiObserver.Stop();
				gcObserver.Stop();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging hell {0}", ex));
			}
		}

		private void CleanInspectorUI()
		{
			if (inspectorForm != null)
			{
				inspectorForm.Dispose();
				inspectorForm = null;
			}

			mainForm = null;
		}
		private void UpdateInspectorUI()
		{
			try
			{
				var gcDetails = gcObserver.Details();
				var uiDetails = uiObserver.Details();

				var uiSorted = uiDetails.OrderByDescending(x => x.Average).ThenByDescending(x => x.Count);
				mainForm.Invoke((MethodInvoker)(() =>
				{
					inspectorForm.Update(uiSorted);
					inspectorForm.Update(gcDetails);
				}));
				gcObserver.Clear();
			}
			catch(Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging hell {0}", ex));
			}
		}
		
		private void ShowInspectorUI()
		{
			mainForm.Invoke((MethodInvoker)(() =>
			{
				inspectorForm = new InspectorUI();
				inspectorForm.Show();
			}));
		}

		private bool Initialize()
		{
			try
			{
				if (Application.OpenForms.Count > 0)
				{
					mainForm = Application.OpenForms[0];
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging hell {0}", ex));
			}

			return mainForm != null;
		}
	}
}
