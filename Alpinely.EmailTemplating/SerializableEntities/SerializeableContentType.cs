using System;
using System.Net.Mime;

namespace Alpinely.TownCrier.SerializableEntities
{
    /// <summary>
    /// Serialisation of mail message
    /// Adapted from code here: http://meandaspnet.blogspot.com/
    /// </summary>
    [Serializable]
    internal class SerializeableContentType
    {
        private String _boundary;
        private String _charSet;
        private String _mediaType;
        private String _name;
        private SerializeableCollection _parameters;

        internal static SerializeableContentType GetSerializeableContentType(ContentType ct)
        {
            if (ct == null)
                return null;

            var sct = new SerializeableContentType();

            sct._boundary = ct.Boundary;
            sct._charSet = ct.CharSet;
            sct._mediaType = ct.MediaType;
            sct._name = ct.Name;
            sct._parameters = SerializeableCollection.GetSerializeableCollection(ct.Parameters);

            return sct;
        }

        internal ContentType GetContentType()
        {
            var sct = new ContentType();

            sct.Boundary = _boundary;
            sct.CharSet = _charSet;
            sct.MediaType = _mediaType;
            sct.Name = _name;

            _parameters.SetColletion(sct.Parameters);

            return sct;
        }
    }
}