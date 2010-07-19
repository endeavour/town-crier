using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Alpinely.TownCrier.SerializableEntities
{
    /// <summary>
    /// Serialisation of mail message
    /// Adapted from code here: http://meandaspnet.blogspot.com/
    /// </summary>
    [Serializable]
    internal class SerializeableLinkedResource
    {
        private String _contentId;
        private Uri _contentLink;
        private Stream _contentStream;
        private SerializeableContentType _contentType;
        private TransferEncoding _transferEncoding;

        internal static SerializeableLinkedResource GetSerializeableLinkedResource(LinkedResource lr)
        {
            if (lr == null)
                return null;

            var slr = new SerializeableLinkedResource();
            slr._contentId = lr.ContentId;
            slr._contentLink = lr.ContentLink;

            if (lr.ContentStream != null)
            {
                var bytes = new byte[lr.ContentStream.Length];
                lr.ContentStream.Read(bytes, 0, bytes.Length);
                slr._contentStream = new MemoryStream(bytes);
            }

            slr._contentType = SerializeableContentType.GetSerializeableContentType(lr.ContentType);
            slr._transferEncoding = lr.TransferEncoding;

            return slr;
        }

        internal LinkedResource GetLinkedResource()
        {
            var slr = new LinkedResource(_contentStream);
            slr.ContentId = _contentId;
            slr.ContentLink = _contentLink;

            slr.ContentType = _contentType.GetContentType();
            slr.TransferEncoding = _transferEncoding;

            return slr;
        }
    }
}