using System;
using System.Net.Mail;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableMailAddress
    {
        private String _address;
        private String _displayName;
        private String _host;
        private String _user;

        public SerializeableMailAddress(string address, string displayName, string host, string user)
        {
            _address = address;
            _displayName = displayName;
            _host = host;
            _user = user;
        }

        internal static SerializeableMailAddress GetSerializeableMailAddress(MailAddress ma)
        {
            if (ma == null)
                return null;

            var sma = new SerializeableMailAddress(ma.Address, ma.DisplayName, ma.Host, ma.User);
            return sma;
        }

        internal MailAddress GetMailAddress()
        {
            return new MailAddress(_address, _displayName);
        }
    }
}