using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Ciseware.EmailTemplating
{
    /// <summary>
    /// Factory class for creating "mail-merged" MailMessage objects
    /// </summary>
    public class MergedEmailFactory
    {
        private MailMessageWrapper _message;

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

    public class MailMessageWrapper
    {
        private IDictionary<string, string>  _tokenValues;

        internal string PlainTextBody = null;
        internal string HtmlBody = null;
        internal bool IsSubjectSet = false;
        internal ITemplateParser TemplateParser;
        internal MailMessage ContainedMailMessage;
        internal IDictionary<string, string> TokenValues;

        public MailMessageWrapper(ITemplateParser templateParser)
        {            
            ContainedMailMessage = new MailMessage();
            TemplateParser = templateParser;            
        }

        
    }

    public static class MailMessageWrapperFluentExtensions
    {
        public static MailMessageWrapper WithSubject(this MailMessageWrapper message, string subjectTemplate)
        {
            if (message.IsSubjectSet)
                throw new InvalidOperationException("Subject has already been set");

            var _populatedSubject = message.TemplateParser.ReplaceTokens(subjectTemplate, message.TokenValues);
            message.ContainedMailMessage.Subject = _populatedSubject;
            message.IsSubjectSet = true;
            return message;
        }

        public static MailMessageWrapper WithHtmlBody(this MailMessageWrapper message, string bodyTemplate)
        {
            if (message.HtmlBody != null)
                throw new InvalidOperationException("An HTML body already exists");

            var _populatedBody = message.TemplateParser.ReplaceTokens(bodyTemplate, message.TokenValues);
            
            message.HtmlBody = _populatedBody;
            return message;
        }

        public static MailMessageWrapper WithPlainTextBody(this MailMessageWrapper message, string bodyTemplate)
        {
            if (message.PlainTextBody != null)
                throw new InvalidOperationException("A plaintext body already exists");

            var _populatedBody = message.TemplateParser.ReplaceTokens(bodyTemplate, message.TokenValues);

            message.PlainTextBody = _populatedBody;
            return message;
        }

        public static MailMessageWrapper WithHtmlBodyFromFile(this MailMessageWrapper message, string filename)
        {
            return message.WithHtmlBody(File.ReadAllText(filename));
        }

        public static MailMessageWrapper WithPlainTextBodyFromFile(this MailMessageWrapper message, string filename)
        {
            return message.WithPlainTextBody(File.ReadAllText(filename));
        }

        public static MailMessage Create(this MailMessageWrapper message)
        {
            if (message.HtmlBody != null && message.PlainTextBody != null)
            {
                message.SetBodyFromPlainText();
                var htmlAlternative = AlternateView.CreateAlternateViewFromString(message.HtmlBody, null, MediaTypeNames.Text.Html);
                message.ContainedMailMessage.AlternateViews.Add(htmlAlternative);
            }
            else
            {
                if (message.HtmlBody != null)
                    message.SetBodyFromHtmlText();
                else if (message.PlainTextBody != null)
                    message.SetBodyFromPlainText();
            }
             
            return message.ContainedMailMessage;
        }

        private static void SetBodyFromPlainText(this MailMessageWrapper message)
        {
            message.ContainedMailMessage.Body = message.PlainTextBody;
            message.ContainedMailMessage.IsBodyHtml = false;
        }

        private static void SetBodyFromHtmlText(this MailMessageWrapper message)
        {
            message.ContainedMailMessage.Body = message.HtmlBody;
            message.ContainedMailMessage.IsBodyHtml = true;
        }

    }
}
