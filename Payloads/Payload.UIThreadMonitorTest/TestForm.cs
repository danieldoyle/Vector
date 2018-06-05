
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UIThreadMonitor;

namespace PayloadTest
{
	public partial class TestForm : Form
	{
		private InspectorUI inspector;
		public TestForm()
		{
			InitializeComponent();
		}

		private void btnLaunch_Click(object sender, EventArgs e)
		{
			inspector = new InspectorUI();
			inspector.Show();
		}
		
		private void btnFakeIt_Click(object sender, EventArgs e)
		{
			IList<UIDetails> fakeStacks = new List<UIDetails>();
			for (int i=0; i< 10; i++)
			{
				fakeStacks.Add(new UIDetails(i, string.Format("Doyle Rules: {0}", i)));
			}
			inspector.Update(fakeStacks);
			inspector.Update(new List<GCDetails>() { new GCDetails() { Timestamp = DateTime.UtcNow, Message = "DoyleTest" } });
		}
	}
}
