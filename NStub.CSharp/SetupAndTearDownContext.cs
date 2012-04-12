using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace NStub.CSharp
{

    /// <summary>
    /// Represents the data used in SetUp and TearDown test-method generation.
    /// </summary>
    public interface ISetupAndTearDownContext
    {

        /// <summary>
        /// Gets a reference to the test SetUp method.
        /// </summary>
         CodeMemberMethod SetUpMethod { get; }
         
        /// <summary>
         /// Gets a reference to the test TearDown method.
         /// </summary>
         CodeMemberMethod TearDownMethod { get; }

         /// <summary>
         /// Gets the test object member field initialization expression ( this.testObject = new Foo( ... ) ) 
         /// of the test SetUp method.
         /// </summary>
        CodeObjectCreateExpression TestObjectMemberFieldCreate { get; }
    }


    /// <summary>
    /// Abstract base class for data used in SetUp and TearDown test-method generation.
    /// </summary>
    public abstract class SetupAndTearDownContextBase : ISetupAndTearDownContext
    {
        /// <summary>
        /// Gets a reference to the test SetUp method.
        /// </summary>
        public CodeMemberMethod SetUpMethod { get; private set; }

        /// <summary>
        /// Gets a reference to the test TearDown method.
        /// </summary>
        public CodeMemberMethod TearDownMethod { get; private set; }

        /// <summary>
        /// Gets the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </summary>
        public CodeObjectCreateExpression TestObjectMemberFieldCreate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAndTearDownContextBase"/> class.
        /// </summary>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="testObjectMemberFieldCreate">The test object member field initialization expression
        /// of the test SetUp method.</param>
        protected SetupAndTearDownContextBase(
            CodeMemberMethod setUpMethod,
            CodeMemberMethod tearDownMethod,
            CodeObjectCreateExpression testObjectMemberFieldCreate)
        {
            Guard.NotNull(() => setUpMethod, setUpMethod);
            Guard.NotNull(() => tearDownMethod, tearDownMethod);
            Guard.NotNull(() => testObjectMemberFieldCreate, testObjectMemberFieldCreate);

            this.SetUpMethod = setUpMethod;
            this.TearDownMethod = tearDownMethod;
            this.TestObjectMemberFieldCreate = testObjectMemberFieldCreate;
        }
    }

    /// <summary>
    /// Implementation of a data class used by SetUp and TearDown test-method generation.
    /// </summary>
    public class SetupAndTearDownContext : SetupAndTearDownContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAndTearDownContext"/> class.
        /// </summary>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="testObjectMemberFieldCreate">The test object member field initialization expression
        /// of the test SetUp method.</param>
        public SetupAndTearDownContext(CodeMemberMethod setUpMethod,
            CodeMemberMethod tearDownMethod,
            CodeObjectCreateExpression testObjectMemberFieldCreate)
            : base(setUpMethod,
             tearDownMethod,
             testObjectMemberFieldCreate)
        {
        }
    }

}
