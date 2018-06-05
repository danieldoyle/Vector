using System;

namespace Payload.Common
{
	public class PayloadAttribute : Attribute
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public bool CanLoadMultiple { get; set; }
	}
}
