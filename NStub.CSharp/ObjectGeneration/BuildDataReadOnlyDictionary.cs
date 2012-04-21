// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildDataReadOnlyDictionary.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.Collections.Generic;

    /// <summary>
    /// Wraps build properties of the <see cref="BuildDataDictionary"/> as read only values.
    /// </summary>
    public class BuildDataReadOnlyDictionary : ReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>, 
                                               IBuildDataReadOnlyDictionary
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildDataReadOnlyDictionary"/> class.
        /// </summary>
        /// <param name="root">The dictionary to wrap as read only.</param>
        public BuildDataReadOnlyDictionary(IDictionary<string, IReadOnlyDictionary<string, IBuilderData>> root)
            : base(root)
        {
        }

        #endregion
    }
}