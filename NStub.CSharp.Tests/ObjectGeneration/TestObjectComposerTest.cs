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
    
    
    [TestFixture()]
    public partial class TestObjectComposerTest
    {
        private NStub.CSharp.ObjectGeneration.BuildDataDictionary buildData;
        private System.CodeDom.CodeMemberMethod setUpMethod;
        private TestObjectComposer testObject;
        private System.CodeDom.CodeMemberField testObjectMemberField;
        private string testObjectName;
        private System.Type testObjectType;
        
        [SetUp()]
        public void SetUp()
        {
            CreateTestObject(typeof(ComposeMeCtorVoid));
        }

        private void CreateTestObject(Type objtype)
        {
            this.buildData = new BuildDataDictionary();
            this.setUpMethod = new CodeMemberMethod() { Name="SetUp" };
            this.testObjectMemberField = new CodeMemberField(objtype.FullName, "testObject");
            this.testObjectName = objtype.Name;// "testObjectField";
            this.testObjectType = objtype;
            this.testObject = new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void ConstructWithParametersBuildDataSetUpMethodTestObjectMemberFieldTestObjectNameTestObjectTypeTest()
        {
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataDictionary();
            this.setUpMethod = new System.CodeDom.CodeMemberMethod();
            this.testObjectMemberField = new System.CodeDom.CodeMemberField();
            this.testObjectName = "Value of testObjectName";
            this.testObjectType = typeof(object);
            this.testObject = new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType);

            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(null, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, null, this.testObjectMemberField, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, null, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, null, this.testObjectType));
            Assert.Throws<ArgumentException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, string.Empty, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, null));
        }
        
        [Test()]
        public void PropertyHasParameterAssignmentsNormalBehavior()
        {
            // Test read access of 'HasParameterAssignments' Property.
            var expected = false;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            testObject.BuildTestObject(MemberVisibility.Public);
            expected = true;
            actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyIsCompletelyAssignedNormalBehavior()
        {
            // Test read access of 'IsCompletelyAssigned' Property.
            var expected = true;
            var actual = testObject.IsCompletelyAssigned;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void AssignExtraTest()
        {
            // TODO: Implement unit test for AssignExtra

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AssignOnlyTest()
        {
            // TODO: Implement unit test for AssignOnly

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AssignParametersTest()
        {
            // TODO: Implement unit test for AssignParameters

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
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
        public void AssignmentListsAreSameAtTheBase()
        {
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            Assert.AreSame(testObject.Assignments, testObject.CtorAssignments);
            Assert.IsInstanceOfType<IEnumerable<AssignmentInfoCollection>>(testObject.Assignments);
            Assert.IsInstanceOfType<ConstructorAssignmentCollection>(testObject.CtorAssignments);
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

        [Test()]
        public void PropertyAssignmentsNormalBehavior()
        {
            // Test read access of 'Assignments' Property.
            var actual = testObject.Assignments;
            Assert.IsNull(actual);
            
            testObject.BuildTestObject(MemberVisibility.Public);
            actual = testObject.Assignments;
            Assert.IsNotEmpty(actual);
        }
        
        [Test()]
        public void PropertyBuildDataNormalBehavior()
        {
            // Test read access of 'BuildData' Property.
            var expected = this.buildData;
            var actual = testObject.BuildData;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void PropertyTestObjectMemberFieldCreateExpressionNormalBehavior()
        {
            // Test read access of 'TestObjectMemberFieldCreateExpression' Property.
            var actual = testObject.TestObjectMemberFieldCreateExpression;
            Assert.IsNull(actual);

            testObject.BuildTestObject(MemberVisibility.Public);
            actual = testObject.TestObjectMemberFieldCreateExpression;
            Assert.IsNotNull(actual);
        }
        
        [Test()]
        public void PropertyTestObjectMemberFieldNormalBehavior()
        {
            // Test read access of 'TestObjectMemberField' Property.
            var expected = this.testObjectMemberField;
            var actual = testObject.TestObjectMemberField;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void PropertyTestObjectNameNormalBehavior()
        {
            // Test read access of 'TestObjectName' Property.
            var expected = this.testObjectName;
            var actual = testObject.TestObjectName;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void PropertyTestObjectTypeNormalBehavior()
        {
            // Test read access of 'TestObjectType' Property.
            var expected = this.testObjectType;
            var actual = testObject.TestObjectType;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void PropertySetUpMethodNormalBehavior()
        {
            // Test read access of 'SetUpMethod' Property.
            var expected = this.setUpMethod;
            var actual = testObject.SetUpMethod;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void TryFindConstructorAssignmentTest()
        {
            CreateTestObject(typeof(ComposeMeCtorString));
            testObject.BuildTestObject(MemberVisibility.Public);

            ConstructorAssignment ctorAssignment;
            var found = testObject.TryFindConstructorAssignment("para2", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("para1", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("pArA2", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("pArA2", out ctorAssignment, true);
            Assert.IsFalse(found);
            Assert.IsNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("NoOneLoveMe", out ctorAssignment, false);
            Assert.IsFalse(found);
            Assert.IsNull(ctorAssignment);

            Assert.Throws<ArgumentNullException>(() => testObject.TryFindConstructorAssignment(null, out ctorAssignment, false));
            Assert.Throws<ArgumentException>(() => testObject.TryFindConstructorAssignment(string.Empty, out ctorAssignment, false));
        }
    }
}
