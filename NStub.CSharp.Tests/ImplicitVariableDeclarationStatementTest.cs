namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    
    
    [TestFixture()]
    public partial class ImplicitVariableDeclarationStatementTest
    {
        [Test()]
        public void ConstructWithParametersNameInitExpressionTest()
        {
            var name = "Value of name";
            var initExpression = new System.CodeDom.CodeExpression();
            var testObject = new ImplicitVariableDeclarationStatement(name, initExpression);
            
            new ImplicitVariableDeclarationStatement(null, initExpression);
            new ImplicitVariableDeclarationStatement(name, null);
        }
    }
}
