using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Ciseware.EmailTemplating;
using NUnit.Framework;

namespace Ciseware.EmailTemplating.Tests
{
    [TestFixture]
    public class EmailTests
    {
        const string sampleTemplatePath = "Templates\\sample-email.html";
        
        [Test]
        public void CanCreateMergedEmailFromExplicitValues()
        {
            string title = "Welcome {%=name%}, thank you for signing up!";
            string body = "Dear {%=name%}, ...";

            var tokenValues = new Dictionary<string,string>{{"Name", "Bill Gates"}};

            var factory = new MergedEmailFactory(new TemplateParser());

            MailMessage message = factory.Create(title, body, tokenValues);

            Assert.That(message.Subject == "Welcome Bill Gates, thank you for signing up!");
            Assert.That(message.Body == "Dear Bill Gates, ...");
        }

        [Test]
        public void CanCreateMergedEmailFromFile()
        {
            string title = "Welcome {%=name%}, thank you for signing up!";
            
            var tokenValues = new Dictionary<string,string>{{"Name", "Bill Gates"},{"UserId", "123"}};

            var factory = new MergedEmailFactory(new TemplateParser());

            MailMessage message = factory.CreateFromFile(title, sampleTemplatePath, tokenValues);

            Assert.That(message.Subject == "Welcome Bill Gates, thank you for signing up!");
            Assert.IsTrue(message.Body.Contains("Dear Bill Gates"));
        }
    }
}
