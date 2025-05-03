using AuthOn.Domain.Entities.Emails;

namespace AuthOn.Domain.Entities.EmailStates
{
    public sealed class EmailState
    {
        #region Properties

        public byte Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        #endregion

        #region Navigation Properties

        public List<Email> Emails { get; private set; } = [];

        #endregion

        #region Static Properties

        public static EmailState Pending => new(1, "PENDING");
        public static EmailState Sent => new(2, "SENT");
        public static EmailState Failed => new(3, "FAILED");

        #endregion

        #region Constructors

        public EmailState() { }

        public EmailState(byte id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #endregion
    }
}