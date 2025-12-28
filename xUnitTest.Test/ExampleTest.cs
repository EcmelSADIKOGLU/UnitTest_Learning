using UnitTest.App;

namespace xUnitTest.Test
{
    public class ExampleTest
    {

        [Fact] // Test method with no parameters
        public void SumTest()
        {
            // Arrange
            var calculator = new Calculator();

            int a = 5;
            int b = 10;
            int expected = 15;

            // Act
            int result = calculator.Sum(a, b);

            // Assert
            Assert.Equal<int>(expected, result);
        }

        [Theory] // Test method with parameters
        [InlineData(3, 4, 7)]
        public void SumTest_WithParameters(int a, int b, int expected)
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            int result = calculator.Sum(a, b);

            // Assert
            Assert.Equal<int>(expected, result);
        }


        [Fact] 
        public void Test_Equal()
        {
            //  Assert.Equal<valueType>(expected, actual);

            Assert.Equal<int>(10, 10);      // Success
            Assert.Equal<int>(5, 10);       // Error

            Assert.Equal<int>(10, 10);      // Error
            Assert.Equal<int>(5, 10);       // Success
        }

        [Fact]
        public void Test_Contain()
        {
            //  Assert.Contains(expected, actual);

            Assert.Contains("Ecmel", "Ecmel Sadıkoğlu");        // Success
            Assert.Contains("Fatih", "Ecmel Sadıkoğlu");        // Error

            Assert.DoesNotContain("Ecmel", "Ecmel Sadıkoğlu");  // Error
            Assert.DoesNotContain("Fatih", "Ecmel Sadıkoğlu");  // Success


            var list = new List<int>() { 1, 2, 3, 4, 5 };

            Assert.Contains<int>(3, list);                      // Success
            Assert.Contains<int>(10, list);                     // Error

            Assert.DoesNotContain<int>(3, list);                // Error
            Assert.DoesNotContain<int>(10, list);               // Success
        }

        [Fact]
        public void Test_TrueFalse()
        {
            //  Assert.True(condition);
            //  Assert.False(condition);

            bool trueCondition = 5 > 3;
            bool falseCondition = 5 < 3;

            Assert.True(trueCondition);       // Success
            Assert.True(falseCondition);      // Error

            Assert.False(trueCondition);      // Error
            Assert.False(falseCondition);     // Success

        }

        [Fact]
        public void Test_Match()
        {
            //  Assert.Matches(regexPattern, actualString);

            Assert.Matches("^Ecmel", "Ecmel Sadıkoğlu");              // Success
            Assert.Matches("^Sadıkoğlu", "Ecmel Sadıkoğlu");          // Error

            Assert.DoesNotMatch("^Ecmel", "Ecmel Sadıkoğlu");         // Error
            Assert.DoesNotMatch("^Sadıkoğlu", "Ecmel Sadıkoğlu");     // Success
        }

        [Fact]
        public void Test_StartsEndsWith()
        {
            // Works for strings Not for other types

            //  Assert.StartsWith(expectedStartString, actualString);
            //  Assert.EndsWith(expectedEndString, actualString);

            Assert.StartsWith("Ecmel", "Ecmel Sadıkoğlu");      // Success
            Assert.StartsWith("Sadıkoğlu", "Ecmel Sadıkoğlu");  // Error

            Assert.EndsWith("Ecmel", "Ecmel Sadıkoğlu");        // Error
            Assert.EndsWith("Sadıkoğlu", "Ecmel Sadıkoğlu");    // Success
        }

        [Fact]
        public void Test_Empty()
        {
            //  Assert.Empty(collection);

            Assert.Empty(new List<int>());            // Success
            Assert.Empty(new List<int>() { 1 });      // Error

            Assert.NotEmpty(new List<int>());         // Error
            Assert.NotEmpty(new List<int>() { 1 });   // Success


            Assert.Empty("");          // Success
            Assert.Empty("Ecmel");     // Error

            Assert.NotEmpty("");       // Error
            Assert.NotEmpty("Ecmel");  // Success
        }

        [Fact]
        public void Test_InRange()
        {
            //  Assert.InRange<T>(actual, low, high);

            Assert.InRange<int>(5, 1, 10);      // Success
            Assert.InRange<int>(15, 1, 10);     // Error

            Assert.NotInRange<int>(5, 1, 10);   // Error
            Assert.NotInRange<int>(15, 1, 10);  // Success
        }

        [Fact]
        public void Test_Single()
        {
            // Test success if collection has exactly one element.

            //  Assert.Single(collection);

            Assert.Single<int>(new List<int>() { 1 });          // Success
            Assert.Single<int>(new List<int>() { 1, 2 });       // Error
            Assert.Single<int>(new List<int>());                // Error
        }

        [Fact]
        public void Test_isType()
        {
            //  Assert.IsType<expectedType>(object);

            // If typeof(object) == expectedType then test is successful.

            int integer = 10;

            //Assert.IsType(integer.GetType(), integer);   // Success
            //Assert.IsType(typeof(int), integer);         // Success

            Assert.IsType<int>(integer);           // Success
            Assert.IsType<string>(integer);        // Error

            Assert.IsNotType<int>(integer);        // Error
            Assert.IsNotType<string>(integer);     // Success
        }

        [Fact]
        public void Test_isAssignable()
        {
            //  Assert.IsAssignableFrom<expectedBaseType>(object);
            // If object is derived from expectedBaseType then test is successful.

            Assert.IsAssignableFrom<IEnumerable<int>>(new List<int>());    // Success
            Assert.IsAssignableFrom<Object>(5);        // Success

            Exception ex = new ArgumentNullException();

            Assert.IsAssignableFrom<Exception>(ex);               // Success
            Assert.IsAssignableFrom<ArgumentNullException>(ex);    // Success
            Assert.IsAssignableFrom<InvalidOperationException>(ex); // Error

            Assert.IsNotAssignableFrom<Exception>(ex);               // Error
            Assert.IsNotAssignableFrom<ArgumentNullException>(ex);    // Error
            Assert.IsNotAssignableFrom<InvalidOperationException>(ex); // Success
        }

        [Fact]
        public void Test_NullNotNull()
        {
            //  Assert.Null(object);

            Assert.Null(null);                   // Success
            Assert.Null(new Object());           // Error

            Assert.NotNull(null);                // Error
            Assert.NotNull(new Object());        // Success
        }

        [Fact]
        public void Test_SameNotSame()
        {
            //  Assert.Same(expectedObject, actualObject);
            //  Assert.NotSame(expectedObject, actualObject);
            object obj1 = new Object();
            object obj2 = obj1;
            object obj3 = new Object();

            Assert.Same(obj1, obj2);      // Success
            Assert.Same(obj1, obj3);      // Error

            Assert.NotSame(obj1, obj2);   // Error
            Assert.NotSame(obj1, obj3);   // Success
        }

        private void Fail()
        {
            //  Assert.Fail(message);

            Assert.Fail("This test fails unconditionally.");
        }





    }
}
