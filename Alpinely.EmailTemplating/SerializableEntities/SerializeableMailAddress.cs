using System;
using System.Net.Mail;

namespace Alpinely.EmailTemplating.SerializableEntities
{
    [Serializable]
    internal class SerializeableMailAddress
    {
        private String Address;
        private String DisplayName;
        private String Host;
        private String User;

        internal static SerializeableMailAddress GetSerializeableMailAddress(MailAddress ma)
        {
            if (ma == null)
                return null;
            var sma = new SerializeableMailAddress();
            sma.User = ma.User;
            sma.Host = ma.Host;
            sma.Address = ma.Address;
            sma.DisplayName = ma.DisplayName;
            return sma;
        }

        internal MailAddress GetMailAddress()
        {
            return new MailAddress(Address, DisplayName);
        }
    }
}