using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UIThreadMonitor
{
	public class GCObserver
	{
		private IList<GCDetails> gcDetails;
		private object mutex = new object();
		private bool updateAvailable;
		private bool running;

		public void Start()
		{
			running = true;
			gcDetails = new List<GCDetails>();
			RegisterForGCNotifications();
		}

		public void Stop()
		{
			CancelGCNotifications();
			running = false;
			gcDetails.Clear();
		}

		public IEnumerable<GCDetails> Details()
		{
			IEnumerable<GCDetails> result = null;
			lock (mutex)
			{
				if (updateAvailable)
				{
					result = gcDetails.ToList();
					updateAvailable = false;
				}
			}
			return result;
		}

		public void Clear()
		{
			lock (mutex)
			{
				gcDetails.Clear();
			}
		}

		private void CancelGCNotifications()
		{
			GC.CancelFullGCNotification();
		}

		private void RegisterForGCNotifications()
		{
			Task.Factory.StartNew(() =>
			{
				GC.RegisterForFullGCNotification(10, 10);
				while (running)
				{
					try
					{
						// Check for a notification of an approaching collection.
						GCNotificationStatus s = GC.WaitForFullGCApproach();
						if (s == GCNotificationStatus.Succeeded)
						{
							Add("GC is coming.  prepare yourself");
						}
						else if (s == GCNotificationStatus.Canceled)
						{
							Add("GC cancelled.  Huzzah.");
							break;
						}
						else
						{
							Add("GC Notification not applicable");
							break;
						}
						// Check for a notification of a completed collection.
						GCNotificationStatus status = GC.WaitForFullGCComplete();
						if (status == GCNotificationStatus.Succeeded)
						{
							Add("GC Complete");
						}
						else if (status == GCNotificationStatus.Canceled)
						{
							Add("GC cancelled.  Huzzah");
							break;
						}
						else
						{
							Add("GC Notification not applicable");
							break;
						}
					}
					catch (Exception ex)
					{
						Trace.WriteLine(string.Format("Fudging hell {0}", ex));
					}

					Thread.Sleep(500);
				}
			}, TaskCreationOptions.LongRunning);

		}

		private void Add(string message)
		{
			lock (mutex)
			{
				gcDetails.Add(new GCDetails() { Timestamp = DateTime.UtcNow, Message = message });
				updateAvailable = true;
			}
		}

	}
}
