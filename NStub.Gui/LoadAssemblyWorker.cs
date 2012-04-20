using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NStub.Core;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using NStub.Gui.Util;
using NStub.CSharp;

namespace NStub.Gui
{
    internal class LoadAssemblyWorker : BackgroundWorker
    {
        private readonly MainForm mainform;
        private readonly IBuildSystem buildSystem;
        public Action<string> Logger { get; set; }
        public Button _browseInputAssemblyButton { get; set; }
        public Button _browseOutputDirectoryButton { get; set; }
        public Button _goButton { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadAssemblyWorker"/> class.
        /// </summary>
        public LoadAssemblyWorker(IBuildSystem buildSystem, MainForm mainform)
        {
            Guard.NotNull(() => buildSystem, buildSystem);
            this.buildSystem = buildSystem;
            Guard.NotNull(() => mainform, mainform);
            this.mainform = mainform;
        }

        public void RunWorkerAsync(
            GeneratorRunnerData runnerData)
        {

            //Dumper();
            //GenerateTests();

            BeforeGenerateTests();

            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;

            //Type generatorType = (Type)cbGeneratorsSelectedItem;
            //IList<TreeNode> mainNodes = assemblyGraph.Nodes.Cast<TreeNode>().ToList();
            //var runnerData = new GeneratorRunnerData(outputFolder, generatorType, inputAssemblyPath, mainNodes.MapToNodes(), referencedAssemblies);

            //var parameters = new object[] { outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies };
            bg.RunWorkerAsync(runnerData);
        }

        private void BeforeGenerateTests()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (_browseInputAssemblyButton != null)
                _browseInputAssemblyButton.Enabled = false;
            if (_browseOutputDirectoryButton != null)
                _browseOutputDirectoryButton.Enabled = false;
            if (_goButton != null)
                _goButton.Enabled = false;
        }

        private void AfterGenerateTests()
        {
             
            //this.InvokeIfRequired(() =>
            //{
            if (_browseInputAssemblyButton != null)
                _browseInputAssemblyButton.Enabled = true;
            if (_browseOutputDirectoryButton != null)
                _browseOutputDirectoryButton.Enabled = true;
            if (_goButton != null)
                _goButton.Enabled = true;
            Cursor.Current = Cursors.Arrow;
            //});
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainform.InvokeIfRequired(() => AfterGenerateTests());
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            bg_DoWork(this, e);
        
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var parameters = (GeneratorRunnerData)e.Argument;
                var prjName = Path.GetFileNameWithoutExtension(parameters.InputAssemblyPath) + ".Tests";
                var prj = new CSharpProjectGenerator(buildSystem, prjName, parameters.OutputFolder);
                //var builderFactory = NStub.CSharp.ObjectGeneration.TestBuilderFactory.Default;
                var testProjectBuilder = new TestProjectBuilder(buildSystem, prj, (pbuildSystem, configuration, codeNamespace) =>
                {
                    //var testBuilders = new TestBuilderFactory();
                    var codeGenerator = (ICodeGenerator)Activator.CreateInstance(parameters.GeneratorType, new object[]
                           {
                             pbuildSystem, codeNamespace, null, configuration
                       });
                    //codeNamespace.Dump(3);
                    return codeGenerator;

                }, this.Log);
                testProjectBuilder.GenerateTests(parameters);

            }
            catch (Exception ex)
            {
                this.Log(ex.Message);
            }
        }
        
        private void Log(string text)
        {
            if (Logger != null)
            {
                Logger(text);
            }
        }
    }
}
