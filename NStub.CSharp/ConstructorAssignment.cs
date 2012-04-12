using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace NStub.CSharp
{
    /// <summary>
    /// Holds a mapping from parameter name to code creation expressions.
    /// </summary>
    internal class ConstructorAssignment
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Gets the assign statement for the parameter.
        /// </summary>
        public CodeAssignStatement AssignStatement { get; private set; }

        /// <summary>
        /// Gets the related member field of the parameter.
        /// </summary>
        public CodeMemberField MemberField { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorAssignment"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="assignStatement">The assign statement for the parameter.</param>
        /// <param name="memberField">The related member field of the parameter.</param>
        public ConstructorAssignment(string parameterName, CodeAssignStatement assignStatement, CodeMemberField memberField)
        {
            Guard.NotNullOrEmpty(() => parameterName, parameterName);
            Guard.NotNull(() => assignStatement, assignStatement);
            Guard.NotNull(() => memberField, memberField);

            this.ParameterName = parameterName;
            this.AssignStatement = assignStatement;
            this.MemberField = memberField;
        }
    }

    /// <summary>
    /// A list of <see cref="ConstructorAssignment"/>'s which holds a reference to a <see cref="ConstructorInfo"/>
    /// that is preferably used in code generation.
    /// to a assignment, that should be used.
    /// </summary>
    internal class AssignmentInfo : IEnumerable<ConstructorAssignment>
    {
        private Dictionary<string, ConstructorAssignment> assignments = new Dictionary<string, ConstructorAssignment>();

        /// <summary>
        /// Adds a assignment to the list of assignments.
        /// </summary>
        /// <param name="assignment">The assignment to add.</param>
        public void AddAssignment(ConstructorAssignment assignment)
        {
            assignments.Add(assignment.ParameterName, assignment);
        }

        /// <summary>
        /// Gets or sets the constructor related to the parameter assignments of this instance.
        /// </summary>
        /// <value>
        /// The related constructor.
        /// </value>
        public ConstructorInfo UsedConstructor { get; internal set; }

        /// <summary>
        /// Gets the number of assignments.
        /// </summary>
        public int Count
        {
            get { return assignments.Count; }
        }

        /// <summary>
        /// Gets a new <see cref="AssignmentInfo"/> that is empty.
        /// </summary>
        /// <returns>a new <see cref="AssignmentInfo"/> that is empty.</returns>
        public static AssignmentInfo Empty()
        {
            var returnValue = new AssignmentInfo();
            return returnValue;
        }

        #region IEnumerable<ConstructorAssignment> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ConstructorAssignment> GetEnumerator()
        {
            return assignments.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return assignments.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the <see cref="T:ConstructorAssignment"/> at the specified index.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to get the <see cref="ConstructorAssignment"/> for.</param>
        /// <value>The <see cref="T:ConstructorAssignment"/> at the specified index.</value>
        public ConstructorAssignment this[string parameterName]
        {
            get
            {
                ConstructorAssignment returnValue;
                bool result = this.assignments.TryGetValue(parameterName, out returnValue);
                return returnValue;
            }

            /*set
            {
                // TODO: set the specified index to value here 
            }*/
        }
        #endregion
    }

    /// <summary>
    /// List of <see cref="ParameterInfo"/>'s with Interface detection for thetypes of the added parameters.
    /// </summary>
    internal class ParameterInfoList : IEnumerable<ParameterInfo>
    {
        private List<ParameterInfo> parameters = new List<ParameterInfo>();

        /// <summary>
        /// Gets a value indicating whether this instance has parameters of a interface type stored.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has interfaces in the parameter types; otherwise, <c>false</c>.
        /// </value>
        public bool HasInterfaces
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of parameters.
        /// </summary>
        public int Count
        {
            get { return parameters.Count; }
        }

        /// <summary>
        /// Gets the number of parameters with standard arguments. ( Currently all but interfaces ).
        /// </summary>
        public int CountOfStandardTypes
        {
            get { return parameters.Count(e => !e.ParameterType.IsInterface); }
        }

        /// <summary>
        /// Adds the specified parameter info to this list.
        /// </summary>
        /// <param name="parameterInfo">The parameter info to add.</param>
        public void AddParameterInfo(ParameterInfo parameterInfo)
        {
            parameters.Add(parameterInfo);
            if (parameterInfo.ParameterType.IsInterface)
            {
                this.HasInterfaces = true;
            }
        }

        #region IEnumerable<ParameterInfo> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ParameterInfo> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        /// <summary>
        /// Gets the parameters where the parameter-type isn't an interface.
        /// </summary>
        public IEnumerable<ParameterInfo> StandardTypes
        {
            get
            {
                return parameters.Where(e => !e.ParameterType.IsInterface);
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        #endregion
    }


    /// <summary>
    /// A list of <see cref="AssignmentInfo"/>'s which holds a reference to a <see cref="AssignmentInfo"/>
    /// of the preferred constructor used in code generation.
    /// </summary>
    internal class ConstructorAssignmentList : IEnumerable<AssignmentInfo>
    {
        private List<AssignmentInfo> ctorAssignments = new List<AssignmentInfo>();

        /*/// <summary>
        /// Gets a value indicating whether this instance has parameter assigned.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has parameter assignmed; otherwise, <c>false</c>.
        /// </value>
        public bool HasParameterAssignments
        {
            get
            {
                return ctorAssignments.Count > 0;
            }
        }*/


        /// <summary>
        /// Gets the preferred constructor assignment that should be used by the code generator.
        /// </summary>
        public AssignmentInfo PreferredConstructor
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstructorAssignmentList"/> class.
        /// </summary>
        /// <param name="preferredConstructor">The preferred constructor, the code generator should use.</param>
        public ConstructorAssignmentList(AssignmentInfo preferredConstructor)
        {
            Guard.NotNull(() => preferredConstructor, preferredConstructor);
            this.PreferredConstructor = preferredConstructor;
            AddConstructorAssignment(preferredConstructor);
        }

        /// <summary>
        /// Gets the number of assignments in the list.
        /// </summary>
        public int Count
        {
            get { return ctorAssignments.Count; }
        }

        /// <summary>
        /// Add a constructor assignment info to the list.
        /// </summary>
        /// <param name="constructorAssignment">The constructor assignment.</param>
        public void AddConstructorAssignment(AssignmentInfo constructorAssignment)
        {
            ctorAssignments.Add(constructorAssignment);
        }

        /// <summary>
        /// Add range of constructor assignment infos to the list.
        /// </summary>
        /// <param name="constructorAssignments">The constructor assignments.</param>
        public void AddConstructorAssignment(IEnumerable<AssignmentInfo> constructorAssignments)
        {
            ctorAssignments.AddRange(constructorAssignments);
        }

        #region IEnumerable<ParameterInfo> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<AssignmentInfo> GetEnumerator()
        {
            return ctorAssignments.GetEnumerator();
        }


        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ctorAssignments.GetEnumerator();
        }

        #endregion
    }

}
