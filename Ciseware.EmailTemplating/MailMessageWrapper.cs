using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

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

        public MailMessageWrapper WithSubject(string subjectTemplate)
        {
            if (IsSubjectSet)
                throw new InvalidOperationException("Subject has already been set");

            string _populatedSubject = TemplateParser.ReplaceTokens(subjectTemplate, TokenValues);
            ContainedMailMessage.Subject = _populatedSubject;
            IsSubjectSet = true;
            return this;
        }

        public MailMessageWrapper WithHtmlBody(string bodyTemplate)
        {
            if (HtmlBody != null)
                throw new InvalidOperationException("An HTML body already exists");

            string _populatedBody = TemplateParser.ReplaceTokens(bodyTemplate, TokenValues);

            HtmlBody = _populatedBody;
            return this;
        }

        public MailMessageWrapper WithPlainTextBody(string bodyTemplate)
        {
            if (PlainTextBody != null)
                throw new InvalidOperationException("A plaintext body already exists");

            string _populatedBody = TemplateParser.ReplaceTokens(bodyTemplate, TokenValues);

            PlainTextBody = _populatedBody;
            return this;
        }

        public MailMessageWrapper WithHtmlBodyFromFile(string filename)
        {
            return WithHtmlBody(File.ReadAllText(filename));
        }

        public MailMessageWrapper WithPlainTextBodyFromFile(string filename)
        {
            return WithPlainTextBody(File.ReadAllText(filename));
        }

        public MailMessage Create()
        {
            if (HtmlBody != null && PlainTextBody != null)
            {
                SetBodyFromPlainText();
                AlternateView htmlAlternative = AlternateView.CreateAlternateViewFromString(HtmlBody, null, MediaTypeNames.Text.Html);
                ContainedMailMessage.AlternateViews.Add(htmlAlternative);
            }
            else
            {
                if (HtmlBody != null)
                    SetBodyFromHtmlText();
                else if (PlainTextBody != null)
                    SetBodyFromPlainText();
            }

            return ContainedMailMessage;
        }

        private void SetBodyFromPlainText()
        {
            ContainedMailMessage.Body = PlainTextBody;
            ContainedMailMessage.IsBodyHtml = false;
        }

        private void SetBodyFromHtmlText()
        {
            ContainedMailMessage.Body = HtmlBody;
            ContainedMailMessage.IsBodyHtml = true;
        }
    }
}