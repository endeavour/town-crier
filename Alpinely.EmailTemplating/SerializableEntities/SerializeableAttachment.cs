using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableAttachment
    {
        private SerializeableContentDisposition ContentDisposition;
        private String ContentId;
        private Stream ContentStream;
        private SerializeableContentType ContentType;
        private String Name;
        private Encoding NameEncoding;
        private TransferEncoding TransferEncoding;

        internal static SerializeableAttachment GetSerializeableAttachment(Attachment att)
        {
            if (att == null)
                return null;

            var saa = new SerializeableAttachment();
            saa.ContentId = att.ContentId;
            saa.ContentDisposition =
                SerializeableContentDisposition.GetSerializeableContentDisposition(att.ContentDisposition);

            if (att.ContentStream != null)
            {
                var bytes = new byte[att.ContentStream.Length];
                att.ContentStream.Read(bytes, 0, bytes.Length);

                saa.ContentStream = new MemoryStream(bytes);
            }

            saa.ContentType = SerializeableContentType.GetSerializeableContentType(att.ContentType);
            saa.Name = att.Name;
            saa.TransferEncoding = att.TransferEncoding;
            saa.NameEncoding = att.NameEncoding;
            return saa;
        }

        internal Attachment GetAttachment()
        {
            var saa = new Attachment(ContentStream, Name);
            saa.ContentId = ContentId;
            ContentDisposition.SetContentDisposition(saa.ContentDisposition);

            saa.ContentType = ContentType.GetContentType();
            saa.Name = Name;
            saa.TransferEncoding = TransferEncoding;
            saa.NameEncoding = NameEncoding;
            return saa;
        }
    }
}