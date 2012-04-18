namespace NStub.CSharp.Tests.ObjectGeneration
{
    using global::MbUnit.Framework;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System;
    using System.Linq;
    using NStub.CSharp.Tests.FluentChecking;

    public partial class CodeFieldReferenceBinderTest
    {

        private CodeFieldReferenceBinder testObject;
        private CodeTypeDeclaration declaration;
        private CodeMemberMethod method;
        
        [SetUp()]
        public void SetUp()
        {
            this.declaration = new CodeTypeDeclaration("MyClass");
            this.method = new CodeMemberMethod();
            this.testObject = this.method.Assign("myField");
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.declaration = null;
            this.testObject = null;
            this.method = null;
        }
        
        [Test()]
        public void PropertyFieldAssignmentNormalBehavior()
        {
            // Test read access of 'FieldAssignment' Property.
            var actual = testObject.FieldAssignment;
            Assert.IsNull(actual);

            testObject.With(123);
            actual = testObject.FieldAssignment;
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void AndCreateInGenericTest()
        {
            var ctd = new CodeTypeDeclaration("MyTestClassTest");
            var fieldType = typeof(IComparable);
            var actual = testObject.AndCreateIn<IComparable>(ctd);
            Assert.AreSame(testObject, actual);
            Assert.IsNotEmpty(ctd.Members.OfType<CodeMemberField>().Where(e => e.Name == "myField"));
            Assert.IsNotEmpty(ctd.Members.OfType<CodeMemberField>().Where(e => e.Type.BaseType == fieldType.FullName));
            var field = ctd.Members.OfType<CodeMemberField>().Where(e => e.Name == "myField").First();
            Assert.IsNull(field.InitExpression);
        }

        [Test()]
        public void AndCreateInTest()
        {
            var ctd = new CodeTypeDeclaration("MyTestClassTest");
            var fieldType = typeof(Guid);
            var actual = testObject.AndCreateIn(ctd, fieldType);
            Assert.AreSame(testObject, actual);
            Assert.IsNotEmpty(ctd.Members.OfType<CodeMemberField>().Where(e => e.Name == "myField"));
            Assert.IsNotEmpty(ctd.Members.OfType<CodeMemberField>().Where(e => e.Type.BaseType == fieldType.FullName));
            var field = ctd.Members.OfType<CodeMemberField>().Where(e => e.Name == "myField").First();
            Assert.IsNull(field.InitExpression);
        }

        [Test()]
        public void AndCreateInTwoTimesShouldThrow()
        {
            var ctd = new CodeTypeDeclaration("MyTestClassTest");
            var fieldType = typeof(Guid);
            var actual = testObject.AndCreateIn(ctd, fieldType);
            Assert.Throws<CodeFieldReferenceException>(() => testObject.AndCreateIn(ctd, fieldType));
            Assert.AreEqual(1, ctd.Members.Count);
        }


        [Test()]
        public void CommitWithNoAssignmentShouldThrow()
        {
            Assert.Throws<CodeFieldReferenceException>(() => testObject.Commit());
            Assert.IsEmpty(method.Statements);
        }

        [Test()]
        public void CommitTest()
        {
            var expectedPrimitive = 55.55d;
            var result = testObject.With(expectedPrimitive).Commit();
            Assert.AreSame(method, result);
            AssertEx.That(method.StatementsOfType<CodeAssignStatement>().Where()
                .ExpressionLeft<CodeFieldReferenceExpression>(Is.FieldNamed("myField")).WasFound(1)
                .ExpressionRight<CodePrimitiveExpression>(Is.Primitve(expectedPrimitive)).WasFound(1)
                .Assert());
        }
        
        [Test()]
        public void WithTest()
        {
            var actual = testObject.With("assignment text");
            var fieldAssignment = testObject.FieldAssignment;
            Assert.AreSame(testObject, actual);
            Assert.IsNotNull(fieldAssignment);

        }
    }
}
