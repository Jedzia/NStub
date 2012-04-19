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
    using NStub.Gui.Properties;
    using System.Text;
    using System.ComponentModel;
    using NStub.Gui.Util;

    /// <summary>e
    /// This is the main UI form for the NStub application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Member Variables (Private)

        private readonly IList<AssemblyName> _referencedAssemblies =
            new List<AssemblyName>();

        #endregion Member Variables (Private)

        #region Constructor (Public)

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.cbGenerators.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", global::NStub.Gui.Properties.Settings.Default, "SelectedGenerator", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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

        private static readonly IBuildSystem sbs = new StandardBuildSystem();
        //private TestBuilder agb;

        private void btnGo_Click(object sender, EventArgs e)
        {
            Dumper();
            //GenerateTests();

            BeforeGenerateTests();

            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;

            string outputFolder = this._outputDirectoryTextBox.Text;
            Type generatorType = (Type)cbGenerators.SelectedItem;
            string inputAssemblyPath = this._inputAssemblyTextBox.Text;
            IList<TreeNode> mainNodes = this._assemblyGraphTreeView.Nodes.Cast<TreeNode>().ToList();
            IList<AssemblyName> referencedAssemblies = this._referencedAssemblies;
            var data = new GeneratorRunnerData(outputFolder, generatorType, inputAssemblyPath, mainNodes.MapToNodes(), referencedAssemblies);

            //var parameters = new object[] { outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies };
            bg.RunWorkerAsync(data);
        }

        private void BeforeGenerateTests()
        {
            Cursor.Current = Cursors.WaitCursor;
            this._browseInputAssemblyButton.Enabled = false;
            this._browseOutputDirectoryButton.Enabled = false;
        }

        private void AfterGenerateTests()
        {
            //this.InvokeIfRequired(() =>
            //{
            this._browseInputAssemblyButton.Enabled = true;
            this._browseOutputDirectoryButton.Enabled = true;
            Cursor.Current = Cursors.Arrow;
            //});
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.InvokeIfRequired(() => AfterGenerateTests());
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            var agb = new TestBuilder(sbs, this.Log);
            agb.GenerateTests((GeneratorRunnerData)e.Argument);
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
        private void Log(string p)
        {
            p = p.TrimEnd('\r', '\n');
            //this.logText.AppendText(DateTime.Now + ": " + p + Environment.NewLine);
            var ct = DateTime.Now;
            logsb.Append(ct + "." + ct.Millisecond + ": " + p + Environment.NewLine);
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
            this._assemblyGraphTreeView.Nodes.Clear();

            for (int theAssembly = 0; theAssembly < this._inputAssemblyOpenFileDialog.FileNames.Length; theAssembly++)
            {
                // Load our input assembly and create its node in the tree
                Assembly inputAssembly =
                    Assembly.LoadFile(this._inputAssemblyOpenFileDialog.FileNames[theAssembly]);
                TreeNode assemblyTreeNode =
                    this.CreateTreeNode(
                        this._inputAssemblyOpenFileDialog.FileNames[theAssembly],
                        "imgAssembly");
                this._assemblyGraphTreeView.Nodes.Add(assemblyTreeNode);

                // Add our referenced assemblies to the project generator so we
                // can reference them later
                foreach (AssemblyName assemblyName in inputAssembly.GetReferencedAssemblies())
                {
                    this._referencedAssemblies.Add(assemblyName);
                }

                // Retrieve the modules from the assembly.  Most assemblies only have one
                // module, but it is possible for assemblies to possess multiple modules
                Module[] modules = inputAssembly.GetModules(false);

                // Add the namespaces in the DLL
                for (int theModule = 0; theModule < modules.Length; theModule++)
                {
                    // Add a node to the tree to represent the module
                    TreeNode moduleTreeNode =
                        this.CreateTreeNode(modules[theModule].Name, "imgModule");
                    this._assemblyGraphTreeView.Nodes[theAssembly].Nodes.Add(moduleTreeNode);
                    Type[] containedTypes = modules[theModule].GetTypes();

                    // Add the classes in each type
                    for (int theClass = 0; theClass < containedTypes.Length; theClass++)
                    {
                        // Add a node to the tree to represent the class
                        var classType = containedTypes[theClass];
                        var classNode = this.CreateTreeNode(classType.FullName, "imgClass");
                        classNode.Tag = classType;
                        this._assemblyGraphTreeView.Nodes[theAssembly].Nodes[theModule].Nodes.Add(
                            classNode);

                        // Create a test method for each method in this type
                        MethodInfo[] methods = containedTypes[theClass].GetMethods();
                        for (int theMethod = 0; theMethod < methods.Length; theMethod++)
                        {
                            this._assemblyGraphTreeView.Nodes[theAssembly].Nodes[theModule].Nodes[theClass].Nodes.Add(
                                this.CreateTreeNode(methods[theMethod].Name, "imgMethod"));

                            // Store the method's MethodInfo object in this node's tag
                            // so that we may retrieve it later
                            this._assemblyGraphTreeView.Nodes[theAssembly].Nodes[theModule].Nodes[theClass].Nodes[
                                theMethod].Tag =
                                methods[theMethod];
                        }
                    }

                    moduleTreeNode.Expand();
                }

                assemblyTreeNode.Expand();
            }
        }

        #endregion Helper Methods (Private)

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Settings.Default.Reload();
            if (!string.IsNullOrEmpty(this._inputAssemblyTextBox.Text))
            {
                this._browseOutputDirectoryButton.Enabled = true;
                this._inputAssemblyOpenFileDialog.FileName = this._inputAssemblyTextBox.Text;

                // Cursor swapCursor = Cursor.Current;
                // Cursor.Current = Cursors.WaitCursor;
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

            // ad.AssemblyResolve += new ResolveEventHandler(ad_AssemblyResolve);
            // ad.TypeResolve += new ResolveEventHandler(ad_TypeResolve);

            this.cbGenerators.Items.Add(typeof(CSharpCodeGenerator));
            this.cbGenerators.Items.Add(typeof(CSharpMbUnitCodeGenerator));
            this.cbGenerators.Items.Add(typeof(CSharpMbUnitRhinoMocksCodeGenerator));
            // this.cbGenerators.SelectedIndex = 2;
            this.cbGenerators.SelectedIndex = global::NStub.Gui.Properties.Settings.Default.SelectedGenerator;
            ad.AssemblyResolve += this.ad_AssemblyResolve;
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

    }
}