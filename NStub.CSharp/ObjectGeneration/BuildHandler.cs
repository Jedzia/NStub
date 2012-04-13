// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildHandler.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using NStub.CSharp.BuildContext;

    public class BuildHandler : IBuildHandler
    {
        #region Fields

        private readonly Func<IMemberBuildContext, bool> handler;
        private readonly Type type;

        #endregion

        #region Constructors

        public BuildHandler(Type type, Func<IMemberBuildContext, bool> handler)
        {
            this.type = type;
            this.handler = handler;
        }

        #endregion

        #region Properties

        public Func<IMemberBuildContext, bool> Handler
        {
            get
            {
                return this.handler;
            }
        }

        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        #endregion
    }
}