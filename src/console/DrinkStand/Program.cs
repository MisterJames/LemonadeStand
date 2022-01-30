// display our game banner
Console.Clear();
Console.WriteLine($"{Environment.NewLine}Welcome to Drink Stand!{Environment.NewLine}");

// start to initialize our game
var game = new GameOptions();
game.NumberOfPlayers = GetNumberOfPlayers();
Console.WriteLine($"You are playing with {game.NumberOfPlayers} players.");
WaitForEnterPress();


///===---   input functions    ---===///

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

// this method simply prompts the user to press the enter key 
// it's just a convenience that saves us a bit of typing!
void WaitForEnterPress()
{
    Console.WriteLine("(Press ENTER to continue)");
    Console.ReadLine();
}

