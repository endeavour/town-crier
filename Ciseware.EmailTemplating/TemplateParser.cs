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
        protected string _templateText;        

        public TemplateParser(string templateText)
        {
            this._templateText = templateText;          
            _regExToken = new Regex(_regExString, RegexOptions.IgnoreCase);
        }

        public string ReplaceTokens(object values)
        {
            var objectType = values.GetType();
            var properties = objectType.GetProperties();

            var output = _regExToken.Replace(_templateText, (match) =>
                                                          {
                                                              var tokenName = match.Groups["TokenName"].Value.ToLower();
                                                              var property = properties.FirstOrDefault(x=>x.Name.ToLower() == tokenName && x.CanRead);

                                                              if (property == null)
                                                              {
                                                                  throw new ArgumentException(
                                                                      "No value supplied for token: " + tokenName +
                                                                      ". Check the property exists and is readable.");
                                                              }
                                                              
                                                              return property.GetValue(values, null).ToString();
                                                          });

            return output;
        }
    }

}
