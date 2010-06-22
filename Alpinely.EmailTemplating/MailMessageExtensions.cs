using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Alpinely.EmailTemplating
{
    /// <summary>
    /// Allows saving a mail message to a file.    
    /// Code adapted from Code Project article: http://www.codeproject.com/KB/IP/smtpclientext.aspx
    /// </summary>
    public static class MailMessageExtensions
    {        
        public static void Save(this MailMessage message, Stream outputStream)
        {
            // Unfortunately we have to use reflection because of poorly designed .NET framework components
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            
                // Get reflection info for MailWriter contructor
                var mailWriterContructor = mailWriterType.GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new[] { typeof(Stream) },
                        null);

                // Construct MailWriter object with our FileStream
                object mailWriter = mailWriterContructor.Invoke(new object[] { outputStream });

                // Get reflection info for Send() method on MailMessage
                MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

                // Call method passing in MailWriter
                sendMethod.Invoke(
                    message,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new[] { mailWriter, true },
                    null);
            }
        }
}

