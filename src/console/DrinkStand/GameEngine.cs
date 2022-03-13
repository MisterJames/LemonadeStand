

// this class is used to process the current state of the 
// game and progress it forward. 

using System.Collections;
///===---   game engine   ---===///
public static class GameEngine
{
    private const int SignCost = 50;
    private const int TypicalRetailPrice = 50;
    private const int TypicalDailySales = 30;

    public static DailyResult ProcessDay(GameState state, PlayerProfile player, double weatherModifier, DailyChoices choices)
    {
        // the number of drinks sold depends on the weather, the price the player 
        // set for the drinks, and the number of signs the player chose to make
        var priceModifier = ComputePriceModifier(choices.DrinkPriceInCents);
        var signsModifier = ComputeSignsModifier(choices.SignsToMake);

        // our potential number of drinks sold is a result of typcial sales and our modifiers
        var potentialDrinksSold = (int)(TypicalDailySales * weatherModifier * priceModifier * signsModifier);

        // our ACTUAL drinks sold can't be more than we made!
        var actualDrinksSold = Math.Min(choices.DrinksToMake, potentialDrinksSold);
                
        var result = new DailyResult(choices)
        {
            DayNumber = player.PastDays.Count + 1,
            DrinkCosts = (player.PastDays.Count < 3 ? 15 : 20) * choices.DrinksToMake,
            SignCosts = choices.SignsToMake * SignCost,
            DrinksSold = actualDrinksSold
        };

        return result;
    }

    public static double ComputeSignsModifier(int signCount)
    {
        var signEffect = Math.Max(0.85F, 1 + Math.Log10(signCount));

        Console.WriteLine($"There were {signCount} signs with {signEffect} modifier.");

        return signEffect;
    }

    public static double ComputePriceModifier(int price)
    {
        // these are the values that we worked out to compute the
        // affect of the price on the amount of potential sales

        double priceAnchor = 200.0;    // a
        double sweetSpot = 40;         // b
        double salesDrift = -2;        // c
        double priceScale = 0.125;     // d
        double priceShift = 50;        // g/f

        double priceModifier = -(
            (   priceAnchor + 
                (sweetSpot * (price - priceShift)) +
                (salesDrift * Math.Pow(price - priceShift, 2)) +
                (priceScale * Math.Pow(price - priceShift, 3)) 
            ) / priceAnchor) + priceShift;

        priceModifier = Math.Min(100, priceModifier);
        priceModifier = Math.Max(0, priceModifier);

        Console.WriteLine($"Price of {price} left {priceModifier} modifier.");

        return priceModifier;
    }
     
}

