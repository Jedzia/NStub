using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NStub.CSharp.ObjectGeneration.Builders;
using NStub.CSharp.ObjectGeneration;
using System.Xml;

namespace NStub.Gui
{
    public partial class GeneratorConfig : Form
    {
        public GeneratorConfig()
        {
            InitializeComponent();
        }

        private static PropertyBuilderUser TestXmlSeria()
        {
            var pb = new PropertyBuilderUser();

            var pbps = new PropertyBuilderUserParameters();
            pbps.MethodSuffix = "OlderDepp";
            pbps.Moep = 42;
            pbps.UseDings = false;
            pb.Items.Add(pbps);

            pbps = new PropertyBuilderUserParameters();
            pbps.MethodSuffix = "OtherParameter";
            pbps.UseDings = true;
            pb.Items.Add(pbps);
            return pb;
        }

        private void GeneratorConfig_Load(object sender, EventArgs e)
        {
            foreach (var builderName in memberfactory.BuilderTypes)
            {
                var lvItem = this.lvParameters.Items.Add(builderName);
            }
            // {[NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder, NStub.CSharp.ObjectGeneration.BuildHandler]}
            var mf = memberfactory as MemberBuilderFactory;
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
                            mf.SetParameters(sampleXmlData, properties);
        }

        protected Type SelectedType
        {
            get
            {
                return lvParameters.SelectedItem as Type;
            }
        }
        
        private readonly IMemberBuilderFactory memberfactory = MemberBuilderFactory.Default;
        private readonly BuildDataCollection properties = new BuildDataCollection();

        private void lvParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedType == null)
            {
                return;
            }

            var mf = memberfactory as MemberBuilderFactory;

            //tbConfig.Text = mf.GetSampleSetupData(SelectedType);
            //tbConfig.Text = mf.GetParameters(SelectedType, properties).Serialize();
            //tbConfig.Text = mf.SerializeSetupData(SelectedType, properties);
            tbConfig.Text = mf.SerializeParametersForBuilderType(properties);
            var sample = memberfactory.GetParameters(SelectedType, properties);
            propGrid.SelectedObject = sample;
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
