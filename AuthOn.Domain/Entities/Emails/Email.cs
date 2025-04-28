using AuthOn.Domain.Entities.EmailStates;
using AuthOn.Domain.ValueObjects;

namespace AuthOn.Domain.Entities.Emails
{
    public sealed class Email
    {
        #region Properties

        public long Id { get; private set; }
        public EmailAddress DestinationEmail { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }
        public byte EmailStateId { get; private set; }
        public bool Visualized { get; private set; }
        public DateTime RecordCreationMoment { get; private set; }
        public DateTime RecordUpdateMoment { get; private set; }

        #endregion

        #region Navigation Properties

        public EmailState? EmailState { get; private set; }

        #endregion

        #region Constructors

        private Email(EmailAddress destinationEmail, string subject, string message)
        {
            DestinationEmail = destinationEmail;
            Subject = subject;
            Message = message;
            EmailStateId = EmailState.Pending.Id;
            Visualized = false;
            RecordCreationMoment = DateTime.UtcNow;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        public Email() { }

        #endregion

        #region Methods

        public static Email Create(EmailAddress destinationEmail, string subject, string message)
        {
            return new Email(destinationEmail, subject, message);
        }

        public void MarkAsVisualized()
        {
            Visualized = true;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        public void UpdateMessage(string message)
        {
            Message = message;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        public void UpdateStateFailed()
        {
            EmailStateId = EmailState.Failed.Id;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        public void UpdateStateSent()
        {
            EmailStateId = EmailState.Sent.Id;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        #endregion
    }
}