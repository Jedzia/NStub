// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="EvePanix">
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
    using System.Windows.Forms;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Application <see cref="BuildDataDictionary"/> data configurator.
    /// </summary>
    public partial class GeneratorConfig : Form
    {
        #region Fields

        private readonly IMemberBuilderFactory memberfactory = MemberBuilderFactory.Default;
        private readonly BuildDataDictionary properties = new BuildDataDictionary();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorConfig"/> class.
        /// </summary>
        public GeneratorConfig()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the selected <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <value>
        /// The type of the selected <see cref="IMemberBuilder"/>.
        /// </value>
        protected Type SelectedType
        {
            get
            {
                return this.lvParameters.SelectedItem as Type;
            }
        }

        #endregion

        private static PropertyBuilderUser TestXmlSeria()
        {
            var pb = new PropertyBuilderUser();

            var pbps = new PropertyBuilderUserParameters { MethodSuffix = "OlderDepp", Moep = 42, UseDings = false };
            pb.Items.Add(pbps);

            pbps = new PropertyBuilderUserParameters { MethodSuffix = "OtherParameter", UseDings = true };
            pb.Items.Add(pbps);
            return pb;
        }

        private void GeneratorConfigLoad(object sender, EventArgs e)
        {
            foreach (var builderName in this.memberfactory.BuilderTypes)
            {
                var lvItem = this.lvParameters.Items.Add(builderName);
            }

            // {[NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder, NStub.CSharp.ObjectGeneration.BuildHandler]}
            var mf = this.memberfactory as MemberBuilderFactory;
            var sampleXmlData =
                @"<NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>" + Environment.NewLine +
                @"  <PropertyBuilderUserParameters>" + Environment.NewLine +
                @"    <MethodSuffix>HeuteMalWasNeues</MethodSuffix>" + Environment.NewLine +
                @"    <UseDings>false</UseDings>" + Environment.NewLine +
                @"    <Moep>0</Moep>" + Environment.NewLine +
                @"    <Enabled>false</Enabled>" + Environment.NewLine +
                @"  </PropertyBuilderUserParameters>" + Environment.NewLine +
                @"</NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>";

            /*var xxxx =*/
            mf.SetParameters(sampleXmlData, this.properties);
        }

        private void LvParametersSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedType == null)
            {
                return;
            }

            var mf = this.memberfactory as MemberBuilderFactory;

            // tbConfig.Text = mf.GetSampleSetupData(SelectedType);
            // tbConfig.Text = mf.GetParameters(SelectedType, properties).Serialize();
            // tbConfig.Text = mf.SerializeSetupData(SelectedType, properties);
            this.tbConfig.Text = mf.SerializeParametersForBuilderType(this.properties);
            var sample = this.memberfactory.GetParameters(this.SelectedType, this.properties);
            this.propGrid.SelectedObject = sample;
            return;

            /*var xmlDoc = new XmlDocument();
            var ele = xmlDoc.CreateElement(SelectedType.FullName);
            var ele2 = xmlDoc.CreateElement("Sub");
            xmlDoc.AppendChild(ele);
            ele.AppendChild(ele2);

            var sample = memberfactory.GetParameters(SelectedType, properties);
            //tbConfig.Text = sample.SampleXml;
            var innerDoc = new XmlDocument();
            innerDoc.LoadXml(sample.Serialize());
            var sssss = innerDoc[SelectedType.Name];
            //ele2.InnerXml = ;

            tbConfig.Text = xmlDoc.OuterXml;
            propGrid.SelectedObject = sample;*/
        }
    }
}