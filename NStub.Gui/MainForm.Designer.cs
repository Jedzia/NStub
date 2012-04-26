namespace NStub.Gui
{
    using NStub.Gui.Components;

    partial class MainForm : ILoggable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._assemblyGraphTreeView = new System.Windows.Forms.TreeView();
            this._objectIconsImageList = new System.Windows.Forms.ImageList(this.components);
            this._browseOutputDirectoryButton = new System.Windows.Forms.Button();
            this._outputDirectoryLabel = new System.Windows.Forms.Label();
            this._outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._goButton = new System.Windows.Forms.Button();
            this._browseInputAssemblyButton = new System.Windows.Forms.Button();
            this._inputAssemblyOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._inputAssemblyLabel = new System.Windows.Forms.Label();
            this._outputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this._inputAssemblyTextBox = new System.Windows.Forms.TextBox();
            this.logText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbGenerators = new System.Windows.Forms.ComboBox();
            this.logtimer = new System.Windows.Forms.Timer(this.components);
            this.bnConfigGenerator = new System.Windows.Forms.Button();
            this.bpc = new NStub.Gui.Components.BuildPropertyComponent(this.components);
            this.settings = new NStub.Gui.Components.SettingsHelperComponent(this.components);
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _assemblyGraphTreeView
            // 
            this._assemblyGraphTreeView.CheckBoxes = true;
            this._assemblyGraphTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._assemblyGraphTreeView.ImageIndex = 0;
            this._assemblyGraphTreeView.ImageList = this._objectIconsImageList;
            this._assemblyGraphTreeView.Location = new System.Drawing.Point(0, 0);
            this._assemblyGraphTreeView.Name = "_assemblyGraphTreeView";
            this._assemblyGraphTreeView.SelectedImageIndex = 0;
            this._assemblyGraphTreeView.Size = new System.Drawing.Size(928, 164);
            this._assemblyGraphTreeView.TabIndex = 15;
            this._assemblyGraphTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvAssemblyGraph_AfterCheck);
            this._assemblyGraphTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvAssemblyGraph_BeforeSelect);
            // 
            // _objectIconsImageList
            // 
            this._objectIconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_objectIconsImageList.ImageStream")));
            this._objectIconsImageList.TransparentColor = System.Drawing.Color.Magenta;
            this._objectIconsImageList.Images.SetKeyName(0, "imgAssembly");
            this._objectIconsImageList.Images.SetKeyName(1, "imgModule");
            this._objectIconsImageList.Images.SetKeyName(2, "imgNamespace");
            this._objectIconsImageList.Images.SetKeyName(3, "imgClass");
            this._objectIconsImageList.Images.SetKeyName(4, "imgMethod");
            // 
            // _browseOutputDirectoryButton
            // 
            this._browseOutputDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browseOutputDirectoryButton.Enabled = false;
            this._browseOutputDirectoryButton.Location = new System.Drawing.Point(857, 30);
            this._browseOutputDirectoryButton.Name = "_browseOutputDirectoryButton";
            this._browseOutputDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this._browseOutputDirectoryButton.TabIndex = 14;
            this._browseOutputDirectoryButton.Text = "Browse...";
            this._browseOutputDirectoryButton.UseVisualStyleBackColor = true;
            this._browseOutputDirectoryButton.Click += new System.EventHandler(this.btnBrowseOutputDirectory_Click);
            // 
            // _outputDirectoryLabel
            // 
            this._outputDirectoryLabel.AutoSize = true;
            this._outputDirectoryLabel.Location = new System.Drawing.Point(12, 35);
            this._outputDirectoryLabel.Name = "_outputDirectoryLabel";
            this._outputDirectoryLabel.Size = new System.Drawing.Size(84, 13);
            this._outputDirectoryLabel.TabIndex = 12;
            this._outputDirectoryLabel.Text = "Output Directory";
            // 
            // _goButton
            // 
            this._goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._goButton.Enabled = false;
            this._goButton.Location = new System.Drawing.Point(857, 575);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(75, 23);
            this._goButton.TabIndex = 11;
            this._goButton.Text = "Go";
            this._goButton.UseVisualStyleBackColor = true;
            this._goButton.Click += new System.EventHandler(this.BtnGoClick);
            // 
            // _browseInputAssemblyButton
            // 
            this._browseInputAssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browseInputAssemblyButton.Location = new System.Drawing.Point(857, 4);
            this._browseInputAssemblyButton.Name = "_browseInputAssemblyButton";
            this._browseInputAssemblyButton.Size = new System.Drawing.Size(75, 23);
            this._browseInputAssemblyButton.TabIndex = 8;
            this._browseInputAssemblyButton.Text = "Browse...";
            this._browseInputAssemblyButton.UseVisualStyleBackColor = true;
            this._browseInputAssemblyButton.Click += new System.EventHandler(this.btnBrowseInputAssembly_Click);
            // 
            // _inputAssemblyOpenFileDialog
            // 
            this._inputAssemblyOpenFileDialog.Filter = "Valid Assemblies | *.dll; *.exe";
            this._inputAssemblyOpenFileDialog.Multiselect = true;
            // 
            // _inputAssemblyLabel
            // 
            this._inputAssemblyLabel.AutoSize = true;
            this._inputAssemblyLabel.Location = new System.Drawing.Point(12, 9);
            this._inputAssemblyLabel.Name = "_inputAssemblyLabel";
            this._inputAssemblyLabel.Size = new System.Drawing.Size(78, 13);
            this._inputAssemblyLabel.TabIndex = 10;
            this._inputAssemblyLabel.Text = "Input Assembly";
            // 
            // _outputDirectoryTextBox
            // 
            this._outputDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._outputDirectoryTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::NStub.Gui.Properties.Settings.Default, "CurrentOutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._outputDirectoryTextBox.Location = new System.Drawing.Point(102, 32);
            this._outputDirectoryTextBox.Name = "_outputDirectoryTextBox";
            this._outputDirectoryTextBox.ReadOnly = true;
            this._outputDirectoryTextBox.Size = new System.Drawing.Size(749, 20);
            this._outputDirectoryTextBox.TabIndex = 13;
            this._outputDirectoryTextBox.Text = global::NStub.Gui.Properties.Settings.Default.CurrentOutputDirectory;
            // 
            // _inputAssemblyTextBox
            // 
            this._inputAssemblyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._inputAssemblyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::NStub.Gui.Properties.Settings.Default, "CurrentInputAssembly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._inputAssemblyTextBox.Location = new System.Drawing.Point(102, 6);
            this._inputAssemblyTextBox.Name = "_inputAssemblyTextBox";
            this._inputAssemblyTextBox.ReadOnly = true;
            this._inputAssemblyTextBox.Size = new System.Drawing.Size(749, 20);
            this._inputAssemblyTextBox.TabIndex = 9;
            this._inputAssemblyTextBox.Text = global::NStub.Gui.Properties.Settings.Default.CurrentInputAssembly;
            // 
            // logText
            // 
            this.logText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logText.Location = new System.Drawing.Point(3, 16);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logText.Size = new System.Drawing.Size(922, 302);
            this.logText.TabIndex = 16;
            this.logText.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this._assemblyGraphTreeView);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(4, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(928, 485);
            this.panel1.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.logText);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(928, 321);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Code Generator";
            // 
            // cbGenerators
            // 
            this.cbGenerators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGenerators.FormattingEnabled = true;
            this.cbGenerators.Location = new System.Drawing.Point(102, 57);
            this.cbGenerators.Name = "cbGenerators";
            this.cbGenerators.Size = new System.Drawing.Size(749, 21);
            this.cbGenerators.TabIndex = 18;
            // 
            // logtimer
            // 
            this.logtimer.Interval = 250;
            this.logtimer.Tick += new System.EventHandler(this.logtimer_Tick);
            // 
            // bnConfigGenerator
            // 
            this.bnConfigGenerator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnConfigGenerator.Location = new System.Drawing.Point(857, 55);
            this.bnConfigGenerator.Name = "bnConfigGenerator";
            this.bnConfigGenerator.Size = new System.Drawing.Size(75, 23);
            this.bnConfigGenerator.TabIndex = 14;
            this.bnConfigGenerator.Text = "Config";
            this.bnConfigGenerator.UseVisualStyleBackColor = true;
            this.bnConfigGenerator.Click += new System.EventHandler(this.bnConfigGenerator_Click);
            // 
            // settings
            // 
            this.settings.MainForm = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 612);
            this.Controls.Add(this.cbGenerators);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bnConfigGenerator);
            this.Controls.Add(this._browseOutputDirectoryButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._outputDirectoryLabel);
            this.Controls.Add(this._outputDirectoryTextBox);
            this.Controls.Add(this._goButton);
            this.Controls.Add(this._browseInputAssemblyButton);
            this.Controls.Add(this._inputAssemblyTextBox);
            this.Controls.Add(this._inputAssemblyLabel);
            this.Name = "MainForm";
            this.Text = "NStub";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView _assemblyGraphTreeView;
		private System.Windows.Forms.Button _browseOutputDirectoryButton;
		private System.Windows.Forms.Label _outputDirectoryLabel;
		private System.Windows.Forms.TextBox _outputDirectoryTextBox;
		private System.Windows.Forms.FolderBrowserDialog _outputFolderBrowserDialog;
		private System.Windows.Forms.Button _goButton;
		private System.Windows.Forms.Button _browseInputAssemblyButton;
		private System.Windows.Forms.TextBox _inputAssemblyTextBox;
		private System.Windows.Forms.OpenFileDialog _inputAssemblyOpenFileDialog;
		private System.Windows.Forms.Label _inputAssemblyLabel;
		private System.Windows.Forms.ImageList _objectIconsImageList;
        private System.Windows.Forms.TextBox logText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbGenerators;
        private System.Windows.Forms.Timer logtimer;
        private System.Windows.Forms.Button bnConfigGenerator;
        private BuildPropertyComponent bpc;
        private SettingsHelperComponent settings;

	}
}

