using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ciseware.EmailTemplating
{
    public class TemplateParser
    {
        protected string _regExString = @"\{\%\=\s*(?<TokenName>\w*)\s*\%\}";
        protected Regex _regExToken;

        public TemplateParser()
        {
            _regExToken = new Regex(_regExString, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Replaces tokens in the template text with the values from the supplied dictionary
        /// </summary>
        /// <param name="templateText">The template text</param>
        /// <param name="tokenValues">Dictionary mapping token names to values</param>
        /// <returns>Text with tokens replaced with their corresponding values from the dictionary</returns>
        public string ReplaceTokens(string templateText, IDictionary<string, string> tokenValues)
        {
            var output = _regExToken.Replace(templateText, (match) =>
                                                          {
                                                              var tokenName = match.Groups["TokenName"].Value.ToLower();
                                                              try
                                                              {
                                                                  var property = tokenValues.First(x=>x.Key.ToLower() == tokenName);
                                                                  return property.Value;
                                                              }
                                                              catch (Exception)
                                                              {
                                                                  throw new ArgumentException("No value supplied for token: " + tokenName);
                                                              }                                                             
                                                          });
            return output;
        }
    }

}
