namespace PayloadTest
{
	partial class TestForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnLaunch = new System.Windows.Forms.Button();
			this.btnFakeIt = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnLaunch
			// 
			this.btnLaunch.Location = new System.Drawing.Point(55, 38);
			this.btnLaunch.Name = "btnLaunch";
			this.btnLaunch.Size = new System.Drawing.Size(145, 66);
			this.btnLaunch.TabIndex = 0;
			this.btnLaunch.Text = "Launch";
			this.btnLaunch.UseVisualStyleBackColor = true;
			this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
			// 
			// btnFakeIt
			// 
			this.btnFakeIt.Location = new System.Drawing.Point(55, 139);
			this.btnFakeIt.Name = "btnFakeIt";
			this.btnFakeIt.Size = new System.Drawing.Size(145, 66);
			this.btnFakeIt.TabIndex = 1;
			this.btnFakeIt.Text = "FakeIt";
			this.btnFakeIt.UseVisualStyleBackColor = true;
			this.btnFakeIt.Click += new System.EventHandler(this.btnFakeIt_Click);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(278, 244);
			this.Controls.Add(this.btnFakeIt);
			this.Controls.Add(this.btnLaunch);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnLaunch;
		private System.Windows.Forms.Button btnFakeIt;
	}
}