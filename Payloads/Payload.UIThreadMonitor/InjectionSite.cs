using Payload.Common;
using System;
using System.Diagnostics;

namespace UIThreadMonitor
{
	public class InjectionSite
	{
		[Payload(Name = "Payload.UIThreadInspector", Description = "Tool for monitoring UI thread usage")]
		public static void Start()
		{
			try
			{
				InspectorController.Instance.Start();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Fudging hell {0}", ex));
			}
		}
	}
}
