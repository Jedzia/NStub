// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuildDataReadOnlyCollection.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Provides read only access to <see cref="IBuilderData"/> organized as a categorized dictionary lookup with read only access.
    /// </summary>
    public interface IBuildDataReadOnlyDictionary :
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>
    {
    }
}