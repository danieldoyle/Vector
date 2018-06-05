using Payload.Common;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HelloWorld
{
	public class InjectionSite
	{
		[Payload(Name = "Payload.HelloWorld", Description = "Basic example payload to inject", CanLoadMultiple = true)]
		public static void Start()
		{
			Task.Factory.StartNew(() =>
			{
				if (Application.OpenForms.Count > 0)
				{
					var mainForm = Application.OpenForms[0];
					mainForm.Invoke((MethodInvoker)(() =>
					{
						MessageBox.Show("Hello World!");
					}));
				}
			});
		}
	}
}
