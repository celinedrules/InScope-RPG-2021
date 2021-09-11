namespace Dialogue
{
    public static class SafeConvert
    {
        public static int ToInt(string s) => int.TryParse(s, out var result) ? result : 0;

        public static float ToFloat(string s) => float.TryParse(s, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var result)
            ? result
            : 0;
    }
}