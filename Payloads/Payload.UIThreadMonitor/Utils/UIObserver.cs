using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace UIThreadMonitor
{
	public class UIObserver
	{
		private IDictionary<int, UIDetails> uiDetails = new Dictionary<int, UIDetails>();
		private System.Threading.Timer watchTimer;
		private System.Threading.Timer uiTimer;
		private Stopwatch sw;
		private Thread mainThread;
		private Form mainForm;
		private object mutex = new object();
		private bool updateAvailable;


		public UIObserver(Form mainForm)
		{
			Threshold = 250;

			this.mainForm = mainForm;
			mainForm.Invoke((MethodInvoker)(() =>
			{
				mainThread = Thread.CurrentThread;
			}));
		}

		public void Start()
		{
			sw = new Stopwatch();
			uiTimer = new System.Threading.Timer(state =>
			{
				try
				{
					mainForm.Invoke((MethodInvoker)(() =>
					{
						lock (sw)
						{
							sw.Restart();
						}
					}));
				}
				catch (Exception ex)
				{
					Trace.WriteLine(string.Format("Fudging hell {0}", ex));
				}

			}, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));

			watchTimer = new System.Threading.Timer(state =>
			{
				try
				{
					lock (sw)
					{
						var latency = sw.ElapsedMilliseconds;
						if (latency > Threshold)
						{
							RecordUIThread(latency);
						}
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine(string.Format("Fudging hell {0}", ex));
				}

			}, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));
		}

		public void Stop()
		{

			if (uiTimer != null)
			{
				uiTimer.Change(Timeout.Infinite, Timeout.Infinite);
				uiTimer.Dispose();
				uiTimer = null;
			}

			if (watchTimer != null)
			{
				watchTimer.Change(Timeout.Infinite, Timeout.Infinite);
				watchTimer.Dispose();
				watchTimer = null;
			}

			if (uiDetails != null)
			{
				uiDetails.Clear();
				uiDetails = null;
			}

			if (sw != null)
			{
				sw.Stop();
				sw = null;
			}
		}

		public int Threshold { get; set; }

		public IEnumerable<UIDetails> Details()
		{
			IEnumerable<UIDetails> result = null;
			lock (mutex)
			{
				if (updateAvailable)
				{
					result = uiDetails.Values.ToList().OrderByDescending(x => x.Average).ThenByDescending(x => x.Count);					
					updateAvailable = false;
				}
			}
			return result;
		}

		private void RecordUIThread(long latency)
		{
			lock (mutex)
			{
				var stackTrace = GetStackTrace(mainThread).ToString();
				if (!stackTrace.Contains("UIThreadObserver"))
				{
					var stackHash = stackTrace.GetHashCode();
					Trace.WriteLine(string.Format("Hash {0} Value {1}", stackHash, stackTrace));
					UIDetails stackStats;
					if (!uiDetails.TryGetValue(stackHash, out stackStats))
					{
						stackStats = new UIDetails(stackHash, stackTrace);
						uiDetails.Add(stackHash, stackStats);
					}
					stackStats.AddSample(latency);
					updateAvailable = true;
				}
			}
		}

		private StackTrace GetStackTrace(Thread targetThread)
		{
			using (ManualResetEvent fallbackThreadReady = new ManualResetEvent(false), exitedSafely = new ManualResetEvent(false))
			{
				Thread fallbackThread = new Thread(delegate ()
				{
					fallbackThreadReady.Set();
					while (!exitedSafely.WaitOne(200))
					{
						try
						{
							targetThread.Resume();
						}
						catch (Exception) {/*Whatever happens, do never stop to resume the target-thread regularly until the main-thread has exited safely.*/}
					}
				});
				fallbackThread.Name = "GetStackFallbackThread";
				try
				{
					fallbackThread.Start();
					fallbackThreadReady.WaitOne();
					//From here, you have about 200ms to get the stack-trace.
					targetThread.Suspend();
					StackTrace trace = null;
					try
					{
						trace = new StackTrace(targetThread, true);
					}
					catch (ThreadStateException)
					{
						//failed to get stack trace, since the fallback-thread resumed the thread
						//possible reasons:
						//1.) This thread was just too slow (not very likely)
						//2.) The deadlock ocurred and the fallbackThread rescued the situation.
						//In both cases just return null.
					}
					try
					{
						targetThread.Resume();
					}
					catch (ThreadStateException) {/*Thread is running again already*/}
					return trace;
				}
				finally
				{
					//Just signal the backup-thread to stop.
					exitedSafely.Set();
					//Join the thread to avoid disposing "exited safely" too early. And also make sure that no leftover threads are cluttering iis by accident.
					fallbackThread.Join();
				}
			}
		}

	}
}
