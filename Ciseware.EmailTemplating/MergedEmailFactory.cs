using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ciseware.EmailTemplating
{
    /// <summary>
    /// Factory class for creating "mail-merged" MailMessage objects
    /// </summary>
    public class MergedEmailFactory
    {
        protected MailMessageWrapper _message;

        public MergedEmailFactory(ITemplateParser templateParser)
        {
            _message = new MailMessageWrapper(templateParser);
        }

        public MailMessageWrapper WithTokenValues(IDictionary<string, string> tokenValues)
        {            
            _message.TokenValues = tokenValues;
            return _message;
        }
    }
}
