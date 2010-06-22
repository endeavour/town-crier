using System;
using System.Collections.Generic;
using System.Net.Mail;
using NUnit.Framework;

namespace Alpinely.EmailTemplating.Tests
{
    [TestFixture]
    public class EmailTests
    {
        private const string SampleTemplatePath = @"Templates\sample-email.html";

        [Test]
        public void CanCreateMergedEmailFromExplicitValues()
        {
            string title = "Welcome {%=name%}, thank you for signing up!";
            string body = "Dear {%=name%}, ...";

            var tokenValues = new Dictionary<string, string> {{"Name", "Bill Gates"}};

            var factory = new MergedEmailFactory(new TemplateParser());

            MailMessage message =
                factory.WithTokenValues(tokenValues).WithSubject(title).WithPlainTextBody(body).Create();

            Assert.That(message.Subject == "Welcome Bill Gates, thank you for signing up!");
            Assert.That(message.Body == "Dear Bill Gates, ...");
        }

        [Test]
        public void CanCreateMergedEmailFromFile()
        {
            string title = "Welcome {%=name%}, thank you for signing up!";

            var tokenValues = new Dictionary<string, string> {{"Name", "Bill Gates"}, {"UserId", "123"}};

            var factory = new MergedEmailFactory(new TemplateParser());

            MailMessage message =
                factory.WithTokenValues(tokenValues).WithSubject(title).WithPlainTextBodyFromFile(SampleTemplatePath).
                    Create();
            Assert.That(message.Subject == "Welcome Bill Gates, thank you for signing up!");
            Assert.IsTrue(message.Body.Contains("Dear Bill Gates"));
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void CannotAddTwoHtmlViews()
        {
            var tokenValues = new Dictionary<string, string> {{"Name", "Bill Gates"}, {"UserId", "123"}};

            var factory = new MergedEmailFactory(new TemplateParser());
            factory.WithTokenValues(tokenValues).WithSubject("xxx").WithHtmlBody("abc").WithHtmlBody("abc");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void CannotAddTwoPlainTextViews()
        {
            var tokenValues = new Dictionary<string, string> {{"Name", "Bill Gates"}, {"UserId", "123"}};

            var factory = new MergedEmailFactory(new TemplateParser());
            factory.WithTokenValues(tokenValues).WithSubject("xxx").WithPlainTextBody("abc").WithPlainTextBody("abc");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void CannotAddTwoSubjects()
        {
            var tokenValues = new Dictionary<string, string> {{"Name", "Bill Gates"}, {"UserId", "123"}};

            var factory = new MergedEmailFactory(new TemplateParser());
            factory.WithTokenValues(tokenValues).WithSubject("xxx").WithSubject("yyy");
        }
    }
}