using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using System.Linq;

namespace NStub.Gui
{

    /// <summary>
    /// Provides data for the <see cref="E:XhtmlWriter.Event"/> event.
    /// </summary>
    [global::System.Serializable]
    public class TextWrittenEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the text of this instance.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWrittenEventArgs"/> class
        /// </summary>
        public TextWrittenEventArgs(string text)
        {
            this.Text = text;
        }
    }
    
    public class XhtmlWriter : StringWriter
    {

        /// <summary>
        /// The event handler for the <see cref="E:XhtmlWriter.TextChanged"/> event.
        /// </summary>
        private EventHandler<TextWrittenEventArgs> textChanged;

        /// <summary>
        /// Occurs when 
        /// </summary>
        public event EventHandler<TextWrittenEventArgs> TextChanged
        {
            add
            {
                // TODO: write your implementation of the add accessor here
                this.textChanged += value;
            }

            remove
            {
                // TODO: write your implementation of the remove accessor here
                this.textChanged -= value;
            }
        }

        /// <summary>
        /// Raises the text changed event.
        /// </summary>
        /// <param name="text">The text of the event arguments.</param>
        protected virtual void RaiseTextChanged(string text)
        {
            OnTextChanged(new TextWrittenEventArgs(text));
        }

        /// <summary>
        /// Raises the <see cref="E:"/> event.
        /// </summary>
        /// <param name="e">The <see cref="TextWrittenEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTextChanged(TextWrittenEventArgs e)
        {
            EventHandler<TextWrittenEventArgs> handler = System.Threading.Interlocked.CompareExchange(ref this.textChanged, null, null);

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void Write(char value)
        {
            base.Write(value);
            
            sb.Append(value);
            FlushForEvent();
        }

        public override void Write(string value)
        {
            base.Write(value);

            sb.Append(value);
            FlushForEvent();
        }

        private void FlushForEvent()
        {
            if (sb.ToString().Contains(this.NewLine))
            //if (sb.Length > 200)
            {
                RaiseTextChanged(sb.ToString());
                sb.Length = 0;
            }
        }

        StringBuilder sb = new StringBuilder();
        //MemoryStream ms = new MemoryStream();


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

            //ms.Write(buffer, index, count);
            sb.Append(buffer, index, count);
            FlushForEvent();
            //RaiseTextChanged(ms.ToString());
        }

        public override void Close()
        {
            base.Close();
            if (sb.Length > 0)
            {
                RaiseTextChanged(sb.ToString());
                sb.Length = 0;
                //sb = null;
            }
        }
    }

    public class Server
    {
        XhtmlWriter lambdaFormatter;
        static Server defaultServer;

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
            //set { Server.lambdaFormatter = value; }
        }

        public TextWriter LambdaFormatter
        {
            get { return this.lambdaFormatter; }
            //set { Server.lambdaFormatter = value; }
        }

        /// <summary>
        /// Occurs when 
        /// </summary>
        public event EventHandler<TextWrittenEventArgs> TextChanged
        {
            add
            {
                //this.textChanged += value;
                this.lambdaFormatter.TextChanged += value;
            }

            remove
            {
                this.lambdaFormatter.TextChanged -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Server"/> class.
        /// </summary>
        private Server()
        {
            lambdaFormatter = new XhtmlWriter();
            //var sss = new Stream();
        }
    }

    public static class Extensions
    {
        // Methods
        /*public static string Disassemble(this MethodBase method)
        {
            return Disassembler.Disassemble(method);
        }*/

        public static T Dump<T>(this T o)
        {
            return o.Dump<T>(null, null);
        }

        public static T Dump<T>(this T o, int maximumDepth)
        {
            return o.Dump<T>(null, new int?(maximumDepth));
        }

        public static T Dump<T>(this T o, string description)
        {
            return o.Dump<T>(description, null);
        }
        //public static string Text { get; private set; }
        public static T Dump<T>(this T o, string description, int? maximumDepth)
        {
            if (maximumDepth < 0)
            {
                maximumDepth = null;
            }
            if (maximumDepth > 20)
            {
                maximumDepth = 20;
            }
            Expression expr = null;
            //TextWriter lambdaFormatter = new StringWriter();
            TextWriter lambdaFormatter = Server.Default.LambdaFormatter;
            if (lambdaFormatter != null)
            {
                if (o is IQueryable)
                {
                    expr = ((IQueryable)o).Expression;
                }
                else if (o is Expression)
                {
                    //expr = (Expression) o;
                    throw new NotImplementedException("Dump: o is Expression");
                }
            }
            string content = "";
            if (expr != null)
            {
                throw new NotImplementedException("Dump: o is Expression");
                try
                {
                    //ExpressionToken token = ExpressionToken.Visit(expr);
                    //if (token != null)
                    {
                        //   content = token.ToString();
                    }
                }
                catch (Exception exception)
                {
                    //Log.Write(exception, "Dump ExpressionToken Visit");
                }
            }
            //XhtmlWriter currentResultsWriter = Server.CurrentResultsWriter;
            var currentResultsWriter = new ObjectDumper(maximumDepth.GetValueOrDefault(), lambdaFormatter);
            bool flag = o is Type;
            if (flag)
            {
                //ObjectNode.ExpandTypes = true;
            }
            try
            {
                TextWriter writer3;
                if (!string.IsNullOrEmpty(description))
                {
                    if (content.Length > 0)
                    {
                        lock ((writer3 = lambdaFormatter))
                        {
                            lambdaFormatter.WriteLine(new HeadingPresenter(description, content));
                        }
                    }
                    HeadingPresenter presenter = new HeadingPresenter(description, o);
                    if (currentResultsWriter != null)
                    {
                        //currentResultsWriter.WriteDepth(presenter, maximumDepth);
                        //ObjectDumper.Write(o, maximumDepth.GetValueOrDefault(), lambdaFormatter);
                        //currentResultsWriter.Write("[" + o.GetType().Name + "]");
                        //currentResultsWriter.Writer = lambdaFormatter;
                        //currentResultsWriter.WriteObject(null, o);
                        currentResultsWriter.WriteObject(presenter);
                        //Text = lambdaFormatter.ToString();
                        return o;
                    }
                    Console.Write(presenter);
                    return o;
                }
                if (content.Length > 0)
                {
                    lock ((writer3 = lambdaFormatter))
                    {
                        lambdaFormatter.WriteLine(content + "\r\n");
                    }
                }
                if (currentResultsWriter != null)
                {
                    //currentResultsWriter.WriteLineDepth(o, maximumDepth);
                    currentResultsWriter.WriteObject(o);
                    //Text = lambdaFormatter.ToString();
                    return o;
                }
                //ObjectDumper.Write(o);
                Console.WriteLine(o);
            }
            finally
            {
                if (flag)
                {
                    //ObjectNode.ExpandTypes = false;
                }
            }
            return o;
        }

        /*internal static DataSet ExecuteDataSet(this DbCommand cmd)
        {
            if ((cmd == null) || (cmd.Connection == null))
            {
                return null;
            }
            DbConnection connection = cmd.Connection;
            using (DbDataAdapter adapter = cmd.Connection.GetFactory().CreateDataAdapter())
            {
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }*/

        /*internal static DbProviderFactory GetFactory(this DbConnection cx)
        {
            if (cx == null)
            {
                return null;
            }
            PropertyInfo property = cx.GetType().GetProperty("DbProviderFactory", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                return null;
            }
            return (property.GetValue(cx, null) as DbProviderFactory);
        }

        public static object ToImage(this Binary imageData)
        {
            return Util.Image(imageData);
        }*/
    }

    internal class HeadingPresenter
    {
        // Fields
        public object Content;
        public object Heading { get; set; }
        internal bool HidePresenter;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HeadingPresenter"/> class.
        /// </summary>
        public HeadingPresenter(string heading, object content)
        {
            this.Heading = heading;
            this.Content = content;
        }
    }

}
