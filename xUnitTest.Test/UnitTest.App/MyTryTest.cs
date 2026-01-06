using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.App;

namespace xUnitTest.Test.UnitTest.App
{
    public class MyTryTest
    {
        private readonly Mock<ICalculatorService> _mockCalculatorService;

        public MyTryTest()
        {
            _mockCalculatorService = new Mock<ICalculatorService>();
        }

        [Theory]
        [InlineData(7, 3, 10)]
        public void Sum_SimpleValues_ReturnSumValue(int number1, int number2, int expected)
        {
            // Arrange
            _mockCalculatorService.Setup(service => service.Sum(number1, number2)).Returns(expected);

            // Act
            int result = _mockCalculatorService.Object.Sum(number1, number2);

            // Assert
            Assert.Equal<int>(expected, result);
        }

        [Theory]
        [InlineData(7, 3, 10)]
        public void Sum_MockTest1(int number1, int number2, int expected)
        {
            // Arrange
            //int actualSum = 0;
            //_mockCalculatorService.Setup(service => service.Sum(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((a, b) => actualSum = a + b).Returns(actualSum); // This line causes issue because actualSum is 0 at the time of setup

            //_mockCalculatorService.Setup(service => service.Sum(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((a, b) => actualSum = a + b).Returns(() => actualSum);

            _mockCalculatorService.Setup(service => service.Sum(It.IsAny<int>(), It.IsAny<int>())).Returns((int a, int b) => a + b);

            // Act
            int result = _mockCalculatorService.Object.Sum(number1, number2);

            // Assert
            Assert.Equal<int>(expected, result);
        }

        [Theory]
        [InlineData(7, 3, 10)]
        public void Sum_MockTest(int number1, int number2, int expected)
        {
            // Arrange
            _mockCalculatorService.Setup(service => service.Sum(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) => a + b);

            // Act
            var result = _mockCalculatorService.Object.Sum(number1, number2);

            // Assert
            Assert.Equal<int>(expected, result);
        }
    }
}
