using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Ciseware.EmailTemplating
{
    /// <summary>
    /// Factory class for creating "mail-merged" MailMessage objects
    /// </summary>
    public class MergedEmailFactory
    {
        private TemplateParser _templateParser;

        public MergedEmailFactory(TemplateParser templateParser)
        {
            _templateParser = templateParser;
        }

        /// <summary>
        /// Create a merged mail message with the supplied subject and body templates
        /// </summary>
        /// <param name="subjectTemplate"></param>
        /// <param name="bodyTemplate"></param>
        /// <param name="tokenValues"></param>
        /// <returns></returns>
        public MailMessage Create(string subjectTemplate, string bodyTemplate, IDictionary<string, string> tokenValues)
        {
            MailMessage mailMessage = new MailMessage();
            
            var populatedSubject = _templateParser.ReplaceTokens(subjectTemplate, tokenValues);
            var populatedBody = _templateParser.ReplaceTokens(bodyTemplate, tokenValues);

            mailMessage.Subject = populatedSubject;
            mailMessage.Body = populatedBody;
            
            return mailMessage;
        }

        /// <summary>
        /// Create a merged mail message with the supplied subject template and the message template read from the given path
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="bodyTemplatePath"></param>
        /// <param name="tokenValues"></param>
        /// <returns></returns>
        public MailMessage CreateFromFile(string subject, string bodyTemplatePath, IDictionary<string, string>  tokenValues)
        {
            var bodyTemplate = File.ReadAllText(bodyTemplatePath);
            return Create(subject, bodyTemplate, tokenValues);
        }
    }
}
