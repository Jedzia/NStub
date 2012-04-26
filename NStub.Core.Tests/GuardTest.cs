namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using NStub.Core.Tests.Stubs;


    [TestFixture()]
    public partial class GuardTest
    {
        
        [Test()]
        public void CanBeAssignedTest()
        {
            // |     Fruit : IFruit (abstract)
            // |->   Apple : Fruit, IFruit, IRedSkinned
            // |->  Banana : Fruit, IFruit

            var reference = "Insert initialization of parameter ´reference´ here";
            var typeToAssign = typeof(Apple);
            var targetType = typeof(Fruit);
            Guard.CanBeAssigned(()=>reference, typeToAssign, targetType);

            typeToAssign = typeof(Apple);
            targetType = typeof(IFruit);
            Guard.CanBeAssigned(() => reference, typeToAssign, targetType);

            typeToAssign = typeof(Apple);
            targetType = typeof(IRedSkinned);
            Guard.CanBeAssigned(() => reference, typeToAssign, targetType);

            typeToAssign = typeof(Fruit);
            targetType = typeof(Apple);
            Assert.Throws<ArgumentException>(() => Guard.CanBeAssigned(() => reference, typeToAssign, targetType));

            typeToAssign = typeof(Banana);
            targetType = typeof(IRedSkinned);
            Assert.Throws<ArgumentException>(() => Guard.CanBeAssigned(() => reference, typeToAssign, targetType));
        }
        
        [Test()]
        public void CanBeAssignedToTest()
        {
            // |     Fruit : IFruit (abstract)
            // |->   Apple : Fruit, IFruit, IRedSkinned
            // |->  Banana : Fruit, IFruit

            var reference = "Insert initialization of parameter ´reference´ here";
            object targetType = new Apple();
            Guard.CanBeAssignedTo<Apple>(() => reference, targetType);

            targetType = new Apple();
            Guard.CanBeAssignedTo<Fruit>(() => reference,  targetType);

            targetType = new Apple();
            Guard.CanBeAssignedTo<IRedSkinned>(() => reference, targetType);

            targetType = new Banana();
            Assert.Throws<ArgumentException>(() => Guard.CanBeAssignedTo<IRedSkinned>(() => reference, targetType));

            targetType = new AndNowToSomethingCompletelyDifferent();
            Assert.Throws<ArgumentException>(() => Guard.CanBeAssignedTo<IFruit>(() => reference,  targetType));
        }
        
        [Test()]
        public void IsAssignableFromTest()
        {
            // |     Fruit : IFruit (abstract)
            // |->   Apple : Fruit, IFruit, IRedSkinned -> GrannySmith
            // |->  Banana : Fruit, IFruit

            var reference = "Insert initialization of parameter ´reference´ here";
            object targetOfAssignment = new Apple();
            Guard.IsAssignableFrom<Apple>(() => reference, targetOfAssignment);

            targetOfAssignment = new Apple();
            Guard.IsAssignableFrom<GrannySmith>(() => reference, targetOfAssignment);

            targetOfAssignment = new Banana();
            Assert.Throws<ArgumentException>(() => Guard.IsAssignableFrom<IFruit>(() => reference, targetOfAssignment));

            targetOfAssignment = new AndNowToSomethingCompletelyDifferent();
            Assert.Throws<ArgumentException>(() => Guard.IsAssignableFrom<Fruit>(() => reference, targetOfAssignment));
        }
        
        [Test()]
        public void NotDefaultTest()
        {
            int value = 55;
            Guard.NotDefault(() => value, value);

            value = 0;
            Assert.Throws<ArgumentException>(() => Guard.NotDefault(() => value, value));
        }
        
        [Test()]
        public void NotEmptyTest()
        {
            List<int> reference = null;
            var values = new List<int>() { 1, 2, 3, 4 };
            Guard.NotEmpty(()=> reference, values);

            values = null;
            Assert.Throws<ArgumentException>(() => Guard.NotEmpty(() => reference, values));
            values = new List<int>() { };
            Assert.Throws<ArgumentException>(() => Guard.NotEmpty(() => reference, values));
        }
        
        [Test()]
        public void NotNullOrEmptyTest()
        {
            string reference = null;
            var value = "Bla";
            Guard.NotNullOrEmpty(() => reference, value);

            value = string.Empty;
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty(() => reference, value));

            value = reference;
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty(() => reference, value));
        }
        
        [Test()]
        public void NotNullTest()
        {
            object reference = null;
            object value = new InfoApe();
            Guard.NotNull(() => reference, value);

            value = reference;
            Assert.Throws<ArgumentException>(() => Guard.NotNull(() => reference, value));
        }
        
        [Test()]
        public void NotOutOfRangeExclusiveTest()
        {
            int value = 4;
            Guard.NotOutOfRangeExclusive(() => value, value, 3, 9);
            value = 6;
            Guard.NotOutOfRangeExclusive(() => value, value, 3, 9);
            value = 8;
            Guard.NotOutOfRangeExclusive(() => value, value, 3, 9);


            value = 3;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => value, value, 3, 9));
            value = 9;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => value, value, 3, 9));
            value = -15;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => value, value, 3, 9));
            value = 15;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeExclusive(() => value, value, 3, 9));
        }
        
        [Test()]
        public void NotOutOfRangeInclusiveTest()
        {
            int value = 3;
            Guard.NotOutOfRangeInclusive(() => value, value, 3, 9);
            value = 6;
            Guard.NotOutOfRangeInclusive(() => value, value, 3, 9);
            value = 9;
            Guard.NotOutOfRangeInclusive(() => value, value, 3, 9);


            value = 2;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => value, value, 3, 9));
            value = 10;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => value, value, 3, 9));
            value = -15;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => value, value, 3, 9));
            value = 15;
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotOutOfRangeInclusive(() => value, value, 3, 9));
        }
    }
}
