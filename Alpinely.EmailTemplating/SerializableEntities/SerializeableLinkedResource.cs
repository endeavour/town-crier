using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableLinkedResource
    {
        private String ContentId;
        private Uri ContentLink;
        private Stream ContentStream;
        private SerializeableContentType ContentType;
        private TransferEncoding TransferEncoding;

        internal static SerializeableLinkedResource GetSerializeableLinkedResource(LinkedResource lr)
        {
            if (lr == null)
                return null;

            var slr = new SerializeableLinkedResource();
            slr.ContentId = lr.ContentId;
            slr.ContentLink = lr.ContentLink;

            if (lr.ContentStream != null)
            {
                var bytes = new byte[lr.ContentStream.Length];
                lr.ContentStream.Read(bytes, 0, bytes.Length);
                slr.ContentStream = new MemoryStream(bytes);
            }

            slr.ContentType = SerializeableContentType.GetSerializeableContentType(lr.ContentType);
            slr.TransferEncoding = lr.TransferEncoding;

            return slr;
        }

        internal LinkedResource GetLinkedResource()
        {
            var slr = new LinkedResource(ContentStream);
            slr.ContentId = ContentId;
            slr.ContentLink = ContentLink;

            slr.ContentType = ContentType.GetContentType();
            slr.TransferEncoding = TransferEncoding;

            return slr;
        }
    }
}