using System.Text.RegularExpressions;

namespace AuthOn.Domain.ValueObjects
{
    public partial record UserName
    {
        private const string Pattern = @"^[a-zA-Z0-9 ]{4,20}$";

        private UserName(string value) => Value = value;

        public static UserName? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !UserNameRegex().IsMatch(value))
            {
                return null;
            }

            return new UserName(value);
        }

        public string Value { get; init; }

        [GeneratedRegex(Pattern)]
        private static partial Regex UserNameRegex();
    }
}