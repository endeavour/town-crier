using System;
using System.Collections;
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
    internal class SerializeableAlternateView
    {
        private readonly IList _linkedResources = new ArrayList();
        private Uri _baseUri;
        private String _contentId;
        private Stream _contentStream;
        private SerializeableContentType _contentType;
        private TransferEncoding _transferEncoding;

        internal static SerializeableAlternateView GetSerializeableAlternateView(AlternateView av)
        {
            if (av == null)
                return null;

            var sav = new SerializeableAlternateView();

            sav._baseUri = av.BaseUri;
            sav._contentId = av.ContentId;

            if (av.ContentStream != null)
            {
                var bytes = new byte[av.ContentStream.Length];
                av.ContentStream.Read(bytes, 0, bytes.Length);
                sav._contentStream = new MemoryStream(bytes);
            }

            sav._contentType = SerializeableContentType.GetSerializeableContentType(av.ContentType);

            foreach (LinkedResource lr in av.LinkedResources)
                sav._linkedResources.Add(SerializeableLinkedResource.GetSerializeableLinkedResource(lr));

            sav._transferEncoding = av.TransferEncoding;
            return sav;
        }

        internal AlternateView GetAlternateView()
        {
            var sav = new AlternateView(_contentStream);

            sav.BaseUri = _baseUri;
            sav.ContentId = _contentId;

            sav.ContentType = _contentType.GetContentType();

            foreach (SerializeableLinkedResource lr in _linkedResources)
                sav.LinkedResources.Add(lr.GetLinkedResource());

            sav.TransferEncoding = _transferEncoding;
            return sav;
        }
    }
}