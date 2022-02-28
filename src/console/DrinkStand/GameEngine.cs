
///===---   game engine   ---===///

// this class is used to process the current state of the 
// game and progress it forward. 

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
        var priceShift = 50;
        var sweetSpot = 40;
        var salesDrift = -2;
        var priceScale = 0.124;
        var priceAnchor = 200;

        var effect = Math.Log(0.25) * price + 100;
        var modifier = (Math.Max(effect, 2.5) / 100);

        Console.WriteLine($"Price of {price} left {modifier} modifier.");

        return modifier;
    }

}