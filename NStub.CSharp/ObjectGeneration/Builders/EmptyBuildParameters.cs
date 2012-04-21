// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyBuildParameters.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    /// <summary>
    /// Provides an implementation of the <see cref="IMemberBuildParameters"/> user data, that has an empty set of data.
    /// </summary>
    public class EmptyBuildParameters : EmptyBuildParametersBase<EmptyBuildParameters>, IMemberBuildParameters
    {
    }
}