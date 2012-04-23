// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XhtmlWriter.cs" company="EvePanix">
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
    using System.Text;
    using System.Threading;

    /// <summary>
    /// StringWriter with an event that is fired on text changes.
    /// </summary>
    /// <remarks>
    /// Fake output writer. does not really write xhtml .... to be done ... later
    /// </remarks>
    public class XhtmlWriter : StringWriter
    {
        #region Fields

        private readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        /// The event handler for the <see cref="XhtmlWriter.TextChanged"/> event.
        /// </summary>
        private EventHandler<TextWrittenEventArgs> textChanged;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the stream gets a <see cref="TextWriter.NewLine"/> or is flushed/closed and not empty. 
        /// </summary>
        /// <remarks>
        /// The <see cref="TextWrittenEventArgs.Text"/> parameter reflects the current text in the stream. 
        /// After the event is fired, the internal memory is blanked and starts watching for <see cref="TextWriter.NewLine"/> again.
        /// <para>The internal memory for event invocation has nothing to do with the underlying stream,
        /// that behaves like a standard <see cref="StringWriter"/>.</para></remarks>
        public event EventHandler<TextWrittenEventArgs> TextChanged
        {
            add
            {
                this.textChanged += value;
            }

            remove
            {
                this.textChanged -= value;
            }
        }

        #endregion

        /// <summary>
        /// Closes the current <see cref="System.IO.StringWriter"/> and the underlying stream.
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (this.sb.Length > 0)
            {
                this.RaiseTextChanged(this.sb.ToString());
                this.sb.Length = 0;

                // sb = null;
            }
        }

        /// <summary>
        /// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if (this.sb.Length > 0)
            {
                // if (sb.Length > 200)
                var currentStr = this.sb.ToString();
                this.RaiseTextChanged(currentStr);
                this.sb.Length = 0;
            }
            base.Flush();
        }

        /// <summary>
        /// Writes a character to this instance of the StringWriter.
        /// </summary>
        /// <param name="value">The character to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The writer is closed.
        ///   </exception>
        public override void Write(char value)
        {
            base.Write(value);

            this.sb.Append(value);
            this.FlushForNewLine();
        }

        /// <summary>
        /// Writes a string to this instance of the StringWriter.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The writer is closed.
        ///   </exception>
        public override void Write(string value)
        {
            base.Write(value);

            this.sb.Append(value);
            this.FlushForNewLine();
        }

        // MemoryStream ms = new MemoryStream();


        /// <summary>
        /// Writes the specified region of a character array to this instance of the StringWriter.
        /// </summary>
        /// <param name="buffer">The character array to read data from.</param>
        /// <param name="index">The index at which to begin reading from <paramref name="buffer"/>.</param>
        /// <param name="count">The maximum number of characters to write.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        ///   </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> or <paramref name="count"/> is negative.
        ///   </exception>
        /// <exception cref="T:System.ArgumentException">
        /// (<paramref name="index"/> + <paramref name="count"/>)&gt; <paramref name="buffer"/>. Length.
        ///   </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The writer is closed.
        ///   </exception>
        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);

            // ms.Write(buffer, index, count);
            this.sb.Append(buffer, index, count);
            this.FlushForNewLine();

            // RaiseTextChanged(ms.ToString());
        }

        /// <summary>
        /// Raises the <see cref="E:"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TextWrittenEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTextChanged(TextWrittenEventArgs e)
        {
            EventHandler<TextWrittenEventArgs> handler = Interlocked.CompareExchange(ref this.textChanged, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the text changed event.
        /// </summary>
        /// <param name="text">The text of the event arguments.</param>
        protected virtual void RaiseTextChanged(string text)
        {
            this.OnTextChanged(new TextWrittenEventArgs(text));
        }

        private void FlushForNewLine()
        {
            var currentStr = this.sb.ToString();
            if (currentStr.Contains(NewLine))
            {
                // if (sb.Length > 200)
                this.RaiseTextChanged(currentStr);
                this.sb.Length = 0;
            }
        }
    }
}