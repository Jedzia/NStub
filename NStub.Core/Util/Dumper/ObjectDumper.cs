// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectDumper.cs" company="EvePanix">
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
    using System.Collections;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Object low level dumper.
    /// </summary>
    public class ObjectDumper
    {
        #region Fields

        private readonly int depth;
        private int level;
        private int pos;
        private TextWriter writer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDumper"/> class.
        /// </summary>
        /// <param name="depth">The iteration level.</param>
        /// <param name="writer">The output text writer.</param>
        internal ObjectDumper(int depth, TextWriter writer)
        {
            this.depth = depth;
            this.writer = writer;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ObjectDumper"/> class from being created.
        /// </summary>
        /// <param name="depth">The iteration level.</param>
        private ObjectDumper(int depth)
        {
            this.depth = depth;
        }

        #endregion

        #region Properties

        private TextWriter Writer
        {
            get
            {
                return this.writer;
            }

            set
            {
                this.writer = value;
            }
        }

        #endregion

        /// <summary>
        /// Writes the specified element to the output.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void Write(object element)
        {
            Write(element, 0);
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="depth">The iteration level.</param>
        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="depth">The iteration level.</param>
        /// <param name="log">The output logger.</param>
        public static void Write(object element, int depth, TextWriter log)
        {
            ObjectDumper dumper = new ObjectDumper(depth);
            dumper.writer = log;
            dumper.Write("[" + element.GetType().Name + "]");
            dumper.WriteObject(null, element);
        }

        internal void WriteObject(object element)
        {
            this.WriteObject(string.Empty, element);
        }

        internal void WriteObject(string prefix, object element)
        {
            // WriteIndent();
            // Write(prefix);
            // WriteLine();

            if (element == null || element is ValueType || element is string)
            {
                this.WriteIndent();
                this.Write(prefix);

                // Write(" [" + element.GetType().FullName + "] ");
                this.WriteValue(element);
                this.WriteLine();
            }
            else
            {
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            this.WriteIndent();
                            this.Write(prefix);
                            this.Write("...");
                            this.WriteLine();
                            if (this.level < this.depth)
                            {
                                this.level++;
                                this.WriteObject(prefix, item);
                                this.level--;
                            }
                        }
                        else
                        {
                            this.WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    this.WriteIndent();
                    if (prefix != null && members.Length > 0)
                    {
                        this.Write("[" + element.GetType().Name + "]");
                    }

                    this.Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                this.WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }

                            // Write("[" + element.GetType().Name + "]");
                            this.Write(m.Name);
                            this.Write("=");
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if (t.IsValueType || t == typeof(string))
                            {
                                this.WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    this.Write("...");
                                }
                                else
                                {
                                    this.Write("{ }");
                                }
                            }
                        }
                    }

                    if (propWritten)
                    {
                        this.WriteLine();
                    }

                    if (this.level < this.depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        this.level++;
                                        this.WriteObject(m.Name + ": ", value);
                                        this.level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void Write(string s)
        {
            if (s != null)
            {
                this.writer.Write(s);
                this.pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < this.level; i++)
            {
                this.writer.Write("  ");
            }
        }

        private void WriteLine()
        {
            this.writer.WriteLine();
            this.pos = 0;
        }

        private void WriteTab()
        {
            this.Write("  ");
            while (this.pos % 8 != 0)
            {
                this.Write(" ");
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                this.Write("null");
            }
            else if (o is DateTime)
            {
                this.Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                this.Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                this.Write("...");
            }
            else
            {
                this.Write("{ }");
            }
        }
    }
}