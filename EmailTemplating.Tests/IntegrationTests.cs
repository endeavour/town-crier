using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Ciseware.EmailTemplating;
using NUnit.Framework;

namespace Ciseware.EmailTemplating.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        const string emailOutputDirectory = @"C:\temp\sampleemails";
        private SmtpClient _smtpClient;

        [TestFixtureSetUp]
        public void Init()
        {
            // Set up an SMTP client to save emails to files rather than actually sending them
            _smtpClient = new SmtpClient();
            _smtpClient.PickupDirectoryLocation = emailOutputDirectory;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

            Directory.CreateDirectory(_smtpClient.PickupDirectoryLocation);

            Console.WriteLine("Email pick up location: " + _smtpClient.PickupDirectoryLocation);
        }

        [Test]
        public void CanSendHtmlEmail()
        {
            var factory =new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string,string>()
            {{"name", "James"},
            {"userid", "123"}};

            var message = factory
                .WithTokenValues(tokenValues)
                .WithHtmlBodyFromFile(@"templates\sample-email.html")
                .Create();

            var from = new MailAddress("robot@test.com", "Automated Emailer");
            var to = new MailAddress("joe@bloggs.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            _smtpClient.Send(message);   
        
            // Would be nice to read in the output file here and check it is correct but - somewhat annoyingly - there isn't a nice
            // way to retrieve the filename of the saved file without using reflection: http://www.codeproject.com/KB/IP/smtpclientext.aspx
        }

        [Test]
        public void CanSendPlainEmail()
        {
            var factory =new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string,string>()
            {{"name", "James"},
            {"userid", "123"}};

            var message = factory
                .WithTokenValues(tokenValues)
                .WithPlainTextBodyFromFile(@"templates\sample-email.txt")
                .Create();

            var from = new MailAddress("robot@test.com", "Automated Emailer");
            var to = new MailAddress("joe@bloggs.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            _smtpClient.Send(message);   
        
            // Would be nice to read in the output file here and check it is correct but - somewhat annoyingly - there isn't a nice
            // way to retrieve the filename of the saved file without using reflection: http://www.codeproject.com/KB/IP/smtpclientext.aspx
        }

        [Test]
        public void CanSendHtmlEmailWithPlainAlternative()
        {
            var factory =new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string,string>()
            {{"name", "James"},
            {"userid", "123"}};

            var message = factory
                .WithTokenValues(tokenValues)
                .WithSubject("Test Subject")
                .WithHtmlBodyFromFile(@"templates\sample-email.html")
                .WithPlainTextBodyFromFile(@"templates\sample-email.txt")
                .Create();

            var from = new MailAddress("james@ciseware.com", "Automated Emailer");
            var to = new MailAddress("james@ciseware.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            _smtpClient.Send(message);   
        
            // Would be nice to read in the output file here and check it is correct but - somewhat annoyingly - there isn't a nice
            // way to retrieve the filename of the saved file without using reflection: http://www.codeproject.com/KB/IP/smtpclientext.aspx
        }
    }
}
