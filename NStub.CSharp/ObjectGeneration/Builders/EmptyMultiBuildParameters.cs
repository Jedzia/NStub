// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyMultiBuildParameters.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a implementation of the <see cref="IMultiBuildParameters"/> user data, that has an empty set of data.
    /// </summary>
    [Description("Default MultiBuilder parameter set.")]
    public class EmptyMultiBuildParameters : EmptyMultiBuildParametersBase<EmptyMultiBuildParameters>
    {
    }
}