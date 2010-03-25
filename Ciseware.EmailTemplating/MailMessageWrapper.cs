using System.Collections.Generic;
using System.Net.Mail;

namespace Ciseware.EmailTemplating
{
    public class MailMessageWrapper
    {
        internal MailMessage ContainedMailMessage;
        internal string HtmlBody;
        internal bool IsSubjectSet;
        internal string PlainTextBody;
        internal ITemplateParser TemplateParser;
        internal IDictionary<string, string> TokenValues;

        public MailMessageWrapper(ITemplateParser templateParser)
        {
            ContainedMailMessage = new MailMessage();
            TemplateParser = templateParser;
        }
    }
}