using Xunit;
using task14;
public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_LinearFunction_ReturnsZeroForSymmetricInterval() =>
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, x => x, 1e-4, 2), 1e-4);

    [Fact]
    public void Solve_SinFunction_ReturnsZeroForSymmetricInterval() =>
        Assert.Equal(0, DefiniteIntegral.Solve(-Math.PI, Math.PI, Math.Sin, 1e-5, 4), 1e-4);

    [Fact]
    public void QuadraticFunction_OneToFour_Returns21() =>
        Assert.Equal(21, DefiniteIntegral.Solve(1, 4, x => x * x, 1e-5, 4), 1e-3);
}
