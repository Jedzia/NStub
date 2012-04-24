// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LeafExpressionToken.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core.Util.Dumper
{
    using System.Text;

    internal class LeafExpressionToken : ExpressionToken
    {
        // Fields
        #region Fields

        public readonly string Text;

        #endregion

        // Methods
        #region Constructors

        public LeafExpressionToken(string text)
        {
            this.Text = text;
        }

        #endregion

        // Properties
        #region Properties

        public override int Length
        {
            get
            {
                return this.Text.Length;
            }
        }

        public override bool MultiLine
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        #endregion

        public override void Write(StringBuilder sb, int indent)
        {
            sb.Append(this.Text);
        }
    }
}