using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.CSharp.Tests.Stubs
{
    [ParameterDescription("Bla")]
    public class InfoApe
    {
        public void PublicVoidMethodVoid()
        {
        }

        public event EventHandler PublicEventObject;

        public int PublicPropertyGetSetInt { get; set; }
        
        public string PublicPropertyGetSetString { get; set; }
    }
}
