namespace Docket.Client.Utilities
{
    public static class TextModification
    {
        public static string TextLimit(string input, int maxLength)
        {
            if (input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength) + "...";
        }
    }
}
