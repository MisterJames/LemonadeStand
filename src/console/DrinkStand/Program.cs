// display our game banner and instructions
Console.Clear();
DisplayWelcome(GameEngine.GameName, GameEngine.TypicalRetailPrice);
WaitForEnterPress();
Console.Clear();

// start to initialize our game
var numPlayers = GetNumberOfPlayers();
var game = new GameState(numPlayers);
Console.WriteLine($"You are playing with {game.NumberOfPlayers} players.");
WaitForEnterPress();

// start the game loop!
while (game.ContinuePlaying)
{
    // get and display the weather so the user can decide what to do
    var todaysWeather = WeatherEngine.GenerateWeatherConditions();
    DisplayWeather(todaysWeather);
    WaitForEnterPress();

    // the weather has the same affect on all players, so we compute that first
    var weatherModifier = WeatherEngine.ComputeWeatherModifier(todaysWeather);

    // figure out how we did today and add it to our progress
    foreach (var player in game.PlayerProfiles)
    {
        // ask the user to make their choices for the day
        var todaysChoices = GetDailyUserChoices(player.PlayerNumber);
        DisplayDailyChoices(player, todaysChoices);    

        var todaysResults = GameEngine.ProcessDay(game, player, weatherModifier, todaysChoices);
        player.PastDays.Add(todaysResults);
    }
    WaitForEnterPress();

    // process the day's business for each player
    DisplayDailyResults(game);
    WaitForEnterPress();
    game.ContinuePlaying = ShouldContinue();

}


///===---   input functions   ---===///

// this function asks the user to select the number of players that will be playing
// and continues to ask them until they pick 1 or 2
int GetNumberOfPlayers(){
    int playerInputCount = 0;

    // we can only play with 1 or 2 players
    while (playerInputCount < 1 || playerInputCount > 2)
    {
        // this waits for the user to type something and press enter
        playerInputCount = GetNumberInputFromConsole("Will 1 or 2 people will be playing? ");

        // we have to check to make sure it's only 1 or 2 players
        if (playerInputCount < 1 || playerInputCount > 2)
        {
            // we'll give the user some feedback if they gave an invalid number
            Console.WriteLine("You can only pick 1 or 2 players, try again!");
            WaitForEnterPress();
        }
        
        Console.WriteLine(Environment.NewLine);
    }
    return playerInputCount;
}

// allow the user to make selections to set the cup count, price and sign count
DailyChoices GetDailyUserChoices(int playerNumber){

    Console.WriteLine($"It's time for player {playerNumber} to conduct business for the day!");

    var price = GetNumberInputFromConsole("How cents will you charge per glass? ");
    var numGlasses = GetNumberInputFromConsole("How may glasses will you make? ");
    var signs = GetNumberInputFromConsole("How many signs will you make? ");

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

// when we allow the users to type, they may not enter a number. this helper
// method allows us to get a number from them and repeatedly ask them for a
// number if they enter some other text
int GetNumberInputFromConsole(string prompt)
{
    int number = -1;

    // number is the starting value that we just set. this loop lets the user
    // type something in, and we will try to turn it into a number until the
    // text entered can be turned into a number
    while (number < 1)
    {
        // display the prompt...
        Console.WriteLine();
        Console.Write(prompt);

        // next, we let the user type...
        var userInput = Console.ReadLine();

        // ...but we don't know if they are letters, symbols, or numbers. we 
        // try to 'parse' the input into our number variable
        if(int.TryParse(userInput, out number) == false)
        {
            // the result of parsing was false, so we couldn't get a number
            Console.WriteLine("You didn't type a valid number!");
        }        
    }
    return number;
}

// when we need the user to choose yes or no they may type Y or yes or YES or nO 
// and so on. this method takes care of figuring out what they meant.
bool ShouldContinue()
{
    // try to be flexible for the user!
    var validYeses = new List<string>(){"Y", "YES", "YOU BET", "SURE"};
    var validNoes = new List<string>(){"N", "NO", "NOPE", "NO WAY"};

    var validChoice = false;
    bool userContinuing = true;

    while (!validChoice)
    {
        // prompt the user for input
        Console.Write("Would you like to keep playing (Yes or No)? ");
        var userChoice = Console.ReadLine() ?? string.Empty;

        if (validYeses.Contains(userChoice.ToUpper()))
        {
            validChoice = true;
            userContinuing = true;
        }

        if (validNoes.Contains(userChoice.ToUpper()))
        {
            validChoice = true;
            userContinuing = false;
        }

    }

    return userContinuing;

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

void DisplayDailyChoices(PlayerProfile player, DailyChoices choices)
{
    Console.WriteLine();
    Console.WriteLine($"DAILY BUSINESS for PLAYER {player.PlayerNumber}");
    Console.WriteLine("--------------------------");
    Console.WriteLine($"You have decided to charge {choices.DrinkPriceInCents} cents per glass.");
    Console.WriteLine($"You will be making {choices.SignsToMake} signs.");
    Console.WriteLine($"You will prepare {choices.DrinksToMake} drinks.");
    Console.WriteLine();
}

void DisplayDailyResults(GameState state)
{
    Console.Clear();

    foreach (var player in state.PlayerProfiles)
    {
        var results = player.PastDays.OrderByDescending(d=>d.DayNumber).First();

        Console.WriteLine();
        Console.WriteLine($"DAILY RESULTS for PLAYER {player.PlayerNumber} - DAY {results.DayNumber}");
        Console.WriteLine("--------------------------");
        Console.WriteLine($"You made {results.Input.DrinksToMake} drinks today and sold {results.DrinksSold}.");
        Console.WriteLine($"Your sales were a total of {results.GrossRevenue}, and your total costs were {results.TotalExpenses}.");
        Console.WriteLine($"At the end of the day you made {results.NetRevenue} and you now have {player.WalletBalance}.");
        Console.WriteLine();
        
    }
}

void DisplayWelcome(string gameName, int typicalPrice)
{
    var currentForegroudColor = Console.ForegroundColor;

    Console.WriteLine("--------------------------");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{Environment.NewLine}Welcome to {gameName}!{Environment.NewLine}");
    Console.ForegroundColor = currentForegroudColor;
    Console.WriteLine("--------------------------");

    Console.WriteLine();
    Console.WriteLine($"The game of {gameName} is quite simple. Each day you will set{Environment.NewLine}" +
        $"a price for your lemonade, make some signs and decide how {Environment.NewLine}" +
        $"many glasses you're going to make. Most people around here {Environment.NewLine}" +
        $"think that {typicalPrice} cents is a good price for a cup of lemonade.");

    Console.WriteLine();
    Console.WriteLine($"Be sure to check the weather forecast! The weather plays a {Environment.NewLine}" +
        $"big part in determining how many glasses of lemonade you will{Environment.NewLine}" +
        "sell. Just hope there isn't a thunderstorm!");

    Console.WriteLine();
    Console.WriteLine($"To get started, your parents were able to find a bag of {Environment.NewLine}" +
        $"sugar in the pantry, so your lemonade is cheaper for the {Environment.NewLine}" +
        "first three days!");

    Console.WriteLine();
    Console.WriteLine($"NOTE: One or two people can play, but your sales and {Environment.NewLine}" +
        $"advertising will not affect each other's sales.");

    Console.WriteLine();
    Console.WriteLine("--------------------------");

}

