using System;
using System.Diagnostics;
using ProcessInjector;


namespace Vector
{
	public class InjectionHelper
	{
		public static bool Inject(Process process, InjectionPayload payload, out string resultMessage)
		{
			var result = false;

			try
			{
				if (process == null || payload == null)
				{
					resultMessage = "Process and payload cannot be null";
					return result;
				}
				
				if (!InjectionSearch.CheckInjected(process, payload) || payload.CanLoadMultiple)
				{
					Injector.Inject(process.MainWindowHandle, payload.Assembly.Location, payload.Class, payload.Method);
					result = InjectionSearch.CheckInjected(process, payload);
					if (result)
					{
						resultMessage = string.Format("Successfully injected payload {0} for process {1} (PID = {2})", payload.Name, process.ProcessName, process.Id);
					}
					else
					{
						resultMessage = string.Format("Failed to inject payload {0} for process {1} (PID = {2})", payload.Name, process.ProcessName, process.Id);
					}
				}
				else
				{
					resultMessage = string.Format("Payload {0} already exists in process {1} (PID = {2})", payload.Name, process.ProcessName, process.Id);
				}
			}
			catch (Exception ex)
			{
				resultMessage = string.Format("An unexpected error occurred trying to inject the payload: {0}", ex);
			}

			return result;
		}		
	}
}
