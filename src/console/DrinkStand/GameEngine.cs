
///===---   game engine   ---===///

// this class is used to process the current state of the 
// game and progress it forward. 

public static class GameEngine
{
    private const int SignCost = 50;

    public static Weather GenerateWeatherConditions(){
        return Weather.Sunny;
    }

    public static DailyResult ProcessDay(GameState state, DailyChoices choices)
    {

        var drinksSold = 30;

        var result = new DailyResult(choices)
        {
            DayNumber = state.PastDays.Count + 1,
            DrinkCosts = (state.CurrentDay < 3 ? 15 : 20) * choices.DrinksToMake ,
            SignCosts = choices.SignsToMake * SignCost,
            DrinksSold = drinksSold
        };

        return result;
    }

}