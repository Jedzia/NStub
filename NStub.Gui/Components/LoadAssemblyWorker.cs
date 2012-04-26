// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadAssemblyWorker.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Components
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using NStub.Core;
    using NStub.CSharp;
    using NStub.CSharp.ObjectGeneration;
    using NStub.Gui.Util;

    /// <summary>
    /// Automated assembly loader background worker. 
    /// </summary>
    internal class LoadAssemblyWorker : BackgroundWorker
    {
        #region Fields

        private readonly IBuildDataDictionary buildData;
        private readonly IBuildSystem buildSystem;
        private readonly MainForm mainform;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadAssemblyWorker"/> class.
        /// </summary>
        /// <param name="buildSystem">The build system.</param>
        /// <param name="buildData">The build properties data storage.</param>
        /// <param name="mainform">The application main window.</param>
        public LoadAssemblyWorker(IBuildSystem buildSystem, IBuildDataDictionary buildData, MainForm mainform)
        {
            Guard.NotNull(() => buildSystem, buildSystem);
            this.buildSystem = buildSystem;
            Guard.NotNull(() => buildData, buildData);
            this.buildData = buildData;
            Guard.NotNull(() => mainform, mainform);
            this.mainform = mainform;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the browse input assembly button.
        /// </summary>
        /// <value>
        /// The browse input assembly button.
        /// </value>
        public Button BrowseInputAssemblyButton { get; set; }

        /// <summary>
        /// Gets or sets the browse output directory button.
        /// </summary>
        /// <value>
        /// The browse output directory button.
        /// </value>
        public Button BrowseOutputDirectoryButton { get; set; }

        /// <summary>
        /// Gets or sets the go button.
        /// </summary>
        /// <value>
        /// The go button.
        /// </value>
        public Button GoButton { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger of this instance.
        /// </value>
        public Action<string> Logger { get; set; }

        #endregion

        /// <summary>
        /// Runs the worker asynchronous.
        /// </summary>
        /// <param name="runnerData">The runner data.</param>
        public void RunWorkerAsync(
            GeneratorRunnerData runnerData)
        {
            // Dumper();
            // GenerateTests();
            this.BeforeGenerateTests();

            var bg = new BackgroundWorker();
            bg.DoWork += this.WorkerDoWork;
            bg.RunWorkerCompleted += this.BgRunWorkerCompleted;

            // Type generatorType = (Type)cbGeneratorsSelectedItem;
            // IList<TreeNode> mainNodes = assemblyGraph.Nodes.Cast<TreeNode>().ToList();
            // var runnerData = new GeneratorRunnerData(outputFolder, generatorType, inputAssemblyPath, mainNodes.MapToNodes(), referencedAssemblies);

            // var parameters = new object[] { outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies };
            bg.RunWorkerAsync(runnerData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            this.WorkerDoWork(this, e);
        }

        private void AfterGenerateTests()
        {
            // this.InvokeIfRequired(() =>
            // {
            if (this.BrowseInputAssemblyButton != null)
            {
                this.BrowseInputAssemblyButton.Enabled = true;
            }

            if (this.BrowseOutputDirectoryButton != null)
            {
                this.BrowseOutputDirectoryButton.Enabled = true;
            }

            if (this.GoButton != null)
            {
                this.GoButton.Enabled = true;
            }

            Cursor.Current = Cursors.Arrow;

            // });
        }

        private void BeforeGenerateTests()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.BrowseInputAssemblyButton != null)
            {
                this.BrowseInputAssemblyButton.Enabled = false;
            }

            if (this.BrowseOutputDirectoryButton != null)
            {
                this.BrowseOutputDirectoryButton.Enabled = false;
            }

            if (this.GoButton != null)
            {
                this.GoButton.Enabled = false;
            }
        }

        private void BgRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.mainform.InvokeIfRequired(this.AfterGenerateTests);
        }

        private void Log(string text)
        {
            if (this.Logger != null)
            {
                this.Logger(text);
            }
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var parameters = (GeneratorRunnerData)e.Argument;
                var prjName = Path.GetFileNameWithoutExtension(parameters.InputAssemblyPath) + ".Tests";
                var prj = new CSharpProjectGenerator(this.buildSystem, prjName, parameters.OutputFolder);

                // var builderFactory = NStub.CSharp.ObjectGeneration.TestBuilderFactory.Default;
                var testProjectBuilder = new CSharpTestProjectBuilder(
                    this.buildSystem, 
                    this.buildData, 
                    prj, 
                    (pbuildSystem, pbuildData, configuration, codeNamespace) =>
                        {
                            // var testBuilders = new TestBuilderFactory();
                            var codeGenerator =
                                (ICodeGenerator)Activator.CreateInstance(
                                    parameters.GeneratorType,
                                    new object[] { pbuildSystem, codeNamespace, null, configuration });

                            // codeNamespace.Dump(3);
                            if (codeGenerator is BaseCSharpCodeGenerator)
                            {
                                ((BaseCSharpCodeGenerator)codeGenerator).BuildProperties =
                                    pbuildData as BuildDataDictionary;
                            }

                            return codeGenerator;
                        }, 
                    this.Log);

                testProjectBuilder.CustomGeneratorParameters = this.CustomGeneratorParameters;
                testProjectBuilder.GenerateTests(parameters);
            }
            catch (Exception ex)
            {
                this.Log(ex.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the user provided code generator parameters.
        /// </summary>
        public ICodeGeneratorSetup CustomGeneratorParameters
        {
            get;
            set;
        }

    }
}