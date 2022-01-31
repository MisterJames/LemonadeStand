// display our game banner
Console.WriteLine($"{Environment.NewLine}Welcome to Drink Stand!{Environment.NewLine}");
WaitForEnterPress();
Console.Clear();

// start to initialize our game
var game = new GameState();
game.NumberOfPlayers = GetNumberOfPlayers();
Console.WriteLine($"You are playing with {game.NumberOfPlayers} players.");
WaitForEnterPress();

// get and display the weather so the user can decide what to do
var todaysWeather = GameEngine.GenerateWeatherConditions();
DisplayWeather(todaysWeather);
WaitForEnterPress();

// ask the user to make their choices for the day
var todaysChoices = GetDailyUserChoices();
DisplayDailyChoices(todaysChoices);
WaitForEnterPress();

// figure out how we did today and add it to our progress
var todaysResults = GameEngine.ProcessDay(game, todaysChoices);
game.PastDays.Add(todaysResults);
DisplayDailyResults(game);
WaitForEnterPress();


///===---   input functions   ---===///

// this function asks the user to select the number of players that will be playing
// and continues to ask them until they pick 1 or 2
int GetNumberOfPlayers(){
    int playerInputCount = 0;

    // we can only play with 1 or 2 players
    while (playerInputCount < 1 || playerInputCount > 2)
    {
        // write the instructions on the screen
        Console.WriteLine("Will 1 or 2 people will be playing? Type a number and press ENTER.");
        
        // this waits for the user to type something and press enter
        var numPlayers = Console.ReadLine();

        // the user typed in some characters, but we don't know if they are letters,
        // symbols, or numbers. we try to 'parse' the number out
        if(int.TryParse(numPlayers, out playerInputCount) == false)
        {
            // the result of parsing was false, so we couldn't get a number
            Console.WriteLine("You didn't type a number!");
            WaitForEnterPress();
        }
        // if they DID type a number, we have to check to make sure it's only 1 or 2
        else if (playerInputCount < 1 || playerInputCount > 2)
        {
            // we'll give the user some feedback if they gave an invalid number
            Console.WriteLine("You can only pick 1 or 2 players, try again!");
            WaitForEnterPress();
        }
        
        Console.Clear();
        Console.WriteLine(Environment.NewLine);
    }
    return playerInputCount;
}

// allow the user to make selections to set the cup count, price and sign count
DailyChoices GetDailyUserChoices(){

    var price = 20;
    var numGlasses = 30;
    var signs = 10;

    return new DailyChoices{ 
        DrinkPriceInCents = price, 
        DrinksToMake = numGlasses, 
        SignsToMake = signs
    };

}

// this method simply prompts the user to press the enter key 
// it's just a convenience that saves us a bit of typing!
void WaitForEnterPress()
{
    Console.WriteLine("(Press ENTER to continue)");
    Console.ReadLine();
}


///===---   display functions   ---===///
void DisplayWeather(Weather weather)
{
    var weatherStatement = string.Empty;

    switch (weather)
    {
        case Weather.Sunny:
            weatherStatement = "sunny";
            break;
        case Weather.HotAndDry:
            weatherStatement = "hot and dry";
        break;
        case Weather.Cloudy:
            weatherStatement = "cloudy";
        break;
        case Weather.Thunderstorms:
            weatherStatement = "thunderstorming";
        break;
    }

    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("LEMONSVILLE WEATHER REPORT");
    Console.WriteLine("--------------------------");
    Console.WriteLine($"Today will be {weatherStatement}!");
    Console.WriteLine();    

}

void DisplayDailyChoices(DailyChoices choices)
{
    Console.WriteLine();
    Console.WriteLine("DAILY BUSINESS");
    Console.WriteLine("--------------------------");
    Console.WriteLine($"You have decided to charge {choices.DrinkPriceInCents} cents per glass.");
    Console.WriteLine($"You will be making {choices.SignsToMake} signs.");
    Console.WriteLine($"You will prepare {choices.DrinksToMake} drinks.");
    Console.WriteLine();
}

void DisplayDailyResults(GameState state)
{
    var results = state.PastDays.OrderByDescending(d=>d.DayNumber).First();

    Console.Clear();
    Console.WriteLine();
    Console.WriteLine($"DAILY RESULTS - DAY {results.DayNumber}");
    Console.WriteLine("--------------------------");
    Console.WriteLine($"You made {results.Input.DrinksToMake} drinks today and sold {results.DrinksSold}.");
    Console.WriteLine($"Your sales were a total of {results.GrossRevenue}, and your total costs were {results.TotalExpenses}.");
    Console.WriteLine($"At the end of the day you made {results.NetRevenue} and you now have {state.WalletBalance}.");
    Console.WriteLine();
}