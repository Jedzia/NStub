namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using NStub.Core;
    using NStub.CSharp.Tests.Stubs;
    using System.CodeDom;


    public partial class TestObjectComposerTest
    {
        [Test()]
        public void AssignParametersWithNoAssignments()
        {
            var testClass = new CodeTypeDeclaration();
            var creator = new CodeObjectCreateExpression();
            testObject.AssignParameters(testClass, creator);
        }

        [Test()]
        public void AssignParametersNullParametersThrow()
        {
            var testClass = new CodeTypeDeclaration();
            var creator = new CodeObjectCreateExpression();

            Assert.Throws<ArgumentNullException>(() => testObject.AssignParameters(null));
            Assert.Throws<ArgumentNullException>(() => testObject.AssignParameters(null, creator));
            Assert.Throws<ArgumentNullException>(() => testObject.AssignParameters(testClass, null));
        }

        [Test()]
        public void AssignParametersCheckCreationOfFields()
        {
            var testClass = new CodeTypeDeclaration();
            Assert.Throws<InvalidOperationException>(() => testObject.AssignParameters(testClass));

            // standard parameterless ctor, "public ComposeMeCtorVoid()" 
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);
            var actualTestClassFields = testClass.Members.OfType<CodeMemberField>().ToArray();
            Assert.Count(0, actualTestClassFields);


            // with a constructor from "public ComposeMeTwoCtor(string para1, int para2)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            actualTestClassFields = testClass.Members.OfType<CodeMemberField>().ToArray();
            Assert.Count(2, actualTestClassFields);

            var expected = "para1";
            var actual = actualTestClassFields[0].Name;
            Assert.AreEqual(expected, actual);
            var expectedfield = "System.String";
            var actualfield = actualTestClassFields[0].Type.BaseType;
            Assert.AreEqual(expectedfield, actualfield);

            expected = "para2";
            actual = actualTestClassFields[1].Name;
            Assert.AreEqual(expected, actual);
            expectedfield = "System.Int32";
            actualfield = actualTestClassFields[1].Type.BaseType;
            Assert.AreEqual(expectedfield, actualfield);
        }

        [Test()]
        public void AssignParametersCheckSetupMethodStatements()
        {
            var testClass = new CodeTypeDeclaration();
            Assert.Throws<InvalidOperationException>(() => testObject.AssignParameters(testClass));

            // standard parameterless ctor, "public ComposeMeCtorVoid()" 
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);
            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(1, actualSetUpStatements);

            // check the CodeDom for "this.testObject = new ComposeMeCtorVoid();"
            var curStm = actualSetUpStatements[0];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "testObject";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeObjectCreateExpression>(curStm.Right);
            var expectedCreateType = "ComposeMeCtorVoid";
            var actualRight = (CodeObjectCreateExpression)curStm.Right;
            Assert.AreEqual(expectedCreateType, actualRight.CreateType.BaseType);
            Assert.Count(0, actualRight.Parameters);


            // with a constructor from "public ComposeMeTwoCtor(string para1, int para2)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(3, actualSetUpStatements);

            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new ComposeMeTwoCtor(this.para1, this.para2);"
            curStm = actualSetUpStatements[2];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            expected = "testObject"; // OuT initializer.
            actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeObjectCreateExpression>(curStm.Right);
            expectedCreateType = "ComposeMeTwoCtor"; // create new type of OuT.
            actualRight = (CodeObjectCreateExpression)curStm.Right;
            Assert.AreEqual(expectedCreateType, actualRight.CreateType.BaseType);
            Assert.Count(2, actualRight.Parameters); // the two field references in the ctor parameters.
            var actParameter = (CodeFieldReferenceExpression)actualRight.Parameters[0];
            var expectedFielRefName = "para1";
            Assert.AreEqual(expectedFielRefName, actParameter.FieldName);
            actParameter = (CodeFieldReferenceExpression)actualRight.Parameters[1];
            expectedFielRefName = "para2";
            Assert.AreEqual(expectedFielRefName, actParameter.FieldName);

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[0];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            expected = "para1";
            actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            var actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual("Value of para1", actualpRight.Value); // 'this.para1 = "Value of para1"';

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para2 = ....;"
            curStm = actualSetUpStatements[1];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            expected = "para2";
            actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual(1234, actualpRight.Value); // 'this.para2 = 1234';

        }

        [Test()]
        public void AssignParametersCheckSetupMethodStatementsOnInterfaces()
        {
            var testClass = new CodeTypeDeclaration();

            // with a constructor from "public ComposeMeWithInterface(string para1, IComposerWithInterface para2)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeWithInterface));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(2, actualSetUpStatements);

            // Todo: should be the following, but now, interfaces are skipped.
            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new ComposeMeWithInterface(this.para1, this.para2);"
            var curStm = actualSetUpStatements[1];
            CheckObjectUnderTestAssignment("ComposeMeWithInterface", curStm, "para1");

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[0];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "para1";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            var actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual("Value of para1", actualpRight.Value); // 'this.para1 = "Value of para1"';

            // interface is completely ignored, Todo: maybe find a way ....
            /*// first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para2 = ....;"
            curStm = actualSetUpStatements[1];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            expected = "para2";
            actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual(1234, actualpRight.Value); // 'this.para2 = 1234';*/
        }

        [Test()]
        public void AssignParametersCheckSetupMethodStatementsOnIEnumerable()
        {
            var testClass = new CodeTypeDeclaration();

            // with a constructor from "public ComposeMeWithIEnumerable(IEnumerable<ComposeMeWithInterface> para1)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeWithIEnumerable));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(3, actualSetUpStatements);

            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new ComposeMeCtorVoid(this.para1);"
            var curStm = actualSetUpStatements[2];
            CheckObjectUnderTestAssignment("ComposeMeWithIEnumerable", curStm, "para1");
            CheckObjectAssignment("para1Item", "NStub.CSharp.Tests.Stubs.ComposeMeWithInterface", actualSetUpStatements[0]);

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[1];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "para1";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeSnippetExpression>(curStm.Right);
            var actualpRight = (CodeSnippetExpression)curStm.Right;
            Assert.AreEqual("new[] { para1Item }", actualpRight.Value); // 'this.para1 = new[] { para1Item };';
        }

        [Test()]
        public void AssignParametersCheckSetupMethodStatementsOnComposeMeWithList()
        {
            var testClass = new CodeTypeDeclaration();

            // with a constructor from "public ComposeMeWithList(List<Guid> para1)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeWithList));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            Assert.IsNotEmpty(testObject.Assignments.First().First().CreateAssignments);

            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(3, actualSetUpStatements);

            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new[] { para1Item };"
            var curStm = actualSetUpStatements[2];
            CheckObjectUnderTestAssignment("ComposeMeWithList", curStm, "para1");
            CheckObjectAssignment("para1Item", "System.Guid", actualSetUpStatements[0]);

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[1];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "para1";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeSnippetExpression>(curStm.Right);
            var actualpRight = (CodeSnippetExpression)curStm.Right;
            Assert.AreEqual("new[] { para1Item }", actualpRight.Value); // 'this.para1 = new[] { para1Item };';
        }

        [Test()]
        public void AssignParametersCheckSetupMethodStatementsOnComposeMeCtorString()
        {
            var testClass = new CodeTypeDeclaration();

            // with a constructor from "public ComposeMeWithList(List<Guid> para1)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeCtorString));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            // no sub assignments (for a IEnumerable )
            Assert.IsEmpty(testObject.Assignments.First().First().CreateAssignments);

            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(3, actualSetUpStatements);

            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new[] { para1Item };"
            var curStm = actualSetUpStatements[2];
            CheckObjectUnderTestAssignment("ComposeMeCtorString", curStm, "para1", "para2");
            // Todo: check for type of para1 and para2 like: CheckObjectAssignment("para2", "System.Int32", actualSetUpStatements[1]);

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[0];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "para1";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            var actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual("Value of para1", actualpRight.Value);

            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para2 = ....;"
            curStm = actualSetUpStatements[1];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            expected = "para2";
            actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(curStm.Right);
            actualpRight = (CodePrimitiveExpression)curStm.Right;
            Assert.AreEqual(1234, actualpRight.Value);
        }


        [Test()]
        public void AssignParametersCheckSetupMethodStatementsOnComposeMeWithClass()
        {
            var testClass = new CodeTypeDeclaration();

            // with a constructor from "public ComposeMeTwoCtor(string para1, int para2)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeWithClass));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass);

            var actualSetUpStatements = setUpMethod.Statements.OfType<CodeAssignStatement>().ToArray();
            Assert.Count(2, actualSetUpStatements);

            // check that the last Statement is the testObject assignment with
            // the CodeDom for "this.testObject = new ComposeMeCtorVoid(this.para1, this.para2);"
            var curStm = actualSetUpStatements[1];
            CheckObjectUnderTestAssignment("ComposeMeWithClass", curStm, "para1");
            
            // first Statement is the assignment of the first parameter with
            // the CodeDom for "this.para1 = ....;"
            curStm = actualSetUpStatements[0];
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "para1";
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeObjectCreateExpression>(curStm.Right);
            var actualpRight = (CodeObjectCreateExpression)curStm.Right;
            // 'this.para1 = new NStub.CSharp.Tests.Stubs.ComposeMeWithInterface()';
            Assert.AreEqual("NStub.CSharp.Tests.Stubs.ComposeMeWithInterface", actualpRight.CreateType.BaseType);
            Assert.Count(0, actualpRight.Parameters);
        }

        [Test()]
        public void AssignParametersCheckCreatorParameters()
        {
            var testClass = new CodeTypeDeclaration();
            var creator = new CodeObjectCreateExpression();

            // standard parameterless ctor, "public ComposeMeCtorVoid()" 
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass, creator);
            var actualCreatorParameters = creator.Parameters.OfType<CodeFieldReferenceExpression>().ToArray();
            Assert.Count(0, actualCreatorParameters);


            // with a constructor from "public ComposeMeTwoCtor(string para1, int para2)"
            testClass = new CodeTypeDeclaration();
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            testObject.AssignParameters(testClass, creator);

            actualCreatorParameters = creator.Parameters.OfType<CodeFieldReferenceExpression>().ToArray();
            Assert.Count(2, actualCreatorParameters);

            var expected = "para1";
            var actual = actualCreatorParameters[0].FieldName;
            Assert.AreEqual(expected, actual);

            expected = "para2";
            actual = actualCreatorParameters[1].FieldName;
            Assert.AreEqual(expected, actual);
        }
    }
}
