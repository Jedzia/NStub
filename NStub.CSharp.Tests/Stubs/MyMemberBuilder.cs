using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.CSharp.ObjectGeneration;
using NStub.CSharp.BuildContext;

namespace NStub.CSharp.Tests.Stubs
{
    public class MyMemberBuilder : IMemberBuilder
    {
        #region IMemberBuilder Members

        public int PreBuildCalled { get; set; }
        public int BuildCalled { get; set; }
        public int GetTestNameCalled { get; set; }
        public IMemberBuildContext Context { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuildHandler"/> class.
        /// </summary>
        public MyMemberBuilder(IMemberBuildContext context)
        {
            this.Context = context;
        }

        public bool Build(NStub.CSharp.BuildContext.IMemberBuildContext context)
        {
            BuildCalled++;
            return true;
        }

        public string GetTestName(NStub.CSharp.BuildContext.IMemberBuildContext context, string originalName)
        {
            GetTestNameCalled++;
            return "what a test";
        }

        #endregion

        #region IMemberBuilder Members


        public void RunPreBuild(IMemberPreBuildContext context)
        {
            PreBuildCalled++;
        }

        #endregion
    }
}
