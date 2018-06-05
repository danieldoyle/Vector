using System.Collections.Generic;
using System.Linq;

namespace UIThreadMonitor
{
	public class UIDetails
	{
		private IList<long> executionMs { get; set; }

		public UIDetails(int key, string stack)
		{
			Key = key;
			Stack = stack;
			executionMs = new List<long>();
		}
		public int Key { get; private set; }
		public string Stack { get; private set; }
		public double Average { get; private set; }
		public decimal Minimum { get; private set; }
		public decimal Maximum { get; private set; }
		
		public int Count
		{
			get { return executionMs.Count; }
		}
		public void AddSample(long execution)
		{
			executionMs.Add(execution);
			if (execution < Minimum || Minimum == 0)
			{
				Minimum = execution;
			}
			if (execution > Maximum || Maximum == 0)
			{
				Maximum = execution;
			}
			Average = executionMs.Average();
		}

	}
}
