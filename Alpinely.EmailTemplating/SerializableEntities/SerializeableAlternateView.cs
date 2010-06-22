using System;
using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableAlternateView
    {
        private readonly IList LinkedResources = new ArrayList();
        private Uri BaseUri;
        private String ContentId;
        private Stream ContentStream;
        private SerializeableContentType ContentType;
        private TransferEncoding TransferEncoding;

        internal static SerializeableAlternateView GetSerializeableAlternateView(AlternateView av)
        {
            if (av == null)
                return null;

            var sav = new SerializeableAlternateView();

            sav.BaseUri = av.BaseUri;
            sav.ContentId = av.ContentId;

            if (av.ContentStream != null)
            {
                var bytes = new byte[av.ContentStream.Length];
                av.ContentStream.Read(bytes, 0, bytes.Length);
                sav.ContentStream = new MemoryStream(bytes);
            }

            sav.ContentType = SerializeableContentType.GetSerializeableContentType(av.ContentType);

            foreach (LinkedResource lr in av.LinkedResources)
                sav.LinkedResources.Add(SerializeableLinkedResource.GetSerializeableLinkedResource(lr));

            sav.TransferEncoding = av.TransferEncoding;
            return sav;
        }

        internal AlternateView GetAlternateView()
        {
            var sav = new AlternateView(ContentStream);

            sav.BaseUri = BaseUri;
            sav.ContentId = ContentId;

            sav.ContentType = ContentType.GetContentType();

            foreach (SerializeableLinkedResource lr in LinkedResources)
                sav.LinkedResources.Add(lr.GetLinkedResource());

            sav.TransferEncoding = TransferEncoding;
            return sav;
        }
    }
}