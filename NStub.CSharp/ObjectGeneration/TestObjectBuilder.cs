// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestObjectBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using NStub.Core;

    /// <summary>
    /// Used to create new test member fields with the matching initialization for them.
    /// </summary>
    [Serializable]
    internal class TestObjectComposer : TestObjectComposerBase, IEquatable<TestObjectComposer>
    {
        /*/// <summary>
        /// Initializes a new instance of the <see cref="TestObjectCreator"/> class.
        /// </summary>
        internal TestObjectCreator() { }*/
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestObjectComposer"/> class.
        /// </summary>
        /// <param name="buildData">The build data dictionary.</param>
        /// <param name="setUpMethod">The SetUpMethod</param>
        /// <param name="testObjectMemberField">The TestObjectMemberField</param>
        /// <param name="testObjectName">The TestObjectName</param>
        /// <param name="testObjectType">Type of the test object.</param>
        internal TestObjectComposer(
            BuildDataDictionary buildData,
            CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName,
            Type testObjectType)
            : base(buildData, setUpMethod, testObjectMemberField, testObjectName, testObjectType)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has ready initialized parameter assignments.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has parameter assignments; otherwise, <c>false</c>.
        /// </value>
        public bool HasParameterAssignments
        {
            get
            {
                return CtorAssignments != null && CtorAssignments.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance constructor parameters are completely assigned.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is completely assigned; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompletelyAssigned
        {
            get
            {
                if (!this.HasParameterAssignments)
                {
                    return true;
                }

                var ctorparameters = CtorAssignments.PreferredConstructor.UsedConstructor.GetParameters();
                foreach (var para in ctorparameters)
                {
                    var assignment = CtorAssignments.PreferredConstructor[para.Name];
                    if (assignment == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        #endregion

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to an explicitly specified constructor
        /// create expression to a specified method.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testMethod">The test method, to add the assign-statements to.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        /// <param name="ctorAssignments">The list of constructor assignments that specify the parameter to add.</param>
        public override void AssignExtra(
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod testMethod,
            CodeObjectCreateExpression testObjectConstructor,
            AssignmentInfoCollection ctorAssignments)
        {
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => testObjectConstructor, testObjectConstructor);

            // Guard.NotNull(() => this.assignments, this.assignments);
            if (testClassDeclaration.Name.StartsWith("Jedzia.SamCat.Model.Tasks.TaskComposer"))
            {
            }

            if (this.HasParameterAssignments)
            {
                var testObjectInitializerPosition = testMethod.Statements.Count - 1;

                var ctorparameters = ctorAssignments.UsedConstructor.GetParameters();
                if (ctorparameters.Length > 1)
                {
                }

                foreach (var para in ctorparameters)
                {
                    var assignment = ctorAssignments[para.Name];
                    if (assignment == null)
                    {
                        continue;
                    }

                    // Add the member field to the test class.
                        testClassDeclaration.Members.Add(assignment.MemberField);

                        // this.BuildData.AddDataItem("Setup", assignment.MemberField.Name, new BuilderData<CodeMemberField>(assignment.MemberField));
                        //  BuildData.AddDataItem(
                        //    "Assignments.Extra." + testMethod.Name + "." + testClassDeclaration.Name, assignment.MemberField.Name, new BuilderData<CodeMemberField>(assignment.MemberField));

                        // Add a local variable for the constructor parameter.
                        testMethod.Statements.Add(assignment.AssignStatement);

                    // Add the local variable to the constructor initializer in the object create expression 
                    // (e.g. SetUp method, test object constructor) of the specified method.
                    testObjectConstructor.Parameters.Add(assignment.AssignStatement.Left);
                }

                // reorder the testObject initializer to the bottom of the SetUp method.
                this.ReorderSetupStatement(testMethod, testObjectInitializerPosition);
            }
        }

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to an explicitly specified constructor
        /// create expression to a specified method.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testMethod">The test method, to add the assign-statements to.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        /// <param name="ctorAssignments">The list of constructor assignments that specify the parameter to add.</param>
        public override void AssignOnly(
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod testMethod,
            CodeObjectCreateExpression testObjectConstructor,
            AssignmentInfoCollection ctorAssignments)
        {
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => testObjectConstructor, testObjectConstructor);

            // Guard.NotNull(() => this.assignments, this.assignments);
            if (testClassDeclaration.Name.StartsWith("Jedzia.SamCat.Model.Tasks.TaskComposer"))
            {
            }

            if (this.HasParameterAssignments)
            {
                var testObjectInitializerPosition = testMethod.Statements.Count - 1;

                var ctorparameters = ctorAssignments.UsedConstructor.GetParameters();
                if (ctorparameters.Length > 1)
                {
                }

                foreach (var para in ctorparameters)
                {
                    var assignment = ctorAssignments[para.Name];
                    if (assignment == null)
                    {
                        continue;
                    }

                    // Add the member field to the test class.
                    /*if (false)
                    {
                        testClassDeclaration.Members.Add(assignment.MemberField);

                        // this.BuildData.AddDataItem("Setup", assignment.MemberField.Name, new BuilderData<CodeMemberField>(assignment.MemberField));
                        //  BuildData.AddDataItem(
                        //    "Assignments.Extra." + testMethod.Name + "." + testClassDeclaration.Name, assignment.MemberField.Name, new BuilderData<CodeMemberField>(assignment.MemberField));

                        // Add a local variable for the constructor parameter.
                        testMethod.Statements.Add(assignment.AssignStatement);
                    }*/

                    // Add the local variable to the constructor initializer in the object create expression 
                    // (e.g. SetUp method, test object constructor) of the specified method.
                    testObjectConstructor.Parameters.Add(assignment.AssignStatement.Left);
                }

                // reorder the testObject initializer to the bottom of the SetUp method.
                this.ReorderSetupStatement(testMethod, testObjectInitializerPosition);
            }
        }

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        public override void AssignParameters(
            CodeTypeDeclaration testClassDeclaration, CodeObjectCreateExpression testObjectConstructor)
        {
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => testObjectConstructor, testObjectConstructor);

            // Guard.NotNull(() => this.assignments, this.assignments);

            if (this.HasParameterAssignments)
            {
                var testObjectInitializerPosition = SetUpMethod.Statements.Count - 1;

                var ctorparameters = CtorAssignments.PreferredConstructor.UsedConstructor.GetParameters();
                foreach (var para in ctorparameters)
                {
                    var assignment = CtorAssignments.PreferredConstructor[para.Name];
                    if (assignment == null)
                    {
                        continue;
                    }

                    // Add the member field to the test class.
                    testClassDeclaration.Members.Add(assignment.MemberField);
                    BuildData.AddDataItem(
                        "Assignments." + testClassDeclaration.Name, 
                        assignment.MemberField.Name, 
                        new BuilderData<ConstructorAssignment>(assignment));


                    // if there a sub assignments to this parameter, add them
                    if (assignment.HasCreationAssignments)
                    {
                        if (this.TestObjectMemberField.Type.BaseType == "DefaultMemberBuilderFactory")
                        {
                        }

                        foreach (var creationAssignment in assignment.CreateAssignments)
                        {
                            testClassDeclaration.Members.Add(creationAssignment.MemberField);
                            AddAssignStatement(creationAssignment.AssignStatement);
                            var assignCreateExpr = assignment.AssignStatement.Right as CodeObjectCreateExpression;
                            if (assignCreateExpr != null)
                            {
                                assignCreateExpr.Parameters.Add(creationAssignment.AssignStatement.Left);
                            }
                            BuildData.AddDataItem(
                                "CreateAssignments." + this.TestObjectType.FullName + "." +
                                assignment.MemberField.Name, creationAssignment.MemberField.Name,
                                new BuilderData<ConstructorAssignment>(creationAssignment));
                        }
                    }


                    // Add a local variable for the constructor parameter.
                    AddAssignStatement(assignment.AssignStatement);

                    // Add the local variable to the constructor initializer in the object create expression 
                    // (e.g. SetUp method, test object constructor) of the specified method.
                    testObjectConstructor.Parameters.Add(assignment.AssignStatement.Left);

                }

                // reorder the testObject initializer to the bottom of the SetUp method.
                this.ReorderSetupStatement(SetUpMethod, testObjectInitializerPosition);
            }
        }

        /// <summary>
        /// Creates a code generation expression for an object to test with a member field and initialization
        /// in the constructor specified <see cref="TestObjectComposerBase.SetUpMethod"/> method.
        /// </summary>
        /// <param name="visibility">The visibility level of the objects to parse. Default should be public.</param>
        /// <returns>
        /// The initialization expression of the object under test.
        /// </returns>
        public override CodeObjectCreateExpression BuildTestObject(MemberVisibility visibility)
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), TestObjectMemberField.Name);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(TestObjectName, new CodeExpression[] { });
            TestObjectMemberFieldCreateExpression = testObjectMemberFieldCreate;
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);

            var flags = BindingFlags.Instance;
            switch (visibility)
            {
                case MemberVisibility.Public:
                    flags |= BindingFlags.Public;
                    break;
                case MemberVisibility.Internal:
                    flags |= BindingFlags.Public | BindingFlags.NonPublic;
                    break;
                case MemberVisibility.Private:
                    flags |= BindingFlags.Public | BindingFlags.NonPublic;
                    break;
                default:
                    break;
            }

            CtorAssignments = this.AddParametersToConstructor(flags);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            AddAssignStatement(as1);

            return testObjectMemberFieldCreate;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is <c>null</c>.</exception>
        public override bool Equals(object obj)
        {
            var other = obj as TestObjectComposer;
            return other != null && this.Equals(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TestObjectComposer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                SetUpMethod == other.SetUpMethod &&
                TestObjectMemberField == other.TestObjectMemberField &&
                TestObjectName == other.TestObjectName;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return CtorAssignments != null ? CtorAssignments.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("SetUpMethod = " + SetUpMethod + ";");
            sb.Append("TestObjectMemberField = " + TestObjectMemberField + ";");
            sb.Append("TestObjectName = " + TestObjectName);
            return sb.ToString();
        }

        /// <summary>
        /// Tries to find a matching constructor assignment for a parameter.
        /// </summary>
        /// <param name="parameter">The name of the parameter to find the constructor assignment for.</param>
        /// <param name="constructorAssignment">The found constructor assignment. Is <c>null</c> when none is found.</param>
        /// <param name="caseSensitive">if set to <c>true</c> if case sensitive parameter name matching should be used.</param>
        /// <returns>
        /// <c>true</c> if a matching constructor assignment was found; otherwise <c>false</c>.
        /// </returns>
        public bool TryFindConstructorAssignment(
            string parameter, out ConstructorAssignment constructorAssignment, bool caseSensitive)
        {
            Guard.NotNullOrEmpty(() => parameter, parameter);

            // Guard.NotNull(() => this.assignments, this.assignments);
            if (CtorAssignments == null)
            {
                constructorAssignment = null;
                return false;
            }

            Func<string, string, bool> comparer;
            if (caseSensitive)
            {
                comparer = (x, y) => x == y;
            }
            else
            {
                comparer = (x, y) => x.ToLower() == y.ToLower();
            }

            foreach (var ctorAssignments in CtorAssignments)
            {
                foreach (var assignment in ctorAssignments)
                {
                    if (comparer(assignment.ParameterName, parameter))
                    {
                        constructorAssignment = assignment;
                        return true;
                    }
                }
            }

            constructorAssignment = null;
            return false;
        }

        /*
                private CodeAssignStatement AddMockObject(
                    CodeMemberMethod setUpMethod,
                    CodeMemberField mockRepositoryMemberField,
                    string testObjectName,
                    ParameterInfo paraInfo,
                    string paraName)
                {
                    var paraType = paraInfo.ParameterType;

                    var mockRef =
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), mockRepositoryMemberField.Name);

                    var fieldRef1 =
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), paraName);

                    // Creates a statement using a code expression.
                    // var expressionStatement = new CodeExpressionStatement(fieldRef1);

                    // Creates a code expression for a CodeExpressionStatement to contain.
                    var invokeExpression = new CodeMethodInvokeExpression(mockRef, "StrictMock");
                    invokeExpression.Method.TypeArguments.Add(paraType.FullName);

                    // Creates a statement using a code expression.
                    // var expressionStatement = new CodeExpressionStatement(invokeExpression);
                    // var objectCreate1 = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
                    var as1 = new CodeAssignStatement(fieldRef1, invokeExpression);
                    setUpMethod.Statements.Add(as1);

                    // var testObject = TestMemberFieldLookup["testObject"];
                    // setUpMethod.Statements.Add(as1);
                    return as1;
                }
        */


        /// <summary>
        /// Add the parameter assignments to the specified constructor expression.
        /// </summary>
        /// <param name="bindingAttr">A bitmask comprised of one or more System.Reflection.BindingFlags
        /// that specify how the search is conducted.  -or- Zero, to return null.</param>
        /// <returns>
        /// A structure with data about parameter initialization for the type of test object of this instance.
        /// </returns>
        /// <exception cref="InvalidOperationException">No preferred constructor found, but there should one.</exception>
        private ConstructorAssignmentCollection AddParametersToConstructor(BindingFlags bindingAttr)
        {
            // Type[] parameters = { /*typeof(int)*/ };

            /*// Get the constructor that takes an integer as a parameter.
            var ctor = this.TestObjectType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                Type.DefaultBinder,
                parameters,
                null);

            if (ctor == null)
            {
                // outputBlock.Text += 
                // "There is no public constructor of MyClass that takes an integer as a parameter.\n";
            }
            else
            {
                // there's only a default ctor, return.
                return assignments;
            }*/

            // else
            // {
            // outputBlock.Text += 
            // "The public constructor of MyClass that takes an integer as a parameter is:\n"; 
            // outputBlock.Text += ctor.ToString() + "\n";
            // }
            var testObjectConstructors = TestObjectType.GetConstructors(bindingAttr);

            // var ctorParameterTypes = new List<ParameterInfo>();
            if (testObjectConstructors.Length < 1)
            {
                return null;
            }

            // for now, use to ctor with the most parameters.
            // foreach testObjectConstructors
            // var most = testObjectConstructors.Max(e => e.GetParameters().Length);
            // var constructor = testObjectConstructors.Where(e => e.GetParameters().Length == most).First();
            var tempAssignments = new List<AssignmentInfoCollection>();
            int most = -1;
            AssignmentInfoCollection mostAssignmentInfoCollection = null;
            foreach (var constructor in testObjectConstructors)
            {
                if (constructor.IsPrivate)
                {
                    continue;
                }
                var assignmentInfoCollection = this.BuildAssignmentInfoForConstructor(bindingAttr, constructor);
                int parameterAmount = constructor.GetParameters().Length;
                if (parameterAmount > most)
                {
                    most = parameterAmount;
                    mostAssignmentInfoCollection = assignmentInfoCollection;
                    tempAssignments.Add(assignmentInfoCollection);
                }
                else
                {
                    tempAssignments.Add(assignmentInfoCollection);
                }
            }

            if (mostAssignmentInfoCollection == null)
            {
                throw new InvalidOperationException("No preferred constructor found, but there should one.");
            }

            var result = new ConstructorAssignmentCollection(mostAssignmentInfoCollection);
            result.AddConstructorAssignment(tempAssignments);

            return result;
        }

        /// <summary>
        /// Builds a assignment info for the specified constructor.
        /// </summary>
        /// <param name="constructor">The constructor to build the parameter assignment info's for.</param>
        /// <returns>An <see cref="AssignmentInfoCollection"/> initialized with the data from the specified 
        /// <paramref name="constructor"/>.</returns>
        private AssignmentInfoCollection BuildAssignmentInfoForConstructor(BindingFlags bindingAttr, ConstructorInfo constructor)
        {
            // var ctorParameterTypesInterface = new List<ParameterInfo>();
            // var ctorParameterTypesStd = new List<ParameterInfo>();
            var ctorParameterTypes = new ParameterInfoCollection();
            var ctorParameters = constructor.GetParameters();
            foreach (var para in ctorParameters)
            {
                // if (!para.ParameterType.IsGenericType)
                {
                    ctorParameterTypes.AddParameterInfo(para);

                    /*if (para.ParameterType.IsInterface)
                                        {
                                            hasInterfaceInCtorParameters = true;
                                            ctorParameterTypesInterface.Add(para);
                                        }
                                        else
                                        {
                                            ctorParameterTypesStd.Add(para);
                                        }*/
                }
            }

            /*if (ctorParameterTypes.CountOfStandardTypes > 0)
            {

            }*/

            /*if (!ctorParameterTypes.HasInterfaces)
            {
                //return new CodeAssignStatement[0];
                return assignments;
            }*/

            // var mockRepositoryMemberField = this.AddMockRepository(
            // testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            // BuildAssignmentInfoForConstructor
            var assignmentInfoCollection = new AssignmentInfoCollection { UsedConstructor = constructor };
            var addTypesFrom = ctorParameterTypes.StandardTypes;
            if ((bindingAttr & BindingFlags.NonPublic)  == BindingFlags.NonPublic)
            {
                addTypesFrom = ctorParameterTypes;
            }

            foreach (var paraInfo in addTypesFrom)
            {
                if (!paraInfo.ParameterType.IsGenericType)
                {
                    var memberField = BaseCSharpCodeGenerator.CreateMemberField(
                        paraInfo.ParameterType.FullName, paraInfo.Name);
                    var fieldAssignment = CodeMethodComposer.CreateAndInitializeMemberField(
                        paraInfo.ParameterType, paraInfo.Name);
                    var assignment = new ConstructorAssignment(paraInfo.Name, fieldAssignment, memberField, paraInfo.ParameterType);
                    assignmentInfoCollection.AddAssignment(assignment);
                }
                else
                {
                    var genericTypeDefinition = paraInfo.ParameterType.GetGenericTypeDefinition();

                    if (this.TestObjectMemberField.Type.BaseType == "DefaultMemberBuilderFactory")
                    {

                    }

                    if (typeof(IEnumerable<>).IsAssignableFrom(genericTypeDefinition))
                    {
                        var genArgs = paraInfo.ParameterType.GetGenericArguments();
                        if (genArgs.Length == 1)
                        {
                            var memberFieldName = paraInfo.Name + "Item";
                            var memberField = BaseCSharpCodeGenerator.CreateMemberField(
                                genArgs[0].FullName, memberFieldName);
                            var fieldAssignment = CodeMethodComposer.CreateAndInitializeMemberField(
                                genArgs[0], memberFieldName);
                            var assignment = new ConstructorAssignment(memberFieldName, fieldAssignment, memberField, genArgs[0]);
                            // assignmentInfoCollection.AddAssignment(assignment);
                            // AddAssignStatement(fieldAssignment);

                            var collectionFieldName = paraInfo.Name;
                            var collectionField = BaseCSharpCodeGenerator.CreateMemberField(
                                paraInfo.ParameterType.FullName, collectionFieldName);
                            var collectionAssignment = CodeMethodComposer.CreateAndInitializeCollectionField(
                                paraInfo.ParameterType, collectionFieldName, memberFieldName);
                            var collection = new ConstructorAssignment(collectionFieldName, collectionAssignment, collectionField, paraInfo.ParameterType);
                            collection.CreateAssignments.Add(assignment);
                            assignmentInfoCollection.AddAssignment(collection);
                        }
                    }

                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(paraInfo.ParameterType))
                    {

                    }

                    if (paraInfo.ParameterType.IsAssignableFrom(typeof(IEnumerable<>)))
                    {
                        
                    }
                }
            }

            return assignmentInfoCollection;
        }

        /// <summary>
        /// Reorders the statements of the specified method at the specified position to the last position 
        /// of the code generation statements.
        /// </summary>
        /// <param name="method">The method to re-order.</param>
        /// <param name="testObjectInitializerPosition">The position of the statement to push to the bottom.</param>
        private void ReorderSetupStatement(CodeMemberMethod method, int testObjectInitializerPosition)
        {
            var removedTypedec = method.Statements[testObjectInitializerPosition];
            method.Statements.RemoveAt(testObjectInitializerPosition);
            method.Statements.Add(removedTypedec);
        }
    }
}