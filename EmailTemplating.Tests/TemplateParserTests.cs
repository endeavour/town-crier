using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ciseware.EmailTemplating;
using NUnit.Framework;

namespace Ciseware.EmailTemplating.Tests
{
    [TestFixture]
    public class TemplateParserTests
    {
        [Test]
        public void CanReplaceToken()
        {
            var inputText = "My name is {%=name%}";

            var values = new Dictionary<string, string>(){{"Name", "Bill Gates"}};

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        public void WhiteSpaceIgnoredWhenReplacingToken()
        {
            var inputText = "My name is {%=     name   %}";

            var values = new Dictionary<string, string>(){{"Name", "Bill Gates"}};

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        public void CanReplaceMultipleTokens()
        {
            var inputText = "My first name is {%=firstname%}. My second name is {%=secondname%}.";
            
            var values = new Dictionary<string, string>(){
            {"FirstName", "Bill"},
            {"SecondName", "Gates"}
            };

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. My second name is Gates."));
        }


        [Test]
        public void CanReplaceManyOfSameToken()
        {
            var inputText = "My first name is {%=firstname%}. Let me repeat that: {%=firstname%}.";

            var values = new Dictionary<string, string>(){{"FirstName", "Bill"}};

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. Let me repeat that: Bill."));
        }

        [Test]
        public void CanSupplyExtraneousProperties()
        {
            var inputText = "My name is {%=name%}";
            
            var values = new Dictionary<string, string>(){
            {"Name", "Bill Gates"},
            {"SecondName", "Gates"}
            };

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionThrownIfValueNotSuppliedForToken()
        {
            var inputText = "My name is {%=name%}";

            var values = new Dictionary<string, string>(){{"Something", "Bill Gates"}};

            var parser = new TemplateParser();
            var outputText = parser.ReplaceTokens(inputText, values);
        }
    }
}
