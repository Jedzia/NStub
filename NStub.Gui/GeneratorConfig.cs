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
        /// Gets the selected <see cref="IMultiBuilder"/> or <c>null</c> when none is selected.
        /// </summary>
        protected MultiLookup SelectedMulti
        {
            get
            {
                /*if (this.chklbParameters.SelectedItem == null)
                {
                    return null;
                }*/
                //return (MultiLookup)this.chklbParameters.SelectedItem;
                return this.chklbParameters.SelectedItem as MultiLookup;
            }
        }

        /// <summary>
        /// Gets the type of the selected <see cref="IMemberBuilder"/> or <c>null</c> when none is selected.
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

        #endregion

        /// <summary>
        /// Adds the multi parameter to the configured builders checked list box.
        /// </summary>
        /// <param name="multi">The <see cref="IMultiBuilder"/> parameter lookup object.</param>
        private void AddMultiParameterToBox(MultiLookup multi)
        {
            // var chkItemIndex = this.chklbParameters.Items.Add(para);
            var chkItemIndex = this.chklbParameters.Items.Add(multi);
            this.chklbParameters.SetItemChecked(chkItemIndex, multi.Parameters.Enabled);
        }

        /// <summary>
        /// Adds the static builder parameter to the configured builders checked list box.
        /// </summary>
        /// <param name="builderType">The Type of the <see cref="IMemberBuilder"/> to add.</param>
        /// <param name="para">The parameter associated with the specified <paramref name="builderType"/>.</param>
        private void AddParameterToBox(Type builderType, IMemberBuildParameters para)
        {
            var chkItemIndex = this.chklbParameters.Items.Add(builderType);
            this.chklbParameters.SetItemChecked(chkItemIndex, para.Enabled);
        }

        /// <summary>
        /// Is attached to the <see cref="Form.Load"/> event and initializes this instance for startup.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void GeneratorConfigLoad(object sender, EventArgs e)
        {
            foreach (var builderType in this.memberfactory.BuilderTypes)
            {
                if (typeof(IMultiBuilder).IsAssignableFrom(builderType))
                {
                    var listViewItem = this.lviewBuilderTypes.Items.Add(builderType.Name);

                    // AddMultiParameterToBox(builderType, 
                    listViewItem.Tag = builderType;
                }
                else
                {
                    var para = this.memberfactory.GetParameters(builderType, this.properties);
                    this.AddParameterToBox(builderType, para);
                }
            }

            var multis = ((MemberBuilderFactory)this.memberfactory).MultiParameters(this.properties);
            foreach (var multi in multis)
            {
                this.AddMultiParameterToBox(multi);

                /*foreach (var item in multi.Lookup)
                {
                    var s = item.Value;
                    var multipara = item.Value as IMultiBuildParameters;
                    AddMultiParameterToBox(multi.BuilderType, multipara);
                }*/
            }
        }

        /*private void AddMultiParameterToBox(Type builderType, IMultiBuildParameters para)
        {
            //var chkItemIndex = this.chklbParameters.Items.Add(para);
            var chkItemIndex = this.chklbParameters.Items.Add(builderType);
            this.chklbParameters.SetItemChecked(chkItemIndex, para.Enabled);
        }*/

        // todo bessere benamsung ... blickt ja keine sau mehr durch

        /// <summary>
        /// Handles the <see cref="CheckedListBox.ItemCheck"/> event of the configured builders checked list box.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemCheckEventArgs"/> instance containing the event data.</param>
        private void ChkLbParametersItemCheck(object sender, ItemCheckEventArgs e)
        {
            // var current = e.CurrentValue;
            var newValue = e.CurrentValue != CheckState.Checked;

            if (this.SelectedType != null)
            {
                // das bloed, wieso nicht gleich den parameter speichern? ... oder im multi lookup?
                var item = this.memberfactory.GetParameters(this.SelectedType, this.properties);
                if (item.Enabled != newValue)
                {
                    item.Enabled = newValue;
                    this.tbConfig.Text = this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
                }
            }
            else if (this.SelectedMulti != null)
            {
                // if (this.SelectedMulti != null)
                var item = this.SelectedMulti.Parameters;
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

        /// <summary>
        /// Handles the <see cref="CheckedListBox.SelectedIndexChanged"/> event of the configured builders checked list box.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ChkLbParametersSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedType != null)
            {
                this.propGrid.SelectedObject = this.memberfactory.GetParameters(this.SelectedType, this.properties);
                this.tbConfig.Text = this.memberfactory.GetBuilderDescription(this.SelectedType) + Environment.NewLine;
                this.tbConfig.Text += this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
            }
            else if (this.SelectedMulti != null)
            {
                // if (this.SelectedMulti != null)
                this.propGrid.SelectedObject = this.SelectedMulti.Parameters;
                this.tbConfig.Text = this.memberfactory.GetBuilderDescription(this.SelectedMulti.BuilderType) +
                                     Environment.NewLine;
                this.tbConfig.Text += this.memberfactory.SerializeSetupData(
                    this.SelectedMulti.Parameters.Id,
                    this.SelectedMulti.BuilderType,
                    this.properties);
            }
        }

        /// <summary>
        /// Handles the <see cref="PropertyGrid.PropertyValueChanged"/> event.
        /// </summary>
        /// <param name="s">The event sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        private void PropGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.SelectedType != null)
            {
                this.tbConfig.Text = this.memberfactory.SerializeSetupData(this.SelectedType, this.properties);
            }
            else if (this.SelectedMulti != null)
            {
                // if (this.SelectedMulti != null)
                this.tbConfig.Text = this.memberfactory.SerializeSetupData(
                    this.SelectedMulti.Parameters.Id,
                    this.SelectedMulti.BuilderType,
                    this.properties);
            }
        }

        /// <summary>
        /// Handles a click on the add multi builder button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnAddClick(object sender, EventArgs e)
        {
            if (this.lviewBuilderTypes.SelectedItems.Count < 1)
            {
                return;
            }

            var selected = this.lviewBuilderTypes.SelectedItems[0];
            var seltype = (Type)selected.Tag;
            var mbpara = this.memberfactory.GetMultiParameter(Guid.Empty, seltype, this.properties);
            if (mbpara != null)
            {
                this.AddMultiParameterToBox(new MultiLookup { BuilderType = seltype, Parameters = mbpara });
            }
        }

        /// <summary>
        /// Handles a click on the remove multi builder button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnRemoveClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the <see cref="ListView.SelectedIndexChanged"/> event of the available multi builders list view.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ListViewBuilderTypesSelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}