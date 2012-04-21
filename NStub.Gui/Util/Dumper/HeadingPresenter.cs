// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeadingPresenter.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Util.Dumper
{
    internal class HeadingPresenter
    {
        // Fields
        #region Fields

        public object Content;

        /// <summary>
        /// Hide the presenter.
        /// </summary>
        internal bool HidePresenter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadingPresenter"/> class.
        /// </summary>
        public HeadingPresenter(string heading, object content)
        {
            this.Heading = heading;
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        public object Heading { get; set; }

        #endregion
    }
}