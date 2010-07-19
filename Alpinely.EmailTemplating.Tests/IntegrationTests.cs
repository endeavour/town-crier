using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Alpinely.TownCrier;
using NUnit.Framework;
namespace Alpinely.TownCrier.Tests
{    
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void CanSendHtmlEmail()
        {
            var factory = new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string, string>
                                  {
                                      {"name", "Joe"},
                                      {"userid", "123"}
                                  };

            MailMessage message = factory
                .WithTokenValues(tokenValues)
                .WithHtmlBodyFromFile(@"templates\sample-email.html")
                .Create();

            var from = new MailAddress("robot@test.com", "Automated Emailer");
            var to = new MailAddress("joe@bloggs.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            using (var output = new MemoryStream())
            {
                message.Save(output);
                var result = StreamToString(output);

                Assert.That(result, Contains.Substring(@"Content-Type: text/html"));

                Assert.That(result, Contains.Substring(@"Dear Joe"));

                // Note that actually the email is encoded with RFC 2045 "Quoted Printable" Encoding, but .NET doesn't ship
                // with a decoder so we'll add the =3D into our test for simplicity.
                Assert.That(result, Contains.Substring(@"http://localhost/trackedLink?userId=3D123"));                

                Console.WriteLine(result);
            }
        }

        [Test]
        public void CanSendHtmlEmailWithPlainAlternative()
        {
            var factory = new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string, string>
                                  {
                                      {"name", "Joe"},
                                      {"userid", "123"}
                                  };

            MailMessage message = factory
                .WithTokenValues(tokenValues)
                .WithSubject("Test Subject")
                .WithHtmlBodyFromFile(@"templates\sample-email.html")
                .WithPlainTextBodyFromFile(@"templates\sample-email.txt")
                .Create();

            var from = new MailAddress("sender@test.com", "Automated Emailer");
            var to = new MailAddress("recipient@test.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            using (var output = new MemoryStream())
            {
                message.Save(output);
                var result = StreamToString(output);

                Assert.That(result, Contains.Substring(@"Content-Type: multipart/alternative"));

                Assert.That(result, Contains.Substring(@"Dear Joe"));

                // Note that actually the email is encoded with RFC 2045 "Quoted Printable" Encoding, but .NET doesn't ship
                // with a decoder so we'll add the =3D into our test for simplicity.
                Assert.That(result, Contains.Substring(@"http://localhost/trackedLink?userId=3D123"));

                Console.WriteLine(result);
            }
        }

        [Test]
        public void CanSendPlainEmail()
        {
            var factory = new MergedEmailFactory(new TemplateParser());

            var tokenValues = new Dictionary<string, string>
                                  {
                                      {"name", "Joe"},
                                      {"userid", "123"}
                                  };

            MailMessage message = factory
                .WithTokenValues(tokenValues)
                .WithPlainTextBodyFromFile(@"templates\sample-email.txt")
                .Create();

            var from = new MailAddress("robot@test.com", "Automated Emailer");
            var to = new MailAddress("joe@bloggs.com", "Joe Bloggs");
            message.From = from;
            message.To.Add(to);

            using (var output = new MemoryStream())
            {
                message.Save(output);
                var result = StreamToString(output);

                Assert.That(result, Contains.Substring(@"Content-Type: text/plain"));

                Assert.That(result, Contains.Substring(@"Dear Joe"));

                // Note that actually the email is encoded with RFC 2045 "Quoted Printable" Encoding, but .NET doesn't ship
                // with a decoder so we'll add the =3D into our test for simplicity.
                Assert.That(result, Contains.Substring(@"http://localhost/trackedLink?userId=3D123"));

                Console.WriteLine(result);
            }
        }

        private static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}