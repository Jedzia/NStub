// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoggable.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui
{
    /// <summary>
    /// Provides simple logging capabilities.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Logs the specified text.
        /// </summary>
        /// <param name="text">The message text.</param>
        void Log(string text);
    }
}