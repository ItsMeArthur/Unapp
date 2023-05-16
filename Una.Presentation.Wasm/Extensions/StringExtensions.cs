namespace Una.Presentation.Wasm.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string password, char maskCharacter = '*')
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            return new string(maskCharacter, password.Length);
        }
    }
}
