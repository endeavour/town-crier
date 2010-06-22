using System;
using System.Net.Mime;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableContentDisposition
    {
        private DateTime CreationDate;
        private String DispositionType;
        private String FileName;
        private Boolean Inline;
        private DateTime ModificationDate;
        private SerializeableCollection Parameters;
        private DateTime ReadDate;
        private long Size;

        internal static SerializeableContentDisposition GetSerializeableContentDisposition(ContentDisposition cd)
        {
            if (cd == null)
                return null;

            var scd = new SerializeableContentDisposition();
            scd.CreationDate = cd.CreationDate;
            scd.DispositionType = cd.DispositionType;
            scd.FileName = cd.FileName;
            scd.Inline = cd.Inline;
            scd.ModificationDate = cd.ModificationDate;
            scd.Parameters = SerializeableCollection.GetSerializeableCollection(cd.Parameters);
            scd.ReadDate = cd.ReadDate;
            scd.Size = cd.Size;

            return scd;
        }

        internal void SetContentDisposition(ContentDisposition scd)
        {
            scd.CreationDate = CreationDate;
            scd.DispositionType = DispositionType;
            scd.FileName = FileName;
            scd.Inline = Inline;
            scd.ModificationDate = ModificationDate;
            Parameters.SetColletion(scd.Parameters);

            scd.ReadDate = ReadDate;
            scd.Size = Size;
        }
    }
}