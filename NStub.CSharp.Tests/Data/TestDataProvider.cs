using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.IO;

namespace NStub.CSharp.Tests.Data
{
    internal class TestDataProvider
    {
        public static string BuildParametersXml()
        {
            string result = string.Empty;
            var rstream = typeof(TestDataProvider).Assembly.GetManifestResourceStream("NStub.CSharp.Tests.Data.BuildParameters.xml");
            using (var reader = new StreamReader(rstream))
            {
                // var rsr = new ResourceReader("NStub.CSharp.Tests.Data.BuildParameters.xml");
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static string BuildParametersMinimalDefaultMethodEraserXml()
        {
            var xml = 
@"<?xml version=""1.0"" encoding=""utf-16""?>" + Environment.NewLine +
@"<BuildParameters>" + Environment.NewLine +
@"  <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser>" + Environment.NewLine +
@"    <EmptyBuildParameters>" + Environment.NewLine +
@"      <Enabled>true</Enabled>" + Environment.NewLine +
@"    </EmptyBuildParameters>" + Environment.NewLine +
@"  </NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser>" + Environment.NewLine +
@"</BuildParameters>" + Environment.NewLine +
@"";
            return xml;
        }

        public static string BuildParametersWrongMultiMinimalDefaultMethodEraserXml()
        {
            var xml =
@"<?xml version=""1.0"" encoding=""utf-16""?>" + Environment.NewLine +
@"<BuildParameters>" + Environment.NewLine +
@"  <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser>" + Environment.NewLine +
@"    <EmptyMultiBuildParameters Id=""f24371d7-db32-4fd2-b97b-19c2c0d73be5"">" + Environment.NewLine +
@"      <Enabled>true</Enabled>" + Environment.NewLine +
@"    </EmptyMultiBuildParameters>" + Environment.NewLine +
@"  </NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser>" + Environment.NewLine +
@"</BuildParameters>" + Environment.NewLine +
@"";
            return xml;
        }

        public static string BuildParametersMinimal(string buildParametersXmlId, Type builderType, Type parameterType, bool enabled)
        {
            var btypeNominator = builderType.FullName;
            var ptypeNominator = parameterType.Name;
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>" + Environment.NewLine +
@"<" + buildParametersXmlId + ">" + Environment.NewLine +
@"  <" + btypeNominator + ">" + Environment.NewLine +
@"    <" + ptypeNominator + ">" + Environment.NewLine +
@"      <Enabled>" + enabled.ToString().ToLowerInvariant() + "</Enabled>" + Environment.NewLine +
@"    </" + ptypeNominator + ">" + Environment.NewLine +
@"  </" + btypeNominator + ">" + Environment.NewLine +
@"</" + buildParametersXmlId + ">" + Environment.NewLine +
@"";
            return xml;
        }

    }
}
