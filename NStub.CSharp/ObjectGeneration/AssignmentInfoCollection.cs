// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignmentInfo.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A list of <see cref="ConstructorAssignment"/>'s which holds a reference to a <see cref="ConstructorInfo"/>
    /// that is preferably used in code generation.
    /// to a assignment, that should be used.
    /// </summary>
    public class AssignmentInfoCollection : IEnumerable<ConstructorAssignment>
    {
        #region Fields

        private readonly Dictionary<string, ConstructorAssignment> assignments =
            new Dictionary<string, ConstructorAssignment>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of assignments.
        /// </summary>
        public int Count
        {
            get
            {
                return this.assignments.Count;
            }
        }

        /// <summary>
        /// Gets or sets the constructor related to the parameter assignments of this instance.
        /// </summary>
        /// <value>
        /// The related constructor.
        /// </value>
        public ConstructorInfo UsedConstructor { get; internal set; }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the <see cref="NStub.CSharp.ObjectGeneration.ConstructorAssignment"/> with the specified parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to get the <see cref="ConstructorAssignment"/> for.</param>
        /// <value>The <see cref="ConstructorAssignment"/> at the specified index.</value>
        public ConstructorAssignment this[string parameterName]
        {
            get
            {
                ConstructorAssignment returnValue;
                this.assignments.TryGetValue(parameterName, out returnValue);
                return returnValue;
            }
        }

        #endregion

        /// <summary>
        /// Gets a new <see cref="AssignmentInfoCollection"/> that is empty.
        /// </summary>
        /// <returns>a new <see cref="AssignmentInfoCollection"/> that is empty.</returns>
        public static AssignmentInfoCollection Empty()
        {
            var returnValue = new AssignmentInfoCollection();
            return returnValue;
        }

        /// <summary>
        /// Adds a assignment to the list of assignments.
        /// </summary>
        /// <param name="assignment">The assignment to add.</param>
        public void AddAssignment(ConstructorAssignment assignment)
        {
            this.assignments.Add(assignment.ParameterName, assignment);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ConstructorAssignment> GetEnumerator()
        {
            return this.assignments.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.assignments.Values.GetEnumerator();
        }
    }
}