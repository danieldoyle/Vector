using System.Reflection;

namespace Vector
{
	public class InjectionPayload
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Class { get; set; }
		public string Method { get; set; }
		public Assembly Assembly { get; set; }
		public bool CanLoadMultiple { get; set; }
	}
}
