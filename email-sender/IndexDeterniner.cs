using System;

class IndexDeterminer
{
    public static int index()
    {
       // Get the current UTC time
        DateTime utcNow = DateTime.UtcNow;

        // Define a valid time zone (UTC+11 for Australia)
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

        // Convert the UTC time to the desired timezone
        DateTime currentTimeInUTCPlus11 = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);

        // Get the hour and minute
        int hour = currentTimeInUTCPlus11.Hour;
        int minute = currentTimeInUTCPlus11.Minute;
        Console.WriteLine($"Current time: {hour:D2}:{minute:D2}");
        int i = (hour-9) * 6 + minute/10;
        Console.WriteLine(i);
        return  i;
    }
}
