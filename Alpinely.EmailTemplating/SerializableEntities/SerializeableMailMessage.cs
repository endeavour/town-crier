using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    ///
    /// Serializeable mailmessage object
    ///
    [Serializable]
    public class SerializeableMailMessage
    {
        private readonly IList<SerializeableAlternateView> AlternateViews = new List<SerializeableAlternateView>();
        private readonly IList<SerializeableAttachment> Attachments = new List<SerializeableAttachment>();
        private readonly IList<SerializeableMailAddress> Bcc = new List<SerializeableMailAddress>();
        private readonly Encoding BodyEncoding;
        private readonly IList<SerializeableMailAddress> CC = new List<SerializeableMailAddress>();
        private readonly DeliveryNotificationOptions DeliveryNotificationOptions;
        private readonly SerializeableCollection Headers;
        private readonly MailPriority Priority;
        private readonly Encoding SubjectEncoding;
        private readonly IList<SerializeableMailAddress> To = new List<SerializeableMailAddress>();

        ///
        /// Creates a new serializeable mailmessage based on a MailMessage object
        ///
        ///
        public SerializeableMailMessage(MailMessage mm)
        {
            IsBodyHtml = mm.IsBodyHtml;
            Body = mm.Body;
            Subject = mm.Subject;
            From = SerializeableMailAddress.GetSerializeableMailAddress(mm.From);
            To = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.To)
            {
                To.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            CC = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.CC)
            {
                CC.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            Bcc = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.Bcc)
            {
                Bcc.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            Attachments = new List<SerializeableAttachment>();
            foreach (Attachment att in mm.Attachments)
            {
                Attachments.Add(SerializeableAttachment.GetSerializeableAttachment(att));
            }

            BodyEncoding = mm.BodyEncoding;

            DeliveryNotificationOptions = mm.DeliveryNotificationOptions;
            Headers = SerializeableCollection.GetSerializeableCollection(mm.Headers);
            Priority = mm.Priority;
            ReplyTo =
                mm.ReplyToList.Select(message => SerializeableMailAddress.GetSerializeableMailAddress(message)).ToList();
            Sender = SerializeableMailAddress.GetSerializeableMailAddress(mm.Sender);
            SubjectEncoding = mm.SubjectEncoding;

            foreach (AlternateView av in mm.AlternateViews)
                AlternateViews.Add(SerializeableAlternateView.GetSerializeableAlternateView(av));
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
            var mm = new MailMessage();

            mm.IsBodyHtml = IsBodyHtml;
            mm.Body = Body;
            mm.Subject = Subject;
            if (From != null)
                mm.From = From.GetMailAddress();

            foreach (SerializeableMailAddress ma in To)
            {
                mm.To.Add(ma.GetMailAddress());
            }

            foreach (SerializeableMailAddress ma in CC)
            {
                mm.CC.Add(ma.GetMailAddress());
            }

            foreach (SerializeableMailAddress ma in Bcc)
            {
                mm.Bcc.Add(ma.GetMailAddress());
            }

            foreach (SerializeableAttachment att in Attachments)
            {
                mm.Attachments.Add(att.GetAttachment());
            }

            mm.BodyEncoding = BodyEncoding;

            mm.DeliveryNotificationOptions = DeliveryNotificationOptions;
            Headers.SetColletion(mm.Headers);
            mm.Priority = Priority;
            if (ReplyTo != null)
            {
                foreach (SerializeableMailAddress address in ReplyTo)
                {
                    mm.ReplyToList.Add(address.GetMailAddress());
                }
            }

            if (Sender != null)
                mm.Sender = Sender.GetMailAddress();

            mm.SubjectEncoding = SubjectEncoding;

            foreach (SerializeableAlternateView av in AlternateViews)
                mm.AlternateViews.Add(av.GetAlternateView());

            return mm;
        }
    }
}