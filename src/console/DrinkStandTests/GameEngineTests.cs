using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace DrinkStandTests
{
    public class GameEngineTests
    {
        [Theory]
        [ClassData(typeof(PriceModifierTestData))]
        public void ExpectedValuesShouldBeCalculated(double expected, int price)
        {
            // price 50 = sales 49
            // price 46 = sales 50

            Assert.Equal(expected, GameEngine.ComputePriceModifier(price));

        }
    }
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
        yield return new object[] { 100, 1 };
        yield return new object[] { 100, 13 };
        yield return new object[] { 80.875, 20 };
        yield return new object[] { 50, 46 };
        yield return new object[] { 62, 30 };
        yield return new object[] { 35.125, 80 };
        yield return new object[] { 0.125, 96 };
        yield return new object[] { 0, 100 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}