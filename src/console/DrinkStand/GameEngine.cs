

// this class is used to process the current state of the 
// game and progress it forward. 

using System.Collections;
///===---   game engine   ---===///
public static class GameEngine
{
    public const int NormalDrinkCost = 20;
    public const double FreeSugarDiscount = 0.25;
    private const int SignCost = 50;
    private const int TypicalRetailPrice = 50;
    private const int TypicalDailySales = 50;

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
            DrinkCosts = GetCurrentDrinkCost(player.PastDays.Count)  * choices.DrinksToMake,
            SignCosts = choices.SignsToMake * SignCost,
            DrinksSold = actualDrinksSold
        };

        return result;
    }

    public static int GetCurrentDrinkCost(int daysPlayed)
    {
        // the ?: operator is called a ternary conditional modifier.
        // it's a fancy way of saying "if...then...else". the first
        // thing we do is check a boolean value. if that evaluates
        // to 'true' then we do whatever comes after the question mark.
        // otherwise, the value is set to what comes after the colon.

        // in this case, the player gets a discount on the first three
        // days because Mom and Dad were giving them free sugar. 

        var result = daysPlayed < 3 
            ? NormalDrinkCost - (NormalDrinkCost * FreeSugarDiscount) 
            : NormalDrinkCost;

        return (int)result; 

    }

    public static double RandomizeModifier(double modifier)
    {
        var rnd = new Random();

        // 'NextDouble' returns a number between 0 and 1. by 
        // subtracting '0.5' from it we end up with a number
        // that is between -0.5 and 0.5 so that the affect of
        // the randomness can be positive or negative. then,
        // we reduce the amount of it by dividing it by 3 so
        // that the randomness is really just a little 'wiggle'
        // and we don't always have the same amount.
        var randomness = (double)(rnd.NextDouble() - 0.5) / 3;

        return modifier + randomness;

    }

    // players should make at least one sign, so that potential
    // patrons know about the stand. making too many won't help,
    // though, so we don't overly reward them for making too many.
    // this is a simple logrithmic computation.
    public static double ComputeSignsModifier(double signCount)
    {
        // the player is slightly penalized if they don't make signs
        if (signCount < 1) return 0.85;

        // there is no benefit to making too many signs
        if (signCount > 10) return 2;

        var signEffect = Math.Log10(signCount / 10) + 2;

        Console.WriteLine($"There were {signCount} signs with {signEffect} modifier.");

        return signEffect;
    }

    // there is a bit of a curve to the reward and penalty with 
    // setting a price. too low, and you'll sell all your lemonade
    // but you won't make much money. set it too high and you'll 
    // end up scaring potential customers away! find the sweet spot!
    public static double ComputePriceModifier(int price)
    {
        // these are the values that we worked out to compute the
        // affect of the price on the amount of potential sales.
        // see the values in the design and planning folder

        double priceAnchor = 200.0;                           // a
        double sweetSpot = 40;                                // b
        double salesDrift = -2;                               // c
        double priceScale = 0.125;                            // d
        double typicalPrice = GameEngine.TypicalRetailPrice;  // g

        double priceModifier = -(
            (   priceAnchor + 
                (sweetSpot * (price - typicalPrice)) +
                (salesDrift * Math.Pow(price - typicalPrice, 2)) +
                (priceScale * Math.Pow(price - typicalPrice, 3)) 
            ) / priceAnchor) + typicalPrice;

        // this limits the range from 0-100
        priceModifier = Math.Min(100, priceModifier);
        priceModifier = Math.Max(0, priceModifier);

        // this generates a number that we can use as a multiplier
        priceModifier = (priceModifier / 100) + 1;

        return priceModifier;
    }
     
}

