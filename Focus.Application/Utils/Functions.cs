using System.Globalization;

namespace Focus.Application.Utils;

public static class Functions
{
    public static DateTime ParseAndConvertToUtc(string? dateString, string fieldName)
    {
        DateTime parsedDate;
        if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out parsedDate))
        {
            return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
        }
        if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            if (parsedDate.Kind == DateTimeKind.Unspecified)
            {
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(parsedDate, DateTimeKind.Local));
            }
            return parsedDate.ToUniversalTime();
        }
        throw new FormatException($"Formato de data inválido para o campo '{fieldName}': {dateString}.");
    }
}