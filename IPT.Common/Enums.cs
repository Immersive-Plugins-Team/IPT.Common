namespace IPT.Common
{
#pragma warning disable 1591, SA1602
    /// <summary>
    /// Represents the player's duty status.
    /// </summary>
    public enum PlayerStatus
    {
        Available,
        Busy,
        Emergency,
        EnRoute,
        Investigating,
        MealBreak,
        OnPatrol,
        OnScene,
        OutOfService,
        Pursuit,
        ReturnToStation,
        TrafficStop,
    }

    /// <summary>
    /// Represents a weather type.
    /// </summary>
    public enum EWeatherType
    {
        Blizzard = 669657108,
        Christmas = -1429616491,
        Clear = 916995460,
        Clearing = 1840358669,
        Clouds = 821931868,
        ExtraSunny = -1750463879,
        Foggy = -1368164796,
        Halloween = -921030142,
        LightSnow = 603685163,
        Neutral = -1530260698,
        Overcast = -1148613331,
        Rain = 1420204096,
        Smog = 282916021,
        Snow = -273223690,
        Thunder = -1233681761,
    }
#pragma warning restore 1591, SA1602
}
