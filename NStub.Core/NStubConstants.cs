using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.Core
{
    /// <summary>
    /// Global constants related to build data.
    /// </summary>
    public static class NStubConstants
    {
        /// <summary>
        /// Key to the user data storage of class <see cref="System.Type"/>'s in 
        /// <see cref="System.CodeDom.CodeTypeDeclaration"/>'s created by the <see cref="TestProjectBuilder"/>. 
        /// For each <i>class under test</i>, this user data stores the associated CLR-Type of the
        /// code generator class definition.</summary>
        public const string UserDataClassTypeKey = "TestObjectClassType";

        /// <summary>
        /// Key to the user data storage of member <see cref="System.Reflection.MethodInfo"/>'s in 
        /// <see cref="System.CodeDom.CodeTypeMember"/>'s created by the <see cref="TestProjectBuilder"/>.
        /// For each <i>class under test</i> in each method, property, event, etc., this user data 
        /// stores the associated member info of the code generator test method definition.</summary>
        public const string TestMemberMethodInfoKey = "MethodMemberInfo";
    }
}
