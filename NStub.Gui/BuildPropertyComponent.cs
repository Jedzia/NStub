// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildPropertyComponent.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
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
        /// Gets the memberfactory.
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
            BuildProperties.AddDataItem("EXTRA", "From Main StartUp", NStub.CSharp.ObjectGeneration.Builders.MemberBuilder.EmptyParameters);
            GeneratorConfigLoad(this.BuildProperties);

            var xml = Memberfactory.SerializeAllSetupData(BuildProperties);
            
            Log("Property data loaded:");
            Log(xml);
            Log("---------------------");
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
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            var sampleXmlData =
@"<NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>" + Environment.NewLine +
@"  <PropertyBuilderUserParameters>" + Environment.NewLine +
@"    <MethodSuffix>HeuteMalWasNeues</MethodSuffix>" + Environment.NewLine +
@"    <UseDings>true</UseDings>" + Environment.NewLine +
@"    <Moep>0</Moep>" + Environment.NewLine +
@"    <Enabled>false</Enabled>" + Environment.NewLine +
@"  </PropertyBuilderUserParameters>" + Environment.NewLine +
@"</NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>";

            var filename = Path.Combine(Application.StartupPath, this.BuildPropertyFilename);
            var xml = File.ReadAllText(filename);
            Memberfactory.DeserializeAllSetupData(xml, properties);
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
