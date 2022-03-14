using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace DrinkStandTests
{
    public class GameEngineTestsShouldComputeExpected
    {
        [Theory]
        [ClassData(typeof(PriceModifierTestData))]
        public void PriceModifiers(double expected, int price)
        {

            Assert.Equal(expected, GameEngine.ComputePriceModifier(price), 2);

        }

        [Theory]
        [ClassData(typeof(SignModifierTestData))]
        public void SignModifiers(double expected, double signCount)
        {
            Assert.Equal(expected, GameEngine.ComputeSignsModifier(signCount), 2);
        }

        [Fact]
        public void ToleratedRandomRange()
        {
            // testing random is hard, but 
            for (double i = 0; i < 100; i++)
            {
                Assert.InRange(
                    GameEngine.RandomizeModifier(i), 
                    i - 0.33, 
                    i + 0.33);
            }
        }

        [Fact]
        public void DiscountedPrice()
        {
            // we get a discount on the first three days
            Assert.Equal(
                GameEngine.NormalDrinkCost - (GameEngine.NormalDrinkCost * GameEngine.FreeSugarDiscount), 
                GameEngine.GetCurrentDrinkCost(2));
        }

        [Fact]
        public void RegularPrice()
        {
            // we get a discount on the first three days
            Assert.Equal(
                GameEngine.NormalDrinkCost, 
                GameEngine.GetCurrentDrinkCost(4));
        }

    }
}

public class SignModifierTestData : IEnumerable<object[]>
{
    // to get these values, I used the desmos calculator
    // to graph the function, then pulled "easy" values out
    // so that I could write the function against them
    //
    // https://www.desmos.com/calculator/kl9uxpviwz

    public IEnumerator<object[]> GetEnumerator()
    {
        // expected, price
        yield return new object[] { .85, 0 };
        yield return new object[] { 1, 1 };
        yield return new object[] { 1.7, 5 };
        yield return new object[] { 2, 10 };
        yield return new object[] { 2, 11 };
        yield return new object[] { 2, 100 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PriceModifierTestData : IEnumerable<object[]>
{
    // to get these values, I used the desmos calculator
    // to graph the function, then pulled "easy" values out
    // so that I could write the function against them
    //
    // https://www.desmos.com/calculator/z28irg6zbn

    public IEnumerator<object[]> GetEnumerator()
    {
        // expected, price
        yield return new object[] { 2, 1 };
        yield return new object[] { 2, 13 };
        yield return new object[] { 1.80875, 20 };
        yield return new object[] { 1.50, 46 };
        yield return new object[] { 1.62, 30 };
        yield return new object[] { 1.35125, 80 };
        yield return new object[] { 1.00125, 96 };
        yield return new object[] { 1, 100 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}