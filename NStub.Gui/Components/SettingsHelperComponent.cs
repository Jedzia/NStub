// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.cs" company="EvePanix">
//   Copyright (c) Jedzia 2009, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <summary>
//   Helper object that extends the standard application settings mechanism with
//   automatic upgrade of settings.
// </summary>
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
    /// Helper object that extends the standard application settings mechanism with 
    /// automatic upgrade of settings. 
    /// </summary>
    /// <remarks>
    /// For a detailed description see the remarks on the
    /// <see cref="SettingsHelper.GoInstall"/> Property.
    /// <para>
    /// <list type="items">Easy setup steps: <item>
    /// 1. Drag the SettingsHelperComponent onto your Main Form. 
    /// </item><item>2. Set the <see cref="MainForm"/> property of the component to your
    /// main form. This attach's it to the Closing event and saves the settings when you leave the application.
    /// </item><item>3. Set the Settings property of the component to your application settings.
    /// <code><![CDATA[
    /// public MainForm()
    /// {
    ///     this.InitializeComponent();
    ///     this.settings.Settings = Settings.Default;   
    /// }
    /// ]]></code>
    /// , where <i>this.settings</i> is the SettingsHelperComponent.
    /// </item><item>4. Provide a boolean property named <i>GoInstall</i> and by default <b>false</b> with your application settings.</item>
    /// </para>
    /// </list><para>Thats it;)</para>
    /// </remarks>
    public class SettingsHelperComponent : SettingsHelper, IComponent
    {
        /// <summary>
        /// Backing field for the parrent form.
        /// </summary>
        private Form mainForm;

        /// <summary>
        /// Backing field for the parrent form.
        /// </summary>
        private UserControl mainUserControl;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsHelperComponent"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SettingsHelperComponent(IContainer container)
        {
            container.Add(this);
        }

        /// <summary>
        /// Gets or sets the parrent form.
        /// </summary>
        /// <value>The main form.</value>
        [Category("Settings"), DefaultValue(null), 
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), 
         EditorBrowsable(EditorBrowsableState.Always), Description("The form to add a event for, when closing")]
        public Form MainForm
        {
            get { return this.mainForm; }
            set 
            { 
                if (value.AttachToLoadEvent(ref this.mainForm, this.MainFormLoad))
                {
                    this.MainUserControl = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the parrent UserControl.
        /// </summary>
        /// <value>The main UserControl.</value>
        [Category("Settings"), DefaultValue(null),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         EditorBrowsable(EditorBrowsableState.Always), Description("The form to add a event for, when closing")]
        public UserControl MainUserControl
        {
            get { return this.mainUserControl; }
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
        /// <returns>The <see cref="T:System.ComponentModel.ISite"></see> object associated with the component; or null, if the component does not have a site.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site { get; set; }

        /// <summary>
        /// Occurs when the parrent-form is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.LoadUpgradeSettings();
        }
    }
}