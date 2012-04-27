using System;
using System.Collections.Generic;
using System.Linq;
using global::MbUnit.Framework;
using NStub.CSharp.ObjectGeneration;
using NStub.Core;
using NStub.CSharp.Tests.Stubs;
using System.CodeDom;

namespace NStub.CSharp.Tests.ObjectGeneration
{
    public partial class TestObjectComposerTest
    {
        [Test()]
        public void BuildTestObjectEmptyDefault()
        {
            testObject.BuildTestObject(MemberVisibility.Public);
            var expected = true;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            // all assignments.
            var expAssignments = 1; // 1 assign lists, the std ctor
            var actAssignments = testObject.Assignments;
            Assert.Count(expAssignments, actAssignments);
            var allCtors = testObject.Assignments.SelectMany((e) => e);
            Assert.IsEmpty(allCtors); // that are empty

            // constructor assignments.
            var expCtorAssignments = 1; // 1 assign lists
            var actCtorAssignments = testObject.CtorAssignments;
            Assert.Count(expCtorAssignments, actCtorAssignments);
            allCtors = actCtorAssignments.SelectMany((e) => e);
            Assert.IsEmpty(allCtors); // that are empty
            Assert.AreSame(testObject.CtorAssignments.PreferredConstructor, actCtorAssignments.First());

            // CodeDom tree.
            Assert.Count(1, testObject.SetUpMethod.Statements);
            Assert.IsInstanceOfType<CodeAssignStatement>(testObject.SetUpMethod.Statements[0]);
            var asignStm = (CodeAssignStatement)testObject.SetUpMethod.Statements[0];
            Assert.AreEqual(this.testObjectMemberField.Name, ((CodeFieldReferenceExpression)asignStm.Left).FieldName);
            Assert.AreEqual(this.testObjectType.Name, ((CodeObjectCreateExpression)asignStm.Right).CreateType.BaseType);
        }

        [Test()]
        public void BuildTestObjectComposeMeTwoCtor()
        {
            CreateTestObject(typeof(ComposeMeCtorString));
            testObject.BuildTestObject(MemberVisibility.Public);
            var expected = true;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            // all assignments.
            var expAssignments = 1; // 1 assign lists
            var actAssignments = testObject.Assignments;
            Assert.Count(expAssignments, actAssignments);
            var allCtorParameters = testObject.Assignments.SelectMany((e) => e);
            Assert.Count(2, allCtorParameters);

            // constructor assignments.
            var expectedConstructors = 1; // 1 assign lists
            var actualConstructors = testObject.CtorAssignments;
            Assert.Count(expectedConstructors, actualConstructors);
            allCtorParameters = actualConstructors.SelectMany((e) => e);
            Assert.Count(2, allCtorParameters);
            var preffered = testObject.CtorAssignments.PreferredConstructor;
            Assert.AreSame(actualConstructors.First(), preffered); // first and only = the preffered
            Assert.Count(2, preffered);
            Assert.IsNotNull(preffered["para1"]);
            Assert.IsNotNull(preffered["para2"]);

            CheckCtorAsgn<string>(preffered, "para1", false);
            CheckCtorAsgn<int>(preffered, "para2", false);

            var first = preffered.First();


            // public ComposeMeCtorString(string para1)
            Assert.AreEqual(typeof(ComposeMeCtorString), this.testObjectType);

            // CodeDom tree.
            Assert.Count(1, testObject.SetUpMethod.Statements);
            Assert.IsInstanceOfType<CodeAssignStatement>(testObject.SetUpMethod.Statements[0]);
            var asignStm = (CodeAssignStatement)testObject.SetUpMethod.Statements[0];
            Assert.AreEqual(this.testObjectMemberField.Name, ((CodeFieldReferenceExpression)asignStm.Left).FieldName);
            Assert.AreEqual(this.testObjectType.Name, ((CodeObjectCreateExpression)asignStm.Right).CreateType.BaseType);


        }

        [Test()]
        public void BuildTestObjectComposeMeCtorString()
        {
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            var expected = true;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            // all assignments.
            var expAssignments = 2; // 2 constructors
            var actAssignments = testObject.Assignments;
            Assert.Count(expAssignments, actAssignments);
            var allCtorParameters = testObject.Assignments.SelectMany((e) => e);
            Assert.Count(3, allCtorParameters);

            // constructor assignments.
            var expectedConstructors = 2; // 2 constructors
            var actualConstructors = testObject.CtorAssignments;
            Assert.Count(expectedConstructors, actualConstructors);
            allCtorParameters = actualConstructors.SelectMany((e) => e);
            Assert.Count(3, allCtorParameters);
            var preffered = testObject.CtorAssignments.PreferredConstructor;
            Assert.AreSame(actualConstructors.First(), preffered); // first and only = the preffered
            Assert.Count(2, preffered);
            Assert.IsNotNull(preffered["para1"]);
            Assert.IsNotNull(preffered["para2"]);

            CheckCtorAsgn<string>(preffered, "para1", false);
            CheckCtorAsgn<int>(preffered, "para2", false);

            var first = preffered.First();

            var assignments = testObject.Assignments.ToArray();
            Assert.AreSame(assignments[0], preffered);
            Assert.AreNotSame(assignments[1], preffered);
            Assert.AreNotEqual(assignments[1], preffered);

            Assert.Count(1, assignments[1]);
            CheckCtorAsgn<bool>(assignments[1], "para1", false);

            // public ComposeMeCtorString(string para1)
            Assert.AreEqual(typeof(ComposeMeTwoCtor), this.testObjectType);

            // CodeDom tree.
            Assert.Count(1, testObject.SetUpMethod.Statements);
            Assert.IsInstanceOfType<CodeAssignStatement>(testObject.SetUpMethod.Statements[0]);
            var asignStm = (CodeAssignStatement)testObject.SetUpMethod.Statements[0];
            Assert.AreEqual(this.testObjectMemberField.Name, ((CodeFieldReferenceExpression)asignStm.Left).FieldName);
            Assert.AreEqual(this.testObjectType.Name, ((CodeObjectCreateExpression)asignStm.Right).CreateType.BaseType);


        }

        [Test()]
        public void BuildTestObjectComposeMeThreeCtor()
        {
            CreateTestObject(typeof(ComposeMeThreeCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            var expected = true;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            // all assignments.
            var expAssignments = 3; // 2 constructors
            var actAssignments = testObject.Assignments;
            Assert.Count(expAssignments, actAssignments);
            var allCtorParameters = testObject.Assignments.SelectMany((e) => e);
            Assert.Count(3, allCtorParameters);

            // constructor assignments.
            var expectedConstructors = 3; // 2 constructors
            var actualConstructors = testObject.CtorAssignments;
            Assert.Count(expectedConstructors, actualConstructors);
            allCtorParameters = actualConstructors.SelectMany((e) => e);
            Assert.Count(3, allCtorParameters);
            var preffered = testObject.CtorAssignments.PreferredConstructor;
            Assert.AreSame(actualConstructors.First(), preffered); // first and only = the preffered
            Assert.Count(2, preffered);
            Assert.IsNotNull(preffered["para1"]);
            Assert.IsNotNull(preffered["para2"]);

            CheckCtorAsgn<string>(preffered, "para1", false);
            CheckCtorAsgn<int>(preffered, "para2", false);

            var first = preffered.First();

            var assignments = testObject.Assignments.ToArray();
            Assert.AreSame(assignments[0], preffered);
            Assert.AreNotSame(assignments[1], preffered);
            Assert.AreNotEqual(assignments[1], preffered);

            Assert.Count(2, assignments[0]); // public ComposeMeThreeCtor(string para1, int para2)
            Assert.Count(0, assignments[1]); // public ComposeMeThreeCtor()
            Assert.Count(1, assignments[2]); // public ComposeMeThreeCtor(bool para1)

            CheckCtorAsgn<string>(assignments[0], "para1", false);
            CheckCtorAsgn<int>(assignments[0], "para2", false);

            CheckCtorAsgn<bool>(assignments[2], "para1", false);

            // public ComposeMeCtorString(string para1)
            Assert.AreEqual(typeof(ComposeMeThreeCtor), this.testObjectType);

            // CodeDom tree.
            Assert.Count(1, testObject.SetUpMethod.Statements);
            Assert.IsInstanceOfType<CodeAssignStatement>(testObject.SetUpMethod.Statements[0]);
            var asignStm = (CodeAssignStatement)testObject.SetUpMethod.Statements[0];
            Assert.AreEqual(this.testObjectMemberField.Name, ((CodeFieldReferenceExpression)asignStm.Left).FieldName);
            Assert.AreEqual(this.testObjectType.Name, ((CodeObjectCreateExpression)asignStm.Right).CreateType.BaseType);
        }

        [Test()]
        public void BuildTestObjectComposeMeVisibility()
        {
            CreateTestObject(typeof(ComposeMeVisibility));
            testObject.BuildTestObject(MemberVisibility.Public);
            var assignments = testObject.Assignments.ToArray();
            Assert.Count(1, assignments);

            Assert.Count(1, assignments[0]); // public ComposeMeThreeCtor(string para1, int para2)
            //Assert.Count(0, assignments[1]); // public ComposeMeThreeCtor()
            //Assert.Count(1, assignments[2]); // public ComposeMeThreeCtor(bool para1)
            CheckCtorAsgn<System.DateTime>(assignments[0], "para1", false);

            CreateTestObject(typeof(ComposeMeVisibility));
            testObject.BuildTestObject(MemberVisibility.Internal);
            assignments = testObject.Assignments.ToArray();
            Assert.Count(3, assignments);

            // protected ComposeMeVisibility(bool para1, string para2, int para3)
            CheckCtorAsgn<bool>(assignments[0], "para1", false);
            CheckCtorAsgn<string>(assignments[0], "para2", false);
            CheckCtorAsgn<int>(assignments[0], "para3", false);

            // internal ComposeMeVisibility(int para1, bool para2)
            CheckCtorAsgn<int>(assignments[1], "para1", false);
            CheckCtorAsgn<bool>(assignments[1], "para2", false);

            // public ComposeMeVisibility(DateTime para1)
            CheckCtorAsgn<DateTime>(assignments[2], "para1", false);

            CreateTestObject(typeof(ComposeMeVisibility));
            testObject.BuildTestObject(MemberVisibility.Private);
            assignments = testObject.Assignments.ToArray();
            Assert.Count(4, assignments);

            // private ComposeMeVisibility(string para1, string para2, int para3, double para4)
            CheckCtorAsgn<string>(assignments[0], "para1", false);
            CheckCtorAsgn<string>(assignments[0], "para2", false);
            CheckCtorAsgn<int>(assignments[0], "para3", false);
            CheckCtorAsgn<double>(assignments[0], "para4", false);

            // protected ComposeMeVisibility(bool para1, string para2, int para3)
            CheckCtorAsgn<bool>(assignments[1], "para1", false);
            CheckCtorAsgn<string>(assignments[1], "para2", false);
            CheckCtorAsgn<int>(assignments[1], "para3", false);

            // internal ComposeMeVisibility(int para1, bool para2)
            CheckCtorAsgn<int>(assignments[2], "para1", false);
            CheckCtorAsgn<bool>(assignments[2], "para2", false);

            // public ComposeMeVisibility(DateTime para1)
            CheckCtorAsgn<DateTime>(assignments[3], "para1", false);
        }
    }
}
