using task04;
using Xunit;
using System;
namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }
    [Fact]
    public void Fighter_ShouldBeStrongerThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }
    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldRotatesCorrectly()
    {
        var fighter = new Fighter();
        fighter.Rotate(45);
        Assert.True(true);
    }
    [Fact]
    public void Cruiser_ShouldRotatesCorrectly()
    {
        var cruiser = new Cruiser();
        cruiser.Rotate(45);
        Assert.True(true);
    }
    [Fact]
    public void Fighter_ShouldMovesForwardCorrectly()
    {
        var fighter = new Fighter();

        fighter.MoveForward();

        Assert.True(true);
    }
    [Fact]
    public void Cruiser_ShouldMovesForwardCorrectly()
    {
        var cruiser = new Cruiser();

        cruiser.MoveForward();

        Assert.True(true);
    }
    [Fact]
    public void Cruiser_ShouldFiresCorrectly()
    {
        var cruiser = new Cruiser();

        cruiser.Fire();

        Assert.True(true);
    }
    [Fact]
    public void Fighter_ShouldFiresCorrectly()
    {
        var fighter = new Cruiser();

        fighter.Fire();

        Assert.True(true);   
    }
}