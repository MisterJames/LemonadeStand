
///===---   game engine   ---===///

// this class is used to process the current state of the 
// game and progress it forward. 

public static class GameEngine
{
    private const int SignCost = 50;
    private const int TypicalRetailPrice = 50;

    public static Weather GenerateWeatherConditions(){
        return Weather.Sunny;
    }

    public static DailyResult ProcessDay(GameState state, PlayerProfile player, Weather weather, DailyChoices choices)
    {
        // the number of drinks sold depends on the weather, the price
        // the person chose, and the number of signs made
        var drinksSold = 30;
                
        var result = new DailyResult(choices)
        {
            DayNumber = player.PastDays.Count + 1,
            DrinkCosts = (player.PastDays.Count < 3 ? 15 : 20) * choices.DrinksToMake,
            SignCosts = choices.SignsToMake * SignCost,
            DrinksSold = drinksSold
        };

        return result;
    }

}