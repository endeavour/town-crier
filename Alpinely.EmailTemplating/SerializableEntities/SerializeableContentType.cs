using System;
using System.Net.Mime;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableContentType
    {
        private String Boundary;
        private String CharSet;
        private String MediaType;
        private String Name;
        private SerializeableCollection Parameters;

        internal static SerializeableContentType GetSerializeableContentType(ContentType ct)
        {
            if (ct == null)
                return null;

            var sct = new SerializeableContentType();

            sct.Boundary = ct.Boundary;
            sct.CharSet = ct.CharSet;
            sct.MediaType = ct.MediaType;
            sct.Name = ct.Name;
            sct.Parameters = SerializeableCollection.GetSerializeableCollection(ct.Parameters);

            return sct;
        }

        internal ContentType GetContentType()
        {
            var sct = new ContentType();

            sct.Boundary = Boundary;
            sct.CharSet = CharSet;
            sct.MediaType = MediaType;
            sct.Name = Name;

            Parameters.SetColletion(sct.Parameters);

            return sct;
        }
    }
}