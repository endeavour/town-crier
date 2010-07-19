using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Alpinely.TownCrier.SerializableEntities
{
    /// <summary>
    /// Serialisation of mail message
    /// Adapted from code here: http://meandaspnet.blogspot.com/
    /// </summary>
    [Serializable]
    public class SerializeableMailMessage
    {
        private readonly IList<SerializeableAlternateView> _alternateViews = new List<SerializeableAlternateView>();
        private readonly IList<SerializeableAttachment> _attachments = new List<SerializeableAttachment>();
        private readonly IList<SerializeableMailAddress> _bcc = new List<SerializeableMailAddress>();
        private readonly Encoding _bodyEncoding;
        private readonly IList<SerializeableMailAddress> _cc = new List<SerializeableMailAddress>();
        private readonly DeliveryNotificationOptions _deliveryNotificationOptions;
        private readonly SerializeableCollection _headers;
        private readonly MailPriority _priority;
        private readonly Encoding _subjectEncoding;
        private readonly IList<SerializeableMailAddress> _to = new List<SerializeableMailAddress>();

        ///
        /// Creates a new serializeable mailmessage based on a MailMessage object
        ///
        public SerializeableMailMessage(MailMessage mailMessage)
        {
            IsBodyHtml = mailMessage.IsBodyHtml;
            Body = mailMessage.Body;
            Subject = mailMessage.Subject;
            From = SerializeableMailAddress.GetSerializeableMailAddress(mailMessage.From);
            _to = new List<SerializeableMailAddress>();
            foreach (var mailAddress in mailMessage.To)
            {
                _to.Add(SerializeableMailAddress.GetSerializeableMailAddress(mailAddress));
            }

            _cc = new List<SerializeableMailAddress>();
            foreach (var mailAddress in mailMessage.CC)
            {
                _cc.Add(SerializeableMailAddress.GetSerializeableMailAddress(mailAddress));
            }

            _bcc = new List<SerializeableMailAddress>();
            foreach (var mailAddress in mailMessage.Bcc)
            {
                _bcc.Add(SerializeableMailAddress.GetSerializeableMailAddress(mailAddress));
            }

            _attachments = new List<SerializeableAttachment>();
            foreach (var attachment in mailMessage.Attachments)
            {
                _attachments.Add(SerializeableAttachment.GetSerializeableAttachment(attachment));
            }

            _bodyEncoding = mailMessage.BodyEncoding;

            _deliveryNotificationOptions = mailMessage.DeliveryNotificationOptions;
            _headers = SerializeableCollection.GetSerializeableCollection(mailMessage.Headers);
            _priority = mailMessage.Priority;
            ReplyTo = mailMessage.ReplyToList.Select(SerializeableMailAddress.GetSerializeableMailAddress).ToList();
            Sender = SerializeableMailAddress.GetSerializeableMailAddress(mailMessage.Sender);
            _subjectEncoding = mailMessage.SubjectEncoding;

            foreach (AlternateView av in mailMessage.AlternateViews)
                _alternateViews.Add(SerializeableAlternateView.GetSerializeableAlternateView(av));
        }

        private Boolean IsBodyHtml { get; set; }
        private String Body { get; set; }
        private SerializeableMailAddress From { get; set; }
        private IList<SerializeableMailAddress> ReplyTo { get; set; }
        private SerializeableMailAddress Sender { get; set; }
        private String Subject { get; set; }


        ///
        /// Returns the MailMessge object from the serializeable object
        ///
        ///
        public MailMessage GetMailMessage()
        {
            var mailMessage = new MailMessage();

            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.Body = Body;
            mailMessage.Subject = Subject;
            if (From != null)
                mailMessage.From = From.GetMailAddress();

            foreach (var mailAddress in _to)
            {
                mailMessage.To.Add(mailAddress.GetMailAddress());
            }

            foreach (var mailAddress in _cc)
            {
                mailMessage.CC.Add(mailAddress.GetMailAddress());
            }

            foreach (var mailAddress in _bcc)
            {
                mailMessage.Bcc.Add(mailAddress.GetMailAddress());
            }

            foreach (var attachment in _attachments)
            {
                mailMessage.Attachments.Add(attachment.GetAttachment());
            }

            mailMessage.BodyEncoding = _bodyEncoding;

            mailMessage.DeliveryNotificationOptions = _deliveryNotificationOptions;
            _headers.SetColletion(mailMessage.Headers);
            mailMessage.Priority = _priority;
            if (ReplyTo != null)
            {
                foreach (SerializeableMailAddress address in ReplyTo)
                {
                    mailMessage.ReplyToList.Add(address.GetMailAddress());
                }
            }

            if (Sender != null)
                mailMessage.Sender = Sender.GetMailAddress();

            mailMessage.SubjectEncoding = _subjectEncoding;

            foreach (var alternateView in _alternateViews)
                mailMessage.AlternateViews.Add(alternateView.GetAlternateView());

            return mailMessage;
        }
    }
}