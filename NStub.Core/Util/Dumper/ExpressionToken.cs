using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NStub.Core.Util.Dumper
{
    public abstract class ExpressionToken
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ExpressionToken Visit(Expression expr)
        {
            ConstantExpression constantExpression = expr as ConstantExpression;
            if (constantExpression != null)
            {
                return Visit(constantExpression);
                //continue;
            }

            /*Lion lion = expr as Lion;
            if (lion != null)
            {
                Visit(lion);
                continue;
            }

            Snake snake = expr as Snake;
            if (snake != null)
            {
                Visit(snake);
                continue;
            }*/

            /*Tiger tiger = expr as Tiger;
            if (tiger != null)
            {
                Visit(tiger);
                continue;
            }*/

            throw new InvalidOperationException();

            //return Visit(expr);
        }

        /*private static ExpressionToken Visit(BinaryExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(ConditionalExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }*/
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ExpressionToken Visit(ConstantExpression exp)
        {
        /*    Func<PropertyInfo, bool> func = null;
            //if (((exp.Value != null) && exp.Type.IsGenericType) && (exp.Type.GetGenericTypeDefinition() == typeof(Table<int>)))
            {
                PropertyInfo property = exp.Value.GetType().GetProperty("Context");
            }
            ExpressionToken returnValue = new ConstantExpressionToken(exp);
            return returnValue;
        }

        {*/
    Func<PropertyInfo, bool> func = null;
    /*if (((exp.Value != null) && exp.Type.IsGenericType) && (exp.Type.GetGenericTypeDefinition() == typeof(Table<>)))
    {
        PropertyInfo property = exp.Value.GetType().GetProperty("Context");
        if (property != null)
        {
            object obj2 = property.GetValue(exp.Value, null);
            if (obj2 != null)
            {
                if (func == null)
                {
                    <>c__DisplayClass11 class2;
                    func = new Func<PropertyInfo, bool>(class2, (IntPtr) this.<Visit>b__f);
                }
                PropertyInfo info2 = Enumerable.First<PropertyInfo>(obj2.GetType().GetProperties(), func);
                if (info2 != null)
                {
                    return new LeafExpressionToken(info2.Name);
                }
            }
        }
    }
    else*/
            if (((exp.Value != null) && exp.Type.IsGenericType) && 
               ((exp.Type.FullName.StartsWith("System.Data.Objects.ObjectQuery`1", StringComparison.Ordinal)
               || exp.Type.FullName.StartsWith("System.Data.Objects.ObjectSet`1", StringComparison.Ordinal))
               || exp.Type.FullName.StartsWith("System.Data.Services.Client.DataServiceQuery`1")))
    {
        return new LeafExpressionToken(exp.Type.GetGenericArguments()[0].Name);
    }
    return new LeafExpressionToken((exp.Value == null) ? "null" : ((exp.Value is string) ? 
        ('"' + ((string) exp.Value) + '"') : exp.Value.ToString()));
}

        /*private static ExpressionToken Visit(InvocationExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(LambdaExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(ListInitExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(MemberAssignment mb)
        {
            ExpressionToken returnValue = null;
            //ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(MemberExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(MemberInitExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(MemberListBinding mb)
        {
            ExpressionToken returnValue = null;
            //ExpressionToken returnValue = new ExpressionToken(mb);
            return returnValue;
        }
        private static ExpressionToken Visit(MemberMemberBinding mb)
        {
            ExpressionToken returnValue = null;
            //ExpressionToken returnValue = new ExpressionToken(mb);
            return returnValue;
        }
        private static ExpressionToken Visit(MethodCallExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(NewArrayExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(NewExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(ParameterExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(TypeBinaryExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(UnaryExpression exp)
        {
            ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }
        private static ExpressionToken Visit(object expr)
        {
            ExpressionToken returnValue = null;
            //ExpressionToken returnValue = new ExpressionToken(exp);
            return returnValue;
        }*/


        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExpressionToken"/> class.
        /// </summary>
        protected ExpressionToken()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.Write(sb, 0);
            sb.AppendLine("After that ExpressionToken");
            return sb.ToString();
        }

        public abstract void Write(StringBuilder sb, int indent);
        public abstract int Length { get; }
        public abstract bool MultiLine { get; set; }

    }

    /*public sealed class ConstantExpressionToken : ExpressionToken
    {
        private readonly Expression expr;
        public ConstantExpressionToken(Expression expr)
        {
            Guard.NotNull(() => expr, expr);
            this.expr = expr;
        }

        public override void Write(StringBuilder sb, int indent)
        {
            sb.Append(expr.Type.FullName);
            sb.Append(expr.ToString());
        }
    }*/

    internal class LeafExpressionToken : ExpressionToken
    {
        // Fields
        public readonly string Text;

        // Methods
        public LeafExpressionToken(string text)
        {
            this.Text = text;
        }

        public override void Write(StringBuilder sb, int indent)
        {
            sb.Append(this.Text);
        }

        // Properties
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
    }

 

 

}
