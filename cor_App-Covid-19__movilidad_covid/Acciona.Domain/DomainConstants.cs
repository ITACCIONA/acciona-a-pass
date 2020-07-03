using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain
{
    public class DomainConstants
    {
        public const string LAST_USER = "LastUser";
        public const string LAST_USER_INFO = "UserInfo";
        public const string LAST_SESSION = "LastSession";
        public const string LAST_DATE = "LastDate";
        public const string LAST_PASSPORT_STATE = "LastPassportState";
        public const string OFFLINE_PASSPORT = "OfflinePassport";
        public const string LAST_START = "LAST_START";
        public const string LOCATION = "Location";

        public const string LAST_DATA_UPDATE = "lastUpdate";
        public const string EMAIL_CONTACT = "consultaMedica@foo.bar";        

        //EncrypUtils
        public const string PassBase = "secret4QR";   
        public const string SaltValue = "!aSimpleSalt!";
        public const string HashAlgorithm = "MD5";
        public const string InitVector = "@f6bdfd2e8cd72ea";

        //URLS
        public const string iOSURL = "https://foo-install.bar/droid/public";
        public const string androidURL = "https://foo-install.bar/ios/public";
        public const string seguridadURL = "https://foo-install.bar/security/public";
    }
}
