// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWrittenEventArgs.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Util.Dumper
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="XhtmlWriter.Event"/> event.
    /// </summary>
    [Serializable]
    public class TextWrittenEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWrittenEventArgs"/> class
        /// </summary>
        public TextWrittenEventArgs(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the text of this instance.
        /// </summary>
        public string Text { get; private set; }

        #endregion
    }
}