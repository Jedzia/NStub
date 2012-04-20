using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.Core;
using System.CodeDom;
using NStub.CSharp.ObjectGeneration;
using NStub.CSharp.ObjectGeneration.Builders;

namespace NStub.CSharp
{
    public class CSharpTestProjectBuilder : TestProjectBuilder
    {
        private readonly Func<IBuildSystem, IBuildDataCollection, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectBuilder"/> class.
        /// </summary>
        /// <param name="sbs">The build system.</param>
        /// <param name="projectGenerator">The project generator.</param>
        /// <param name="createGeneratorCallback">The callback to create new code 
        /// generators per test class <see cref="CodeNamespace"/>.</param>
        /// <param name="logger">The logging method.</param>
        public CSharpTestProjectBuilder(
            IBuildSystem sbs,
            IBuildDataCollection buildData,
            IProjectGenerator projectGenerator,
            Func<IBuildSystem, IBuildDataCollection, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback,
            Action<string> logger)
            : base(sbs, projectGenerator, OldOnCreateCodeGenerator, logger)
        {
            Guard.NotNull(() => buildData, buildData);
            Guard.NotNull(() => createGeneratorCallback, createGeneratorCallback);
            this.createGeneratorCallback = createGeneratorCallback;

            //properties = SetUpBuildProperties();
            properties = SetUpBuildProperties(buildData);
        }

        private readonly BuildDataCollection buildData;// = new BuildDataCollection();

        private static BuildDataCollection SetUpBuildProperties(IBuildDataCollection buildData)
        {
            var props = new BuildDataCollection();

            //ReadOnlyCollection
            foreach (var category in buildData.Data())
            {
                foreach (var item in category.Value)
                {
                    props.AddDataItem(category.Key, item.Key, item.Value);
                }
                //props.AddDataItem(item.Key, item,);
            }
            props.AddDataItem("DasGuuuut", MemberBuilder.EmptyParameters);
            // Todo: !!!
            return props;
        }

        private static ICodeGenerator OldOnCreateCodeGenerator(IBuildSystem buildSystem, ICodeGeneratorParameters configuration, CodeNamespace codeNamespace)
        {
            return null;
        }

        protected override ICodeGenerator OnCreateCodeGenerator(
            CodeNamespace codeNamespace,
            CodeGeneratorParameters configuration,
            IBuildSystem buildSystem)
        {
            var codeGenerator = this.createGeneratorCallback(buildSystem, properties, configuration, codeNamespace);
            
            return codeGenerator;
        }

        private readonly BuildDataCollection properties;
    }
}
