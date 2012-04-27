using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.CSharp.Tests.Stubs
{
    public class ComposeMeCtorVoid
    {
        public ComposeMeCtorVoid()
        {

        }
    }

    public class ComposeMeCtorString
    {
        public ComposeMeCtorString(string para1, int para2)
        {

        }
    }

    public class ComposeMeTwoCtor
    {
        public ComposeMeTwoCtor(bool para1)
        {

        }

        public ComposeMeTwoCtor(string para1, int para2)
        {

        }
    }

    public interface IComposerWithInterface
    {
    }

    public class ComposeMeWithInterface : IComposerWithInterface
    {
        public ComposeMeWithInterface(string para1, IComposerWithInterface para2)
        {

        }
    }

    public class ComposeMeWithClass 
    {
        public ComposeMeWithClass(ComposeMeWithInterface para1)
        {

        }
    }

    public class ComposeMeWithIEnumerable
    {
        public ComposeMeWithIEnumerable(IEnumerable<ComposeMeWithInterface> para1)
        {

        }
    }

    public class ComposeMeThreeCtor
    {
        public ComposeMeThreeCtor()
        {

        }
        public ComposeMeThreeCtor(bool para1)
        {

        }

        public ComposeMeThreeCtor(string para1, int para2)
        {

        }
    }

    public class ComposeMeVisibility
    {
        private ComposeMeVisibility(string para1, string para2, int para3, double para4)
        {

        }

        protected ComposeMeVisibility(bool para1, string para2, int para3)
        {

        }

        internal ComposeMeVisibility(int para1, bool para2)
        {

        }

        public ComposeMeVisibility(DateTime para1)
        {

        }
    }

}
