using System.Text.RegularExpressions;

namespace AuthOn.Domain.ValueObjects
{
    public partial record Password
    {
        private const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

        public string Value { get; }

        private Password(string value) => Value = value;

        public static Password? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !PasswordRegex().IsMatch(value))
            {
                return null;
            }

            return new Password(value);
        }

        [GeneratedRegex(Pattern)]
        private static partial Regex PasswordRegex();
    }
}