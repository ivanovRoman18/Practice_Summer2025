namespace task11tests;

using Xunit;

public class DynamicCalculatorTests
{
    [Fact]
    public void TestSum()
    {
        dynamic calculator = DynamicCalculatorCreator.CreateCalculator();
        Assert.Equal(8, calculator.Add(5, 3));
    }

    [Fact]
    public void TestMinus()
    {
        dynamic calculator = DynamicCalculatorCreator.CreateCalculator();
        Assert.Equal(2, calculator.Minus(5, 3));
    }

    [Fact]
    public void TestMul()
    {
        dynamic calculator = DynamicCalculatorCreator.CreateCalculator();
        Assert.Equal(15, calculator.Mul(5, 3));
    }

    [Fact]
    public void TestDiv()
    {
        dynamic calculator = DynamicCalculatorCreator.CreateCalculator();
        Assert.Equal(2, calculator.Div(6, 3));
    }

    [Fact]
    public void TestClassStructure()
    {
        dynamic calculator = DynamicCalculatorCreator.CreateCalculator();

        Assert.Equal(8, calculator.Add(5, 3));
        Assert.Equal(2, calculator.Minus(5, 3));
        Assert.Equal(15, calculator.Mul(5, 3));
        Assert.Equal(2, calculator.Div(6, 3));
    }
}
