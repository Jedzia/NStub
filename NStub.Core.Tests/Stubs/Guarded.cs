using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.Core.Tests.Stubs
{
    interface IFruit { }

    interface IRedSkinned { }

    abstract class Fruit : IFruit { }

    class Apple : Fruit, IRedSkinned { }

    class Banana : Fruit { }

    class GrannySmith : Apple { }

    class AndNowToSomethingCompletelyDifferent { }
}
