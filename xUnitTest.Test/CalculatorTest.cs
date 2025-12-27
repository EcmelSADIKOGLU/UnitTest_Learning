using UnitTest.App;

namespace xUnitTest.Test
{
    public class CalculatorTest
    {
        [Fact]
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
    }
}
