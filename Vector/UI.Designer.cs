namespace Vector
{
	partial class UI
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
			this.btnInject = new System.Windows.Forms.Button();
			this.processGrid = new System.Windows.Forms.DataGridView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lblProcess = new System.Windows.Forms.Label();
			this.lblPayload = new System.Windows.Forms.Label();
			this.payloadGrid = new System.Windows.Forms.DataGridView();
			this.btnRefreshProcess = new System.Windows.Forms.Button();
			this.btnRefreshPayload = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			((System.ComponentModel.ISupportInitialize)(this.processGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.payloadGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// btnInject
			// 
			this.btnInject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInject.Location = new System.Drawing.Point(3, 263);
			this.btnInject.Name = "btnInject";
			this.btnInject.Size = new System.Drawing.Size(450, 42);
			this.btnInject.TabIndex = 5;
			this.btnInject.Text = "Inject";
			this.btnInject.UseVisualStyleBackColor = true;
			this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
			// 
			// processGrid
			// 
			this.processGrid.AllowUserToAddRows = false;
			this.processGrid.AllowUserToDeleteRows = false;
			this.processGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.processGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.processGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.processGrid.Location = new System.Drawing.Point(3, 54);
			this.processGrid.MultiSelect = false;
			this.processGrid.Name = "processGrid";
			this.processGrid.ReadOnly = true;
			this.processGrid.RowTemplate.Height = 28;
			this.processGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.processGrid.Size = new System.Drawing.Size(450, 234);
			this.processGrid.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.btnRefreshProcess);
			this.splitContainer1.Panel1.Controls.Add(this.lblProcess);
			this.splitContainer1.Panel1.Controls.Add(this.processGrid);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnRefreshPayload);
			this.splitContainer1.Panel2.Controls.Add(this.lblPayload);
			this.splitContainer1.Panel2.Controls.Add(this.payloadGrid);
			this.splitContainer1.Panel2.Controls.Add(this.btnInject);
			this.splitContainer1.Size = new System.Drawing.Size(456, 612);
			this.splitContainer1.SplitterDistance = 291;
			this.splitContainer1.TabIndex = 2;
			// 
			// lblProcess
			// 
			this.lblProcess.AutoSize = true;
			this.lblProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProcess.Location = new System.Drawing.Point(3, 10);
			this.lblProcess.Name = "lblProcess";
			this.lblProcess.Size = new System.Drawing.Size(198, 25);
			this.lblProcess.TabIndex = 2;
			this.lblProcess.Text = "Choose your process";
			// 
			// lblPayload
			// 
			this.lblPayload.AutoSize = true;
			this.lblPayload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPayload.Location = new System.Drawing.Point(3, 10);
			this.lblPayload.Name = "lblPayload";
			this.lblPayload.Size = new System.Drawing.Size(198, 25);
			this.lblPayload.TabIndex = 3;
			this.lblPayload.Text = "Choose your payload";
			// 
			// payloadGrid
			// 
			this.payloadGrid.AllowUserToAddRows = false;
			this.payloadGrid.AllowUserToDeleteRows = false;
			this.payloadGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.payloadGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.payloadGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.payloadGrid.Location = new System.Drawing.Point(3, 53);
			this.payloadGrid.Name = "payloadGrid";
			this.payloadGrid.RowTemplate.Height = 28;
			this.payloadGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.payloadGrid.Size = new System.Drawing.Size(450, 204);
			this.payloadGrid.TabIndex = 3;
			// 
			// btnRefreshProcess
			// 
			this.btnRefreshProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefreshProcess.ImageIndex = 0;
			this.btnRefreshProcess.ImageList = this.imageList1;
			this.btnRefreshProcess.Location = new System.Drawing.Point(405, 3);
			this.btnRefreshProcess.Name = "btnRefreshProcess";
			this.btnRefreshProcess.Size = new System.Drawing.Size(48, 48);
			this.btnRefreshProcess.TabIndex = 0;
			this.btnRefreshProcess.UseVisualStyleBackColor = true;
			this.btnRefreshProcess.Click += new System.EventHandler(this.btnRefreshProcess_Click);
			// 
			// btnRefreshPayload
			// 
			this.btnRefreshPayload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefreshPayload.ImageIndex = 0;
			this.btnRefreshPayload.ImageList = this.imageList1;
			this.btnRefreshPayload.Location = new System.Drawing.Point(405, 3);
			this.btnRefreshPayload.Name = "btnRefreshPayload";
			this.btnRefreshPayload.Size = new System.Drawing.Size(48, 48);
			this.btnRefreshPayload.TabIndex = 2;
			this.btnRefreshPayload.UseVisualStyleBackColor = true;
			this.btnRefreshPayload.Click += new System.EventHandler(this.btnRefreshPayload_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "refresh.png");
			// 
			// UI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(456, 612);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(478, 668);
			this.Name = "UI";
			this.Text = "Vector";
			((System.ComponentModel.ISupportInitialize)(this.processGrid)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.payloadGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnInject;
		private System.Windows.Forms.DataGridView processGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView payloadGrid;
		private System.Windows.Forms.Label lblProcess;
		private System.Windows.Forms.Label lblPayload;
		private System.Windows.Forms.Button btnRefreshProcess;
		private System.Windows.Forms.Button btnRefreshPayload;
		private System.Windows.Forms.ImageList imageList1;
	}
}

