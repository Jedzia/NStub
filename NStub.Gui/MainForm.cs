// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui
{
    using System;
    using System.Linq;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using NStub.Core;
    using NStub.CSharp;
    using NStub.CSharp.MbUnit;
    using NStub.CSharp.MbUnitRhinoMocks;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.Builders;
    using NStub.Gui.Components;
    using NStub.Gui.Properties;
    using System.Text;
    using System.ComponentModel;
    using NStub.Gui.Util;
    using NStub.Core.Util.Dumper;

    /// <summary>e
    /// This is the main UI form for the NStub application.
    /// </summary>
    public partial class MainForm : Form, ILoggable
    {

        private readonly IList<AssemblyName> _referencedAssemblies =
            new List<AssemblyName>();
        private readonly LoadAssemblyWorker bg;
        //private readonly IMemberBuilderFactory memberfactory = MemberBuilderFactory.Default;
        private static readonly IBuildSystem sbs = new StandardBuildSystem();
        //private TestBuilder agb;
        //private readonly BuildDataDictionary buildData;


        #region Constructor (Public)

        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.settings.Settings = Settings.Default;
            this.cbGenerators.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", global::NStub.Gui.Properties.Settings.Default, "SelectedGenerator", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bpc.Logger = this;
            this.settings.Logger = this;

            //buildData = new BuildDataDictionary();
            bpc.BuildProperties.AddDataItem("FromMainForm", new EmptyBuildParameters());

            bg = new LoadAssemblyWorker(sbs, bpc.BuildProperties, this)
            {
                BrowseInputAssemblyButton = this._browseInputAssemblyButton,
                BrowseOutputDirectoryButton = this._browseOutputDirectoryButton,
                GoButton = this._goButton,
                Logger = Log,
            };
        }

        #endregion Constructor (Public)

        #region Event Handlers (Private)

        /// <summary>
        /// Handles the Click event of the btnBrowseInputAssembly control.
        /// Allows the user to select the assembly they wish to generate the test cases
        /// for.  Calls the <see cref="LoadAssembly"/> method 
        /// to query the assembly for its contained types.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the 
        /// event data.</param>
        private void btnBrowseInputAssembly_Click(object sender, EventArgs e)
        {
            if (this._inputAssemblyOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                this._inputAssemblyTextBox.Text = this._inputAssemblyOpenFileDialog.FileName;
                this._browseOutputDirectoryButton.Enabled = true;

                Cursor swapCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                this.LoadAssembly();

                Cursor.Current = swapCursor;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnBrowseOutputDirectory control.
        ///	Allows the user to select the output directory for the generated sources
        /// files.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the 
        /// event data.</param>
        private void btnBrowseOutputDirectory_Click(object sender, EventArgs e)
        {
            if (this._outputFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this._outputDirectoryTextBox.Text = this._outputFolderBrowserDialog.SelectedPath;
                this._goButton.Enabled = true;
            }
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            Dumper();

            /*var bg = new LoadAssemblyWorker(sbs, this.buildData, this)
            {
               BrowseInputAssemblyButton = this._browseInputAssemblyButton,
               BrowseOutputDirectoryButton = this._browseOutputDirectoryButton,
               GoButton = this._goButton,
                Logger = Log,
            };*/

            string outputFolder = this._outputDirectoryTextBox.Text;
            Type generatorType = (Type)cbGenerators.SelectedItem;
            string inputAssemblyPath = this._inputAssemblyTextBox.Text;
            IList<TreeNode> mainNodes = this._assemblyGraphTreeView.Nodes.Cast<TreeNode>().ToList();
            IList<AssemblyName> referencedAssemblies = this._referencedAssemblies;
            var data = new GeneratorRunnerData(outputFolder, generatorType, inputAssemblyPath, mainNodes.MapToNodes(), referencedAssemblies);

            bg.RunWorkerAsync(data);
        }

        private void Dumper()
        {
            Log("------------------------------------------------");
            Log("Running Dumper");
            new MyObjectDumper().Test();
            Log("End Of Running Dumper");
            Log("------------------------------------------------");
        }

        void Default_TextChanged(object sender, TextWrittenEventArgs e)
        {
            Log(e.Text);
        }

        StringBuilder logsb = new StringBuilder();

        /// <summary>
        /// Logs the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Log(string text)
        {
            text = text.TrimEnd('\r', '\n');
            //this.logText.AppendText(DateTime.Now + ": " + p + Environment.NewLine);
            var ct = DateTime.Now;
            //var msg = string.Format("{0}.{1}: {2}{3}", ct, ct.Millisecond, p, Environment.NewLine);
            //logsb.Append(ct + "." + ct.Millisecond + ": " + p + Environment.NewLine);
            logsb.Append(ct);
            logsb.Append(".");
            logsb.AppendFormat("{0:000}", ct.Millisecond);
            logsb.Append(": ");
            logsb.Append(text);
            logsb.Append(Environment.NewLine);
        }

        private void logtimer_Tick(object sender, EventArgs e)
        {
            this.Invoke((Action)delegate()
            {
                this.logText.AppendText(logsb.ToString());
                logsb.Length = 0;
            });
        }

        /// <summary>
        /// Handles the AfterCheck event of the tvAssemblyGraph control.
        /// Checks or unchecks all children of 
        /// <see cref="System.Windows.Forms.TreeViewEventArgs.Node">e.Node</see>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.TreeViewEventArgs"/> 
        /// instance containing the event data.</param>
        private void tvAssemblyGraph_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Apply this choice to the Node's children
            this.CheckChildren(e.Node);
        }

        /// <summary>
        /// Handles the BeforeSelect event of the tvAssemblyGraph control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs"/> 
        /// instance containing the event data.</param>
        private void tvAssemblyGraph_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // tvAssemblyGraph.SelectedImageIndex = e.Node.ImageIndex;
        }

        #endregion Event Handlers (Private)

        #region Helper Methods (Private)

        /// <summary>
        /// Applies the check state of the given 
        /// <see cref="System.Windows.Forms.TreeNode">TreeNode</see> to all of its 
        /// children.
        /// </summary>
        /// <param name="treeNode">The TreeNode which contains the check state to apply.</param>
        private void CheckChildren(TreeNode treeNode)
        {
            if (treeNode.Nodes.Count != 0)
            {
                this.CheckChildren(treeNode.Nodes[0]);
                for (int i = 0; i < treeNode.Nodes.Count; i++)
                {
                    treeNode.Nodes[i].Checked = treeNode.Checked;
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="System.Windows.Forms.TreeNode">TreeNode</see> with the 
        /// given text and image key.
        /// </summary>
        /// <param name="text">The text of the TreeNode.</param>
        /// <param name="imageKey">The key corresponding to the TreeNode's image.</param>
        /// <returns></returns>
        private TreeNode CreateTreeNode(string text, string imageKey)
        {
            TreeNode treeNode = new TreeNode(text);
            treeNode.Checked = true;
            treeNode.ImageIndex = this._objectIconsImageList.Images.IndexOfKey(imageKey);

            return treeNode;
        }

        /// <summary>
        /// Reflects through the currently selected assembly and reflects the type tree
        /// in tvAssemblyGraph.
        /// </summary>
        private void LoadAssembly()
        {
            var asf = new AssemblyFetcher(this._inputAssemblyOpenFileDialog.FileName, this._inputAssemblyOpenFileDialog.FileNames);
            var resss = asf.LoadAssembly();
            this._assemblyGraphTreeView.Nodes.Clear();
            this._assemblyGraphTreeView.Nodes.Add(resss.Nodes[0].MapToTree());
        }

        #endregion Helper Methods (Private)

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Settings.Default.Save();
            // bpc.SaveBuildPropertyData();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // Settings.Default.Reload();
            if (!string.IsNullOrEmpty(this._inputAssemblyTextBox.Text))
            {
                this._browseOutputDirectoryButton.Enabled = true;
                this._inputAssemblyOpenFileDialog.FileName = this._inputAssemblyTextBox.Text;

                try
                {
                    this.LoadAssembly();
                    
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message);
                }
                finally
                {
                    if (!string.IsNullOrEmpty(this._outputDirectoryTextBox.Text))
                    {
                        this._goButton.Enabled = true;
                    }

                    Server.Default.TextChanged += new EventHandler<TextWrittenEventArgs>(Default_TextChanged);
                    this.logtimer.Enabled = true;

                }
            }

            AppDomain ad = AppDomain.CurrentDomain;

            this.cbGenerators.Items.Add(typeof(CSharpCodeGenerator));
            this.cbGenerators.Items.Add(typeof(CSharpMbUnitCodeGenerator));
            this.cbGenerators.Items.Add(typeof(CSharpMbUnitRhinoMocksCodeGenerator));
            
            this.cbGenerators.SelectedIndex = global::NStub.Gui.Properties.Settings.Default.SelectedGenerator;
            ad.AssemblyResolve += this.ad_AssemblyResolve;

            bpc.LoadBuildPropertyData();

        }

        private Assembly ad_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var finfo = new FileInfo(this._inputAssemblyOpenFileDialog.FileName);

            var assName = args.Name.Split(new[] { ',' })[0] + ".dll";
            var path = Path.Combine(finfo.Directory.FullName, assName);

            // int depp = 6;
            var ass = Assembly.LoadFile(path);
            return ass;
        }

        private Assembly ad_TypeResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }

        private void bnConfigGenerator_Click(object sender, EventArgs e)
        {
            var wnd = new GeneratorConfig(bpc.BuildProperties);
            var result = wnd.ShowDialog();
        }

    }
}