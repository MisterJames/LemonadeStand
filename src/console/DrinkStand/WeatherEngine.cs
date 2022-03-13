public static class WeatherEngine
{
    public static Weather GenerateWeatherConditions()
    {
        var rnd = new Random(Environment.TickCount);

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

        modifier = Math.Max(modifier, 0.15);

        Console.WriteLine($"Weather was {weather} with {modifier} modifier.");

        return modifier;
    }
}
