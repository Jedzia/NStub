// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Server.cs" company="EvePanix">
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
    using System.IO;

    /// <summary>
    /// Main instance provider for object dumping. 
    /// </summary>
    public class Server
    {
        #region Fields

        private readonly XhtmlWriter lambdaFormatter;
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
                this.lambdaFormatter.TextChanged += value;
            }

            remove
            {
                this.lambdaFormatter.TextChanged -= value;
            }
        }

        #endregion

        #region Properties

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

            // set { Server.lambdaFormatter = value; }
        }

        #endregion
    }
}