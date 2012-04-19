// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorAssignmentCollection.cs" company="EvePanix">
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
    using NStub.Core;

    /// <summary>
    /// A list of <see cref="AssignmentInfoCollection"/>'s which holds a reference to a <see cref="AssignmentInfoCollection"/>
    /// of the preferred constructor used in code generation.
    /// </summary>
    internal class ConstructorAssignmentCollection : IEnumerable<AssignmentInfoCollection>
    {
        #region Fields

        private readonly List<AssignmentInfoCollection> ctorAssignments = new List<AssignmentInfoCollection>();

        #endregion

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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorAssignmentCollection"/> class.
        /// </summary>
        /// <param name="preferredConstructor">The preferred constructor, the code generator should use.</param>
        public ConstructorAssignmentCollection(AssignmentInfoCollection preferredConstructor)
        {
            Guard.NotNull(() => preferredConstructor, preferredConstructor);
            this.PreferredConstructor = preferredConstructor;
            AddConstructorAssignment(preferredConstructor);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of assignments in the list.
        /// </summary>
        public int Count
        {
            get
            {
                return this.ctorAssignments.Count;
            }
        }

        /// <summary>
        /// Gets the preferred constructor assignment that should be used by the code generator.
        /// </summary>
        public AssignmentInfoCollection PreferredConstructor { get; private set; }

        #endregion

        /// <summary>
        /// Add a constructor assignment info to the list.
        /// </summary>
        /// <param name="constructorAssignment">The constructor assignment.</param>
        public void AddConstructorAssignment(AssignmentInfoCollection constructorAssignment)
        {
            this.ctorAssignments.Add(constructorAssignment);
        }

        /// <summary>
        /// Add range of constructor assignment infos to the list.
        /// </summary>
        /// <param name="constructorAssignments">The constructor assignments.</param>
        public void AddConstructorAssignment(IEnumerable<AssignmentInfoCollection> constructorAssignments)
        {
            this.ctorAssignments.AddRange(constructorAssignments);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<AssignmentInfoCollection> GetEnumerator()
        {
            return this.ctorAssignments.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ctorAssignments.GetEnumerator();
        }
    }
}