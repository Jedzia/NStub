namespace NStub.Core.Util.Dumper
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Linq;

    /// <summary>
    /// Object dumper extensions.
    /// </summary>
    public static class Extensions
    {
        // Methods
        /*public static string Disassemble(this MethodBase method)
        {
            return Disassembler.Disassemble(method);
        }*/

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o)
        {
            return o.Dump<T>(null, null, 0, int.MaxValue);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="maximumDepth">The maximum dump regression depth.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, int maximumDepth)
        {
            return o.Dump<T>(null, null, maximumDepth, int.MaxValue);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="maxcount">The maximum count of dumps.</param>
        /// <param name="maximumDepth">The maximum dump regression depth.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, int maximumDepth, int maxcount)
        {
            return o.Dump<T>(null, null, maximumDepth, maxcount);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, string description)
        {
            return o.Dump<T>(null, description, 0, int.MaxValue);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="description">The description.</param>
        /// <param name="maximumDepth">The maximum dump regression depth.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, string description, int maximumDepth)
        {
            return o.Dump<T>(null, description, maximumDepth, int.MaxValue);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="description">The description.</param>
        /// <param name="maximumDepth">The maximum dump regression depth.</param>
        /// <param name="maxcount">The maximum count of dumps.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, string description, int maximumDepth, int maxcount)
        {
            return o.Dump<T>(null, description, maximumDepth, maxcount);
        }

        /// <summary>
        /// Dumps the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="o">The object to dump.</param>
        /// <param name="prefix">The prefix to print on the left side.</param>
        /// <param name="description">The description text.</param>
        /// <param name="maximumDepth">The maximum dump regression depth.</param>
        /// <param name="maxcount">The maximum count of dumps.</param>
        /// <returns>
        /// The untouched object for fluent usage of <c>Dump</c>.
        /// </returns>
        public static T Dump<T>(this T o, string prefix, string description, int maximumDepth, int maxcount)
        {

            if (maximumDepth < 0)
            {
                maximumDepth = 0;
            }
            if (maximumDepth > 20)
            {
                maximumDepth = 20;
            }

            if (!string.IsNullOrEmpty(description))
            {
                maximumDepth++;
            }

            if (maxcount < 0)
            {
                maxcount = 0;
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
                    expr = /*(Expression)*/ o as Expression;
                    //expr = (Expression) (object)o;
                    // throw new NotImplementedException("Dump: o is Expression");
                }
            }
            string content = "";
            if (expr != null)
            {
                //throw new NotImplementedException("Dump: o is Expression");
                try
                {
                    ExpressionToken token = ExpressionToken.Visit(expr);
                    if (token != null)
                    {
                       content = token.ToString();
                    }
                }
                catch (Exception exception)
                {
                    Server.Default.LambdaFormatter.Write("Dump ExpressionToken Visit \r\n" + exception.ToString());
                }
            }
            //XhtmlWriter currentResultsWriter = Server.CurrentResultsWriter;
            var currentResultsWriter = new ObjectDumper(maximumDepth, maxcount, lambdaFormatter);
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
}
