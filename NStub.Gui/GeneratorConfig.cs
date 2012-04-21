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
                return this.lvParameters.SelectedItem as Type;
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
            foreach(var builderType in this.memberfactory.BuilderTypes)
            {
                var para = this.memberfactory.GetParameters(builderType, this.properties);
                var lvItemIndex = this.lvParameters.Items.Add(builderType);
                this.lvParameters.SetItemChecked(lvItemIndex, para.Enabled);
            }
        }

        private void LvParametersItemCheck(object sender, ItemCheckEventArgs e)
        {
            var current = e.CurrentValue;
            if (this.SelectedType == null)
            {
                return;
            }

            var item = this.memberfactory.GetParameters(this.SelectedType, this.properties);
            var newValue = e.CurrentValue != CheckState.Checked;
            if (item.Enabled != newValue)
            {
                item.Enabled = newValue;
                this.SetXmlConfigTextFromType(this.SelectedType);
            }
        }

        private void LvParametersSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedType == null)
            {
                return;
            }

            this.propGrid.SelectedObject = this.memberfactory.GetParameters(this.SelectedType, this.properties);
            this.SetXmlConfigTextFromType(this.SelectedType);
        }

        private void PropGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.SetXmlConfigTextFromType(this.SelectedType);
        }

        private void SetXmlConfigTextFromType(Type parameterType)
        {
            if (parameterType == null)
            {
                return;
            }

            this.tbConfig.Text = this.memberfactory.SerializeSetupData(parameterType, this.properties);
        }
    }
}