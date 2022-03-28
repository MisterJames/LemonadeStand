public static class WeatherEngine
{
    public static Weather GenerateWeatherConditions()
    {
        var rnd = new Random(Environment.TickCount);

        // this is a good reason to watch the IntelliSense when
        // you are coding! when we call 'Next()' on 'Random' the
        // first number is the INCLUSIVE lower bounds, meaning
        // the LOWEST number that can be randomly selected. the
        // second parameter, however, is the EXCLUSIVE uppwer bounds
        // meaning it CAN'T be that high. so, this is how to 
        // pick a random number between 1 and 100 that includes
        // both 1 and 100. 
        var randomWeatherConditions = rnd.Next(1, 101);


        if (randomWeatherConditions > 95)
            return Weather.Thunderstorms;

        if (randomWeatherConditions > 85)
            return Weather.HotAndDry;

        if (randomWeatherConditions > 35)
            return Weather.Sunny;

        if (randomWeatherConditions > 10)
            return Weather.Cloudy;

        return Weather.CoolAndCold;
    }

    // weather will affect the sales within a range. even though
    // it is sunny, it may be cooler, or windy, or humid. to 
    // reflect that, we add a bit of randomness to the modifier
    // rather than just using a straight formula
    public static double ComputeWeatherModifier(Weather weather)
    {
        var modifier = 1.0;
        var rnd = new Random();
        var randomness = (float)(rnd.NextDouble() - 0.5) / 2;

        switch (weather)
        {
            case Weather.Sunny:
                modifier = 0.9 + randomness;
                break;
            case Weather.HotAndDry:
                modifier = 1.35 + randomness;
                break;
            case Weather.Cloudy:
                modifier = .7 + randomness;
                break;
            case Weather.CoolAndCold:
                modifier = .3 + randomness;
                break;
            case Weather.Thunderstorms:
                modifier = 0 + randomness;
                break;
            default:
                break;
        }

        // we can't go below 0...no refunds, people!
        modifier = Math.Max(modifier, 0);

        Console.WriteLine($"Weather was {weather} with {modifier} modifier.");

        return modifier;
    }
}
