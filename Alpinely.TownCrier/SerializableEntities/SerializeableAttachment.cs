using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Alpinely.TownCrier.SerializableEntities
{
    /// <summary>
    /// Serialisation of mail message
    /// Adapted from code here: http://meandaspnet.blogspot.com/
    /// </summary>
    [Serializable]
    internal class SerializeableAttachment
    {
        private SerializeableContentDisposition _contentDisposition;
        private String _contentId;
        private Stream _contentStream;
        private SerializeableContentType _contentType;
        private String _name;
        private Encoding _nameEncoding;
        private TransferEncoding _transferEncoding;

        internal static SerializeableAttachment GetSerializeableAttachment(Attachment att)
        {
            if (att == null)
                return null;

            var saa = new SerializeableAttachment();
            saa._contentId = att.ContentId;
            saa._contentDisposition =
                SerializeableContentDisposition.GetSerializeableContentDisposition(att.ContentDisposition);

            if (att.ContentStream != null)
            {
                var bytes = new byte[att.ContentStream.Length];
                att.ContentStream.Read(bytes, 0, bytes.Length);

                saa._contentStream = new MemoryStream(bytes);
            }

            saa._contentType = SerializeableContentType.GetSerializeableContentType(att.ContentType);
            saa._name = att.Name;
            saa._transferEncoding = att.TransferEncoding;
            saa._nameEncoding = att.NameEncoding;
            return saa;
        }

        internal Attachment GetAttachment()
        {
            var saa = new Attachment(_contentStream, _name);
            saa.ContentId = _contentId;
            _contentDisposition.SetContentDisposition(saa.ContentDisposition);

            saa.ContentType = _contentType.GetContentType();
            saa.Name = _name;
            saa.TransferEncoding = _transferEncoding;
            saa.NameEncoding = _nameEncoding;
            return saa;
        }
    }
}