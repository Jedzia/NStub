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
        private void CreateTestObject(Type objtype)
        {
            this.buildData = new BuildDataDictionary();
            this.setUpMethod = new CodeMemberMethod() { Name = "SetUp" };
            this.testObjectMemberField = new CodeMemberField(objtype.FullName, "testObject");
            this.testObjectName = objtype.Name;// "testObjectField";
            this.testObjectType = objtype;
            this.testObject = new TestObjectComposer(
                this.buildData,
                this.setUpMethod,
                this.testObjectMemberField,
                this.testObjectName,
                this.testObjectType);
        }

        private static CodeObjectCreateExpression CheckObjectUnderTestAssignment(
            string typeOfOuT, 
            CodeAssignStatement curStm,
            params string[] fieldnames)
        {
            Assert.IsInstanceOfType<CodeFieldReferenceExpression>(curStm.Left);
            var expected = "testObject"; // OuT initializer.
            var actualLeft = (CodeFieldReferenceExpression)curStm.Left;
            Assert.AreEqual(expected, actualLeft.FieldName);
            Assert.IsInstanceOfType<CodeObjectCreateExpression>(curStm.Right);
            var expectedCreateType = typeOfOuT; // create new type of OuT.
            var actualRight = (CodeObjectCreateExpression)curStm.Right;
            Assert.AreEqual(expectedCreateType, actualRight.CreateType.BaseType);

            // check the field references in the ctor parameters.
            if (fieldnames == null)
            {
                Assert.Count(0, actualRight.Parameters);
            }
            else
            {
                Assert.Count(fieldnames.Length, actualRight.Parameters);
                for (int i = 0; i < actualRight.Parameters.Count; i++)
                {
                    var actParameter = (CodeFieldReferenceExpression)actualRight.Parameters[i];
                    var expectedFielRefName = fieldnames[i];
                    Assert.AreEqual(expectedFielRefName, actParameter.FieldName);
                }
            }
            return actualRight;
        }

        private static void CheckCtorAsgn<T>(AssignmentInfoCollection preffered, string name, bool hasCreationAssignments)
        {
            Type memberType = typeof(T);
            Assert.IsNotNull(preffered[name].AssignStatement);
            Assert.IsNotNull(preffered[name].CreateAssignments);
            Assert.AreEqual(hasCreationAssignments, preffered[name].HasCreationAssignments);
            Assert.IsNotNull(preffered[name].MemberField);
            Assert.AreEqual(name, preffered[name].MemberFieldName);
            Assert.AreEqual(memberType, preffered[name].MemberType);
            Assert.AreEqual(name, preffered[name].ParameterName);
        }

    }
}
