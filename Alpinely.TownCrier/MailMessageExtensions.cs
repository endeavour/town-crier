using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Alpinely.TownCrier
{
    /// <summary>
    /// Allows saving a mail message to a file.    
    /// Code adapted from Code Project article: http://www.codeproject.com/KB/IP/smtpclientext.aspx
    /// </summary>
    public static class MailMessageExtensions
    {
        private struct HeaderName
        {
            public const string ContentTransferEncoding = "Content-Transfer-Encoding";
            public const string ContentType = "Content-Type";
            public const string Bcc = "Bcc";
            public const string Cc = "Cc";
            public const string From = "From";
            public const string Subject = "Subject";
            public const string To = "To";
            public const string MimeVersion = "MIME-Version";
            public const string MessageId = "Message-ID";
            public const string Priority = "Priority";
            public const string Importance = "Importance";
            public const string XPriority = "X-Priority";
            public const string Date = "Date";
        }

        public static void Save(this MailMessage message, Stream outputStream)
        {
            // Get reflection info for Send() method on MailMessage
            MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendMethod != null)
            {
                SaveCLR(message, outputStream, sendMethod);
            }
            else
            {
                SaveMono(message, outputStream);
            }
        }

        private static void SaveCLR(MailMessage message, Stream outputStream, MethodInfo sendMethod)
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

            var methodParams = sendMethod.GetParameters().Length == 2
                                   ? new[] { mailWriter, true } // .NET 4.0 (and earlier?)
                                   : new[] { mailWriter, true, true }; // .NET 4.5 has an additional parameter

            // Call method passing in MailWriter
            sendMethod.Invoke(
                message,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                methodParams,
                null);
        }

        private static void SaveMono(MailMessage message, Stream outputStream)
        {
            // On Mono the implementation is different, and in some ways even worse than the MS implementation
            // so we have to use reflection to replicate the important bits of the SendToFile method
            using (var sw = new StreamWriter(outputStream))
            using (var client = new SmtpClient())
            {
                var smtpClientType = typeof(SmtpClient);
                var streamField = smtpClientType.GetField("writer", BindingFlags.Instance | BindingFlags.NonPublic);
                var boundaryIndexField = smtpClientType.GetField("boundaryIndex", BindingFlags.Instance | BindingFlags.NonPublic);
                var sendHeaderMethod = smtpClientType.GetMethod("SendHeader", BindingFlags.Instance | BindingFlags.NonPublic);
                var encodeAddressMethod = smtpClientType.GetMethod("EncodeAddress", BindingFlags.Static | BindingFlags.NonPublic);
                var encodeAddressesMethod = smtpClientType.GetMethod("EncodeAddresses", BindingFlags.Static | BindingFlags.NonPublic);
                var encodeSubjectRFC2047Method = smtpClientType.GetMethod("EncodeSubjectRFC2047", BindingFlags.Instance | BindingFlags.NonPublic);
                var addPriorityHeaderMethod = smtpClientType.GetMethod("AddPriorityHeader", BindingFlags.Instance | BindingFlags.NonPublic);
                var sendWithoutAttachmentsMethod = smtpClientType.GetMethod("SendWithoutAttachments", BindingFlags.Instance | BindingFlags.NonPublic);
                var sendWithAttachmentsMethod = smtpClientType.GetMethod("SendWithAttachments", BindingFlags.Instance | BindingFlags.NonPublic);

                streamField.SetValue(client, sw);

                sendHeaderMethod.Invoke(
                    client,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[]
                        {
                            HeaderName.Date,
                            DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss zzz", DateTimeFormatInfo.InvariantInfo)
                        },
                    null);

                var from = encodeAddressMethod.Invoke(null, BindingFlags.Static | BindingFlags.NonPublic, null,
                                                      new[] { message.From }, null);

                var to = encodeAddressesMethod.Invoke(null, BindingFlags.Static | BindingFlags.NonPublic, null,
                                                      new[] { message.To }, null);

                sendHeaderMethod.Invoke(
                    client,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { HeaderName.From, @from },
                    null);

                sendHeaderMethod.Invoke(
                    client,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { HeaderName.To, to },
                    null);

                if (message.CC.Count > 0)
                {
                    var cc = encodeAddressesMethod.Invoke(null, BindingFlags.Static | BindingFlags.NonPublic, null,
                                                          new[] { message.CC }, null);

                    sendHeaderMethod.Invoke(
                        client,
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new object[] { HeaderName.Cc, cc },
                        null);
                }

                //TODO: Should probably do BCC here too but mono source code doesn't for some reason? Maybe a bug in mono?

                var subject = encodeSubjectRFC2047Method.Invoke(client,
                                                                BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                                new[] { message }, null);


                sendHeaderMethod.Invoke(
                    client,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { HeaderName.Subject, subject },
                    null);

                foreach (var headerName in message.Headers.AllKeys)
                {
                    sendHeaderMethod.Invoke(
                        client,
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new object[] { headerName, message.Headers[headerName] },
                        null);
                }

                addPriorityHeaderMethod.Invoke(
                    client,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { message },
                    null);

                boundaryIndexField.SetValue(client, 0);

                if (message.Attachments.Count > 0)
                {
                    sendWithAttachmentsMethod.Invoke(client, BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                     new[] { message }, null);
                }
                else
                {
                    sendWithoutAttachmentsMethod.Invoke(client, BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                        new object[] { message, null, false }, null);
                }

                sw.Flush();
            }
        }

    }
}

