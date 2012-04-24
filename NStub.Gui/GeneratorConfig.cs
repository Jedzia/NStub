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
    using NStub.Core;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Application <see cref="BuildDataDictionary"/> data configurator.
    /// </summary>
    public partial class GeneratorConfig : Form
    {
        #region Fields

        private readonly IMemberBuilderFactory memberfactory = MemberBuilderFactory.Default;
        private readonly BuildDataDictionary properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorConfig"/> class.
        /// </summary>
        public GeneratorConfig()
        {
            this.properties = new BuildDataDictionary();
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorConfig"/> class.
        /// </summary>
        /// <param name="buildData">The build data property store.</param>
        public GeneratorConfig(BuildDataDictionary buildData)
        {
            Guard.NotNull(() => buildData, buildData);
            this.properties = buildData;
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
                return this.chklbParameters.SelectedItem as Type;
            }
        }

        protected MultiLookup SelectedMulti
        {
            get
            {
                return this.chklbParameters.SelectedItem as MultiLookup;
            }
        }

        #endregion

        /*private static BuilderParameter TestXmlSeria()
        {
            var pb = new BuilderParameter();

            var pbps = new BuilderParameterPropertyBuilder { MethodSuffix = "OlderDepp", Moep = 42, UseDings = false };
            pb.Items.Add(pbps);

            pbps = new BuilderParameterPropertyBuilder { MethodSuffix = "OtherParameter", UseDings = true };
            pb.Items.Add(pbps);
            return pb;
        }*/
        private void GeneratorConfigLoad(object sender, EventArgs e)
        {
            foreach (var builderType in this.memberfactory.BuilderTypes)
            {
                if (typeof(IMultiBuilder).IsAssignableFrom(builderType))
                {
                    var lviewItem = this.lbBuilderTypes.Items.Add(builderType.Name);
                    //AddMultiParameterToBox(builderType, 
                    lviewItem.Tag = builderType;
                }
                else
                {
                    var para = this.memberfactory.GetParameters(builderType, this.properties);
                    AddParameterToBox(builderType, para);
                }
            }

            var multis = ((MemberBuilderFactory)this.memberfactory).MultiParameters(properties);
            foreach (var multi in multis)
            {
                AddMultiParameterToBox2(multi);

                /*foreach (var item in multi.Lookup)
                {
                    var s = item.Value;
                    var multipara = item.Value as IMultiBuildParameters;
                    AddMultiParameterToBox(multi.BuilderType, multipara);
                }*/
            }

        }

        private void AddParameterToBox(Type builderType, IMemberBuildParameters para)
        {
            var chkItemIndex = this.chklbParameters.Items.Add(builderType);
            this.chklbParameters.SetItemChecked(chkItemIndex, para.Enabled);
            //this.chklbParameters.Items[chkItemIndex]
        }

        private void AddMultiParameterToBox2(MultiLookup multi)
        {
            //var chkItemIndex = this.chklbParameters.Items.Add(para);
            var chkItemIndex = this.chklbParameters.Items.Add(multi);
            this.chklbParameters.SetItemChecked(chkItemIndex, multi.Parameters.Enabled);
        }

        /*private void AddMultiParameterToBox(Type builderType, IMultiBuildParameters para)
        {
            //var chkItemIndex = this.chklbParameters.Items.Add(para);
            var chkItemIndex = this.chklbParameters.Items.Add(builderType);
            this.chklbParameters.SetItemChecked(chkItemIndex, para.Enabled);
        }*/

        // todo bessere benamsung ... blickt ja keine sau mehr durch
        private void LvParametersItemCheck(object sender, ItemCheckEventArgs e)
        {
            var current = e.CurrentValue;
            if (this.SelectedType == null)
            {
                return;
            }

            var newValue = e.CurrentValue != CheckState.Checked;

            if (this.SelectedType != null)
            {
                // das bloed, wieso nicht gleich den parameter speichern? ... oder im multi lookup?
                var item = this.memberfactory.GetParameters(this.SelectedType, this.properties);
                if (item.Enabled != newValue)
                {
                    item.Enabled = newValue;
                    if (this.SelectedType != null)
                    {
                        this.tbConfig.Text = this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
                    }
                }
            }
            else if (this.SelectedMulti != null)
            {
                var item = SelectedMulti.Parameters;
                if (item.Enabled != newValue)
                {
                    item.Enabled = newValue;
                    this.tbConfig.Text = this.memberfactory.SerializeSetupData(
                        this.SelectedMulti.Parameters.Id,
                        this.SelectedMulti.BuilderType,
                        this.properties);
                }
            }
        }

        private void LvParametersSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedType != null)
            {
                this.propGrid.SelectedObject = this.memberfactory.GetParameters(this.SelectedType, this.properties);
                this.tbConfig.Text = this.memberfactory.GetBuilderDescription(this.SelectedType) + Environment.NewLine;
                this.tbConfig.Text += this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
            }
            else if (this.SelectedMulti != null)
            {
                this.propGrid.SelectedObject = this.SelectedMulti.Parameters;
                this.tbConfig.Text = this.memberfactory.GetBuilderDescription(this.SelectedMulti.BuilderType) + Environment.NewLine;
                this.tbConfig.Text += this.memberfactory.SerializeSetupData(
                    this.SelectedMulti.Parameters.Id,
                    this.SelectedMulti.BuilderType,
                    this.properties);
            }
        }

        private void PropGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.SelectedType != null)
            {
                this.tbConfig.Text = this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
            }
            else if (this.SelectedMulti != null)
            {
                this.tbConfig.Text = this.memberfactory.SerializeSetupData(
                    this.SelectedMulti.Parameters.Id,
                    this.SelectedMulti.BuilderType,
                    this.properties);
            }
        }


        private void lbBuilderTypes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.lbBuilderTypes.SelectedItems.Count < 1)
            {
                return;
            }

            var selected = this.lbBuilderTypes.SelectedItems[0];
            var seltype = (Type)selected.Tag;
            var mbf = memberfactory as MemberBuilderFactory;
            var mbpara = mbf.GetMultiParameter(Guid.Empty, seltype, properties);
            if (mbpara != null)
            {
                AddMultiParameterToBox2(new MultiLookup() { BuilderType = seltype, Parameters = mbpara });
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }
    }
}