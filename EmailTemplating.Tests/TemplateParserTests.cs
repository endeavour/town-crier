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

            var values = new
            {
                Name = "Bill Gates"
            };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        public void CanReplaceMultipleTokens()
        {
            var inputText = "My first name is {%=firstname%}. My second name is {%=secondname%}.";

            var values = new
            {
                FirstName = "Bill",
                SecondName = "Gates"
            };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. My second name is Gates."));
        }


        [Test]
        public void CanReplaceManyOfSameToken()
        {
            var inputText = "My first name is {%=firstname%}. Let me repeat that: {%=firstname%}.";

            var values = new
            {
                FirstName = "Bill"
            };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);

            Assert.That(outputText, Is.EqualTo("My first name is Bill. Let me repeat that: Bill."));
        }

        [Test]
        public void CanSupplyExtraneousProperties()
        {
            var inputText = "My name is {%=name%}";

            var values = new
            {
                Name = "Bill Gates",
                Age = 50
            };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);

            Assert.That(outputText, Is.EqualTo("My name is Bill Gates"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionThrownIfValueNotSuppliedForToken()
        {
            var inputText = "My name is {%=name%}";

            var values = new
            {
                SomethingElse = "Bill Gates"
            };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentExceptionThrownIfValueNotReadableProperty()
        {
            var inputText = "My name is {%=name%}";

            var values = new ClassWithWriteOnlyProperty { Name = "Bill Gates" };

            var parser = new TemplateParser(inputText);
            var outputText = parser.ReplaceTokens(values);
        }

        private class ClassWithWriteOnlyProperty
        {
            private string _name;

            public string Name
            {
                set { _name = value; }
            }
        }

    }
}
