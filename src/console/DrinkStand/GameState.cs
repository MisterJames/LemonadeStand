public class GameState
{
    public GameState(int numPlayers)
    {
        // by default, the game will go more than one day
        ContinuePlaying = true;

        // keep track of the progress for each player in the game
        PlayerProfiles = new List<PlayerProfile>();
        for (int i = 0; i < numPlayers; i++)
        {
            PlayerProfiles.Add(new PlayerProfile(i + 1));
        }
    }

    public int NumberOfPlayers => PlayerProfiles.Count;
    public List<PlayerProfile> PlayerProfiles { get; init; }
    public bool ContinuePlaying { get; set; } 
}

public class PlayerProfile
{
    public PlayerProfile(int playerNumber)
    {        
        PlayerNumber = playerNumber;
        PastDays = new List<DailyResult>();        
    }

    private const int StartingBalance = 1000;
    public int PlayerNumber { get; set; }
    public List<DailyResult> PastDays { get; set; }
    public int WalletBalance => StartingBalance + PastDays.Sum(d => d.NetRevenue);

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
    CoolAndCold,
    Thunderstorms
}