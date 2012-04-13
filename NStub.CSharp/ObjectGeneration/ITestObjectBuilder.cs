// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestObjectBuilder.cs" company="EvePanix">
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

    /// <summary>
    /// Has the capability of creating new test objects.
    /// </summary>
    public interface ITestObjectBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the build data dictionary that stores generation wide category/key/value properties.
        /// </summary>
        BuildDataCollection BuildData { get; }

        /// <summary>
        /// Gets the SetUpMethod.
        /// </summary>
        /// <value>The SetUpMethod.</value>
        CodeMemberMethod SetUpMethod { get; }

        /// <summary>
        /// Gets the TestObjectMemberField.
        /// </summary>
        /// <value>The TestObjectMemberField.</value>
        CodeMemberField TestObjectMemberField { get; }

        /// <summary>
        /// Gets the member field create expression for the object under test.
        /// </summary>
        CodeObjectCreateExpression TestObjectMemberFieldCreateExpression { get; }

        /// <summary>
        /// Gets the TestObjectName.
        /// </summary>
        /// <value>The TestObjectName.</value>
        string TestObjectName { get; }

        /// <summary>
        /// Gets the type of the test object.
        /// </summary>
        /// <value>
        /// The type of the test object.
        /// </value>
        Type TestObjectType { get; }

        #endregion

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        void AssignParameters(CodeTypeDeclaration testClassDeclaration);

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        void AssignParameters(
            CodeTypeDeclaration testClassDeclaration, CodeObjectCreateExpression testObjectConstructor);

        /// <summary>
        /// Creates a code generation expression for an object to test with a member field and initialization
        /// in the previous specified <see cref="SetUpMethod"/> method.
        /// </summary>
        /// <returns>
        /// The initialization expression of the object under test.
        /// </returns>
        CodeObjectCreateExpression BuildTestObject();
    }
}