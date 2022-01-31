public class GameState
{
    private const int StartingBalance = 1000;

    public GameState()
    {
        PastDays = new List<DailyResult>();
    }

    public int NumberOfPlayers { get; set; }

    public List<DailyResult> PastDays { get; set; }
    public int WalletBalance => StartingBalance + PastDays.Sum(d => d.NetRevenue);

    public int CurrentDay { get; set; }
}

public class DailyActvity
{
    public DailyActvity(DailyChoices userChoices, Weather weaterConditions)
    {
        UserChoices = userChoices;
        WeatherConditions = weaterConditions;
    }
    public Weather WeatherConditions { get; init; }
    public DailyChoices UserChoices { get; init; }

}

public class DailyChoices
{
    public int DrinkPriceInCents { get; set; }
    public int DrinksToMake { get; set; }
    public int SignsToMake { get; set; }
    
}

public class DailyResult
{
    public DailyResult(DailyChoices input)
    {
        Input = input;
    }
    public DailyChoices Input { get; init; }
    public int DayNumber { get; set; }
    public int DrinkCosts { get; set; }
    public int SignCosts { get; set; }
    public int DrinksSold { get; set; }
    public int TotalExpenses => DrinkCosts + SignCosts;
    public int GrossRevenue => DrinksSold * Input.DrinkPriceInCents;
    public int NetRevenue => GrossRevenue - TotalExpenses;
}

public enum Weather
{
    Sunny,
    HotAndDry,
    Cloudy,
    Thunderstorms
}