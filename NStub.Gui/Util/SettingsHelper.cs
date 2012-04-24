// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Util
{
    #region Imports

    using System;
    using System.ComponentModel;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Helper object that extends the standard application settings mechanism with 
    /// automatic upgrade of settings. 
    /// </summary>
    /// <remarks>
    /// For a detailed description see the remarks on the
    /// <see cref="GoInstall"/> Property.
    /// </remarks>
    public class SettingsHelper : IDisposable
    {
        #region Fields

        /// <summary>
        /// Backing field for the <see cref="GoInstall"/> property.
        /// </summary>
        private string goinstall = "GoInstall";

        /// <summary>
        /// Indicates that this instance is disposed.
        /// </summary>
        private bool isdisposed;

        /// <summary>
        /// The settings of this instance.
        /// </summary>
        private ApplicationSettingsBase settings;

        /// <summary>
        /// Backing field for the <see cref="ShouldSaveChanges"/> property.
        /// </summary>
        private bool shouldSaveChanges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsHelper"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="goinstall">
        /// default setting parameter
        /// used for updating.
        /// </param>
        public SettingsHelper(ApplicationSettingsBase settings, string goinstall)
            : this()
        {
            this.settings = settings;
            this.goinstall = goinstall;
            this.Disposed = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsHelper"/> class.
        /// </summary>
        protected SettingsHelper()
        {
            this.shouldSaveChanges = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Represents the method that handles the <see cref="E:System.ComponentModel.IComponent.Disposed"></see> event of a component.
        /// </summary>
        public event EventHandler Disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a string identifying the default setting parameter 
        /// used for updating.
        /// </summary>
        /// <value>The name of the setting ( has to be a boolean value ) used for updating.</value>
        /// <remarks>
        /// Your "GoInstall" setting has to be boolean and by default set to <c>false</c>. This
        /// enables the application to detect this state, that occurs only when new
        /// settings are generated from the framework by default. So, when a new version setting
        /// is loaded, the <see cref="SettingsHelper"/> calls the upgrade method and updates
        ///  the settings from the old version.
        /// </remarks>
        [Category("Settings"), DefaultValue("GoInstall"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         EditorBrowsable(EditorBrowsableState.Always),
         Description("A string identifying the default setting parameter.")]
        public string GoInstall
        {
            get
            {
                return this.goinstall;
            }

            set
            {
                this.goinstall = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed
        {
            get
            {
                return this.isdisposed;
            }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger of this instance.
        /// </value>
        public ILoggable Logger { get; set; }

        /// <summary>
        /// Gets or sets the application settings default class.
        /// </summary>
        /// <value>The settings.</value>
        [Category("Settings"), DefaultValue(null),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         EditorBrowsable(EditorBrowsableState.Always), Description("The application settings defaultclass.")]
        public ApplicationSettingsBase Settings
        {
            get
            {
                return this.settings;
            }

            set
            {
                this.settings = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether every settings property change [should save changes].
        /// </summary>
        /// <value>
        /// <c>true</c> if [every change should save settings]; otherwise, <c>false</c>.
        /// </value>
        [Category("Settings"), DefaultValue(true),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         EditorBrowsable(EditorBrowsableState.Always), Description("true if every change should save settings")]
        public bool ShouldSaveChanges
        {
            get
            {
                return this.shouldSaveChanges;
            }

            set
            {
                this.shouldSaveChanges = value;
            }
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!this.isdisposed)
            {
                this.isdisposed = true;
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }

                if (this.shouldSaveChanges && (this.settings != null))
                {
                    this.settings.Save();
                }
            }
        }

        /// <summary>
        /// Load and upgrade the settings.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Settings and the install helper setting must be set!
        /// </exception>
        public void LoadUpgradeSettings()
        {
            if (this.settings == null || string.IsNullOrEmpty(this.goinstall))
            {
                throw new NullReferenceException("Settings and the install helper setting must be set!");
            }

            try
            {
                this.settings.Reload();
                if (!(bool)this.settings[this.goinstall])
                {
                    this.settings.Upgrade();
                    this.settings[this.goinstall] = true;
                    this.settings.Save();
                }
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.Log(string.Format("{0},{1}: {2}", "SettingsHelper", "LoadUpgradeSettings", ex));
                }
            }

            this.settings.PropertyChanged += this.SettingsPropertyChanged;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Settings control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.shouldSaveChanges && (this.settings != null))
            {
                this.settings.Save();
            }
        }
    }
}