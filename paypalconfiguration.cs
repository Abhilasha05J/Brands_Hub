using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineshoppingstore
{
    public class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AaZwFMCmP8ZgKpgxXWerX6G0jQurQpabi4OUB3CW65YG5DPnXqklwTYdmaO_7WvC1rOzVw6EZ4p2Mya4";
            ClientSecret = "EH2YMTQI8G74DoLzSVw41suWAnY-fkS0ts46PR4JL-2Y-apHjmqrwDoWBmUBhhNwdOqcDFjhTIBAvHNv";
        }
        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
    
