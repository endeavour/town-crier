using System.Collections.Generic;

namespace Alpinely.TownCrier
{
    /// <summary>
    /// Factory class for creating "mail-merged" MailMessage objects
    /// </summary>
    public class MergedEmailFactory
    {
        protected MailMessageWrapper Message;

        public MergedEmailFactory(ITemplateParser templateParser)
        {
            Message = new MailMessageWrapper(templateParser);
        }

        public MailMessageWrapper WithTokenValues(IDictionary<string, string> tokenValues)
        {
            Message.TokenValues = tokenValues;
            return Message;
        }
    }
}