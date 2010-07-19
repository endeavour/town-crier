using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Alpinely.TownCrier.Tests
{
    [TestFixture]
    public class TemplateParserTests
    {
        [Test]
        public void CanReplaceManyOfSameToken()
        {
            string inputText = "My first name is {%=firstname%}. Let me repeat that: {%=firstname%}.";

            var values = new Dictionary<string, string> {{"FirstName", "Bill"}};

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. Let me repeat that: Bill."));
        }

        [Test]
        public void CanReplaceMultipleTokens()
        {
            string inputText = "My first name is {%=firstname%}. My second name is {%=secondname%}.";

            var values = new Dictionary<string, string>
                             {
                                 {"FirstName", "Bill"},
                                 {"SecondName", "Gates"}
                             };

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. My second name is Gates."));
        }

        [Test]
        public void CanReplaceToken()
        {
            string inputText = "My name is {%=name%}";

            var values = new Dictionary<string, string> {{"Name", "Bill Gates"}};

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        public void CanSupplyExtraneousProperties()
        {
            string inputText = "My name is {%=name%}";

            var values = new Dictionary<string, string>
                             {
                                 {"Name", "Bill Gates"},
                                 {"SecondName", "Gates"}
                             };

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ExceptionThrownIfValueNotSuppliedForToken()
        {
            string inputText = "My name is {%=name%}";

            var values = new Dictionary<string, string> {{"Something", "Bill Gates"}};

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);
        }

        [Test]
        public void WhiteSpaceIgnoredWhenReplacingToken()
        {
            string inputText = "My name is {%=     name   %}";

            var values = new Dictionary<string, string> {{"Name", "Bill Gates"}};

            var parser = new TemplateParser();
            string outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }
    }
}