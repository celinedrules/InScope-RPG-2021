namespace Dialogue
{
    public static class Tools
    {
        public static int StringToInt(string s) => SafeConvert.ToInt(s);
        public static float StringToFloat(string s) => SafeConvert.ToFloat(s);
        public static bool StringToBool(string s) => (string.Compare(s, "True", System.StringComparison.OrdinalIgnoreCase) == 0);
    }
}