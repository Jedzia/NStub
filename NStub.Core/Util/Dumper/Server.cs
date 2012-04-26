// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Server.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core.Util.Dumper
{
    using System;
    using System.IO;

    /// <summary>
    /// Main instance provider for object dumping. 
    /// </summary>
    public class Server
    {
        #region Fields

        //private readonly XhtmlWriter lambdaFormatter;
        private TextWriter lambdaFormatter;
        private static Server defaultServer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server"/> class.
        /// </summary>
        private Server()
        {
            this.lambdaFormatter = new XhtmlWriter();

            // var sss = new Stream();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the dumper text changes.
        /// </summary>
        public event EventHandler<TextWrittenEventArgs> TextChanged
        {
            add
            {
                // this.textChanged += value;
                if (this.lambdaFormatter is XhtmlWriter)
                {
                    ((XhtmlWriter)this.lambdaFormatter).TextChanged += value;
                }
            }

            remove
            {
                if (this.lambdaFormatter is XhtmlWriter)
                {
                    ((XhtmlWriter)this.lambdaFormatter).TextChanged -= value;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Switch the <see cref="LambdaFormatter"/> to <see cref="Console.Out"/>.
        /// </summary>
        public static void ToConsoleOut()
        {
            Default.LambdaFormatter = Console.Out;
        }

        /// <summary>
        /// Gets the default server singleton instance.
        /// </summary>
        public static Server Default
        {
            get
            {
                if (defaultServer == null)
                {
                    defaultServer = new Server();
                }

                return defaultServer;
            }

            // set { Server.lambdaFormatter = value; }
        }

        /// <summary>
        /// Gets the default lambda formatter.
        /// </summary>
        public TextWriter LambdaFormatter
        {
            get
            {
                return this.lambdaFormatter;
            }

            set { this.lambdaFormatter = value; }
        }

        #endregion
    }
}