// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelperComponent.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Components
{
    #region Imports

    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using NStub.Gui.Util;

    #endregion

    /// <summary>
    /// Helper object that extends the standard application settings mechanism with automatic upgrade of settings.
    /// </summary>
    /// <remarks>
    /// For a detailed description see the remarks on the <see cref="SettingsHelper.GoInstall">SettingsHelper.GoInstall</see> Property. 
    /// <para> </para>
    /// <para>Easy setup steps: </para>
    /// <list type="number">
    /// <item>
    /// <description>Drag the SettingsHelperComponent onto your Main Form.</description></item>
    /// <item>
    /// <description>Set the <see cref="MainForm">Main Form</see> property of the component to your main form. This attach's it to the Closing event and saves the settings when you leave the application. </description></item>
    /// <item>
    /// <description>Provide a boolean property named <i>GoInstall</i> that is by default <c>false</c> with your application settings.</description></item>
    /// <item>
    /// <description>Set the Settings property of the component to your application settings. The <i>this.settings</i> field is the SettingsHelperComponent. </description></item></list>
    /// <code>
    ///     public MainForm()
    ///     {
    ///         this.InitializeComponent();
    ///         this.settings.Settings = Settings.Default;
    ///     }</code>
    /// <para> </para>
    /// <para>Thats it;)</para>
    /// </remarks>
    public class SettingsHelperComponent : SettingsHelper, IComponent
    {
        #region Fields

        /// <summary>
        /// Backing field for the parent form.
        /// </summary>
        private Form mainForm;

        /// <summary>
        /// Backing field for the parent form.
        /// </summary>
        private UserControl mainUserControl;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsHelperComponent"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SettingsHelperComponent(IContainer container)
        {
            container.Add(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent form.
        /// </summary>
        /// <value>The main form.</value>
        [Category("Settings"), DefaultValue(null), 
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), 
         EditorBrowsable(EditorBrowsableState.Always), Description("The form to add a event for, when closing")]
        public Form MainForm
        {
            get
            {
                return this.mainForm;
            }

            set
            {
                if (value.AttachToLoadEvent(ref this.mainForm, this.MainFormLoad))
                {
                    this.MainUserControl = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the parent <see cref="UserControl"/>.
        /// </summary>
        /// <value>The main <see cref="UserControl"/>.</value>
        [Category("Settings"), DefaultValue(null), 
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), 
         EditorBrowsable(EditorBrowsableState.Always), Description("The form to add a event for, when closing")]
        public UserControl MainUserControl
        {
            get
            {
                return this.mainUserControl;
            }

            set
            {
                if (value.AttachToLoadEvent(ref this.mainUserControl, this.MainFormLoad))
                {
                    this.MainForm = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"></see> associated with the <see cref="T:System.ComponentModel.IComponent"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.ISite"></see> object associated with the component; or <c>null</c>, if the component does not have a site.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site { get; set; }

        #endregion

        /// <summary>
        /// Occurs when the parent-form is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            LoadUpgradeSettings();
        }
    }
}