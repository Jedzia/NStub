// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildPropertyComponent.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Components
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Component which holds the application wide <see cref="BuildDataDictionary"/> BuildProperties.
    /// </summary>
    public partial class BuildPropertyComponent : Component
    {
        private string buildPropertyFilename;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertyComponent"/> class.
        /// </summary>
        public BuildPropertyComponent()
        {
            InitializeComponent();
            SetupInstance();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPropertyComponent"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public BuildPropertyComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            SetupInstance();
        }

        /// <summary>
        /// Gets the build properties.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public BuildDataDictionary BuildProperties { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has skipped loading the parameter file.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has skipped loading the parameter file; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>For further investigation of errors you should supply a logging handler to the
        /// <see cref="Logger"/> property and check the error message produced by this instance.</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool HasSkippedLoading { get; private set; }

        /// <summary>
        /// Gets the member factory.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IMemberBuilderFactory Memberfactory { get; private set; }

        /// <summary>
        /// Gets or sets the logging callback.
        /// </summary>
        /// <value>
        /// The  logging callback.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ILoggable Logger { get; set; }

        /// <summary>
        /// Gets or sets the location of the build property storage file.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("BuildParameters.xml")]
        public string BuildPropertyFilename
        {
            get
            {
                if (string.IsNullOrEmpty(this.buildPropertyFilename))
                {
                    this.buildPropertyFilename = "BuildParameters.xml";
                }

                return this.buildPropertyFilename;
            }

            set
            {
                this.buildPropertyFilename = value;
            }
        }

        /// <summary>
        /// Loads the build property data.
        /// </summary>
        public void LoadBuildPropertyData()
        {
            BuildProperties.AddDataItem("EXTRA", "From Main StartUp", CSharp.ObjectGeneration.Builders.MemberBuilder.EmptyParameters);
            GeneratorConfigLoad(this.BuildProperties);
        }

        /// <summary>
        /// Saves the build property data.
        /// </summary>
        public void SaveBuildPropertyData()
        {
            var xml = Memberfactory.SerializeAllSetupData(BuildProperties);
            var filename = Path.Combine(Application.StartupPath, this.BuildPropertyFilename);
            File.WriteAllText(filename, xml);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise, <c>false</c>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.SaveBuildPropertyData();

                if (components != null)
                { 
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void GeneratorConfigLoad(IBuildDataDictionary properties)
        {
            Log("--------- Loading Property Data (Start) ------------");

/*            var sampleXmlData =
@"<NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>" + Environment.NewLine +
@"  <PropertyBuilderUserParameters>" + Environment.NewLine +
@"    <MethodSuffix>HeuteMalWasNeues</MethodSuffix>" + Environment.NewLine +
@"    <UseDings>true</UseDings>" + Environment.NewLine +
@"    <Moep>0</Moep>" + Environment.NewLine +
@"    <Enabled>false</Enabled>" + Environment.NewLine +
@"  </PropertyBuilderUserParameters>" + Environment.NewLine +
@"</NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>";
*/

            // IBuildSystem sys;
            // sys.
            var filename = Path.Combine(Application.StartupPath, this.BuildPropertyFilename);
            if (File.Exists(filename))
            {
                var xml = File.ReadAllText(filename);
                Memberfactory.DeserializeAllSetupData(xml, properties);
            }
            else
            {
                Log("Build parameter file '" + filename + "' does not exist. Skipped loading.");
                HasSkippedLoading = true;
            }

            // log the parsed data to the logger.
            var xmlparsed = Memberfactory.SerializeAllSetupData(BuildProperties);

            Log("Property data in property store:");
            Log(xmlparsed);
            Log("--------- Loading Property Data (End) ------------");
        }

        private void Log(string text)
        {
            if (Logger != null)
            {
                Logger.Log(text);
            }
        }

        private void SetupInstance()
        {
            BuildProperties = new BuildDataDictionary();
            Memberfactory = MemberBuilderFactory.Default;
        }
    }
}
