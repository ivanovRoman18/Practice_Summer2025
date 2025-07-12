using Xunit;
using task11; // Используем пространство имен из основного проекта

public class CalculatorTests
{
    private ICalculator _calculator;

    public CalculatorTests()
    {
        _calculator = DynamicCalculatorCreator.CreateCalculator();
    }

    [Fact]
    public void Add_ShouldReturnCorrectSum()
    {
        int a = 5;
        int b = 3;
        int expected = 8;

        int result = _calculator.Add(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Minus_ShouldReturnCorrectDifference()
    {
        int a = 5;
        int b = 3;
        int expected = 2;

        int result = _calculator.Minus(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Mul_ShouldReturnCorrectMul()
    {
        int a = 5;
        int b = 3;
        int expected = 15;

 
        int result = _calculator.Mul(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Div_ShouldReturnCorrectQuotient()
    {
        // Arrange
        int a = 6;
        int b = 3;
        int expected = 2;

        // Act
        int result = _calculator.Div(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Div_ByZero_ShouldThrowDivideByZeroException()
    {
        int a = 1;
        int b = 0;

        var exception = Assert.Throws<DivideByZeroException>(() => _calculator.Div(a, b));
        Assert.Contains("divide", exception.Message.ToLower());
    }
}

