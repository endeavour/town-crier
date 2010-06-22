using System;
using System.Collections.Specialized;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    /// <summary>
    /// Serialisation of mail message
    /// Adapted from code here: http://meandaspnet.blogspot.com/
    /// </summary>
    [Serializable]
    internal class SerializeableCollection
    {
        private readonly NameValueCollection Collection = new NameValueCollection();

        internal static SerializeableCollection GetSerializeableCollection(NameValueCollection col)
        {
            if (col == null)
                return null;

            var scol = new SerializeableCollection();
            foreach (String key in col.Keys)
                scol.Collection.Add(key, col[key]);

            return scol;
        }

        internal static SerializeableCollection GetSerializeableCollection(StringDictionary col)
        {
            if (col == null)
                return null;

            var scol = new SerializeableCollection();
            foreach (String key in col.Keys)
                scol.Collection.Add(key, col[key]);

            return scol;
        }

        internal void SetColletion(NameValueCollection scol)
        {
            foreach (String key in Collection.Keys)
            {
                scol.Add(key, Collection[key]);
            }
        }

        internal void SetColletion(StringDictionary scol)
        {
            foreach (String key in Collection.Keys)
            {
                if (scol.ContainsKey(key))
                    scol[key] = Collection[key];
                else
                    scol.Add(key, Collection[key]);
            }
        }
    }
}