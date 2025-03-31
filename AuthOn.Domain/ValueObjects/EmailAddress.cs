using System.Text.RegularExpressions;

namespace AuthOn.Domain.ValueObjects
{
    public partial record EmailAddress
    {
        private const string Pattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        private EmailAddress(string value) => Value = value;

        public static EmailAddress? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !EmailAddressRegex().IsMatch(value))
            {
                return null;
            }

            return new EmailAddress(value);
        }

        public string Value { get; init; }

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailAddressRegex();
    }
}