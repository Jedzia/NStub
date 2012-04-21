using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.IO;
using NStub.CSharp.ObjectGeneration.Builders;

namespace NStub.Gui
{
    /// <summary>
    /// Test class for the object dumper. DeleteME
    /// </summary>
    public class MyObjectDumper
    {
        /// <summary>
        /// Tests this instance.
        /// </summary>
        /// <returns>the dumped object </returns>
        public string Test()
        {

            //TestXmlSeria();

            var testObject = new CodeTypeDeclaration("DeclClass");
            var method = new CodeMemberMethod() { Name = "MyMethod" };
            testObject.Members.Add(method);
            var method2 = new CodeMemberMethod() { Name = "OtherMethod" };
            testObject.Members.Add(method2);
            var comment = new CodeCommentStatement("This is a comment");
            method.Statements.Add(comment);
            var prop = new CodeMemberProperty() { Name = "MyProperty", Type = new CodeTypeReference(typeof(string)) };
            testObject.Members.Add(prop);

            testObject.Dump("The description", 5);
            //var res = Extensions.Text;
            return string.Empty;
            
            /* var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var swriter = new StringWriter();
            ObjectDumper.Write(testObject, int.MaxValue, swriter);
            //var xx = ms.ToString();
            var result = swriter.ToString();*/
        }

        /*private static void TestXmlSeria()
        {
            var pb = new PropertyBuilderParameters();

            var pbps = new PropertyBuilderParametersSetup();
            pbps.MethodSuffix = "OlderDepp";
            pbps.Moep = 42;
            pbps.UseDings = false;
            pb.Items.Add(pbps);

            pbps = new PropertyBuilderParametersSetup();
            pbps.MethodSuffix = "OtherParameter";
            pbps.UseDings = true;
            pb.Items.Add(pbps);

            pb.SaveToFile("PropertyBuilderParametersSetup.xml");
        }*/
    }

}
