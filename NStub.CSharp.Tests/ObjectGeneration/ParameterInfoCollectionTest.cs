namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
using System.Reflection;
    
    
    [TestFixture()]
    public partial class ParameterInfoCollectionTest
    {

        private ParameterInfoCollection testObject;
        private ParameterInfoCollection testObjectWithInterface;
        private ParameterInfo[] parameterInfoMethodWithInterfaces;
        private ParameterInfo[] parameterInfoMethod;

        public double InfoMethod(string a, int b, bool c, Pointer d)
        {
            return 3.14159d;
        }

        public double InfoMethodWithInterface(string a, int b, bool c, ICloneable d)
        {
            return 3.14159d;
        }

        [SetUp()]
        public void SetUp()
        {
            this.testObjectWithInterface = new ParameterInfoCollection();
            var methodInfo = typeof(ParameterInfoCollectionTest).GetMethod("InfoMethodWithInterface");
            var parameters = methodInfo.GetParameters();
            this.parameterInfoMethodWithInterfaces = parameters.ToArray();
            foreach (var parameter in parameters)
            {
                testObjectWithInterface.AddParameterInfo(parameter);
            }

            this.testObject = new ParameterInfoCollection();
            methodInfo = typeof(ParameterInfoCollectionTest).GetMethod("InfoMethod");
            parameters = methodInfo.GetParameters();
            this.parameterInfoMethod = parameters.ToArray();
            foreach (var parameter in parameters)
            {
                testObject.AddParameterInfo(parameter);
            }
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObjectWithInterface = null;
            this.testObject = null;
            this.parameterInfoMethodWithInterfaces = null;
        }
        
        [Test()]
        public void ConstructWithParametersTest()
        {
            this.testObjectWithInterface = new ParameterInfoCollection();
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 4;
            var actual = testObjectWithInterface.Count;
            Assert.AreEqual(expected, actual);

             actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyCountOfStandardTypesNormalBehavior()
        {
            // Test read access of 'CountOfStandardTypes' Property.
            // 3 Standard and 1 Interface parameter types on method "InfoMethodWithInterface" above.
            var expected = 3;
            var actual = testObjectWithInterface.CountOfStandardTypes;
            Assert.AreEqual(expected, actual);

            // 4 Standard parameter types on method "InfoMethod" above.
            expected = 4;
            actual = testObject.CountOfStandardTypes;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyHasInterfacesNormalBehavior()
        {
            // Test read access of 'HasInterfaces' Property.
            var expected = true;
            var actual = testObjectWithInterface.HasInterfaces;
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = testObject.HasInterfaces;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyStandardTypesNormalBehavior()
        {
            // Test read access of 'StandardTypes' Property.
            var expected = parameterInfoMethodWithInterfaces.Where(e => !e.ParameterType.IsInterface);
            var actual = testObjectWithInterface.StandardTypes;
            Assert.AreElementsEqual(expected, actual);

            expected = parameterInfoMethod;
            actual = testObject.StandardTypes;
            Assert.AreElementsEqual(expected, actual);
        }

        [Test()]
        public void IEnumerable()
        {
            Assert.Count(4, testObjectWithInterface);
            Assert.Count(4, testObject);

            var expected = parameterInfoMethodWithInterfaces;
            var actual = testObjectWithInterface;
            Assert.AreElementsEqual(expected, actual);

            expected = parameterInfoMethod;
            actual = testObject;
            Assert.AreElementsEqual(expected, actual);
        }

        [Test()]
        public void AddParameterInfoTest()
        {
            Assert.IsFalse(testObject.HasInterfaces);
            Assert.Count(4, testObject);
            var methodInfo = typeof(ParameterInfoCollectionTest).GetMethod("InfoMethodWithInterface");
            var parameters = methodInfo.GetParameters();
            foreach (var parameter in parameters)
            {
                testObject.AddParameterInfo(parameter);
            }

            Assert.IsTrue(testObject.HasInterfaces);
            Assert.Count(8, testObject);
            
            var expected = parameterInfoMethod.Concat(parameters);
            var actual = testObject.StandardTypes;
            Assert.AreElementsEqual(expected.Where(e => !e.ParameterType.IsInterface), actual);

            actual = testObject;
            Assert.AreElementsEqual(expected, actual);
        }

        [Test()]
        public void GetEnumeratorTest()
        {
            Assert.IsNotNull(testObjectWithInterface.GetEnumerator());
            Assert.IsNotNull(((System.Collections.IEnumerable)testObjectWithInterface).GetEnumerator());
            Assert.IsInstanceOfType<IEnumerator<AssignmentInfoCollection>>(testObjectWithInterface.GetEnumerator());
        }
    }
}
