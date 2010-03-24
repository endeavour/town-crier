using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Ciseware.EmailTemplating;
using NUnit.Framework;

namespace EmailTemplating.Tests
{
    [TestFixture]
    public class IntegrationTests
    {

        [Test]
        public void CanSendHtmlEmail()
        {
            var factory =new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string,string>()
            {{"name", "James"},
            {"userid", "123"}};

            var message = factory.CreateFromFile("Hello {%=name%}", @"templates\sample-email.html", tokenValues);

            var from = new MailAddress("info@ciseware.com", "Ciseware Automated Emailer");
            var to = new MailAddress("james@ciseware.com", "James Freiwirth");
            message.From = from;
            message.To.Add(to);
            message.IsBodyHtml = true;

            var smtpMailer = new SmtpClient("mail.ciseware.com", 25);
            smtpMailer.Send(message);
        }
    }
}
