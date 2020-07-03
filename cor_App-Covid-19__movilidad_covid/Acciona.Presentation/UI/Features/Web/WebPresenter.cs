using Acciona.Domain;
using Acciona.Domain.Model;
using Acciona.Presentation.Navigation;
using Domain.Services;
using Newtonsoft.Json;
using Presentation.UI.Base;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Presentation.UI.Features.Web
{
    public class WebPresenter : BasePresenter<WebUI, IWebNavigator>
    {
        private string url;
        private Uri redirect;


        public override void OnCreate()
        {
            base.OnCreate();
            if (url == null)
            {
                redirect = new System.Uri("https://foo-web-med.bar/");
                var parameters = CreateRequestQueryParameters("193b7f43-ef3d-4339-98d8-15300c2686f5", redirect, "acciona-covid-webapi");
                var authorizeUrl = new System.Uri("https://foo-idservice.bar/connect/authorize");   //PRO   new System.Uri("https://foo-idservice.bar/connect/authorize"); //DEV    
                string queryString = string.Join("&", parameters.Select(i => i.Key + "=" + i.Value));
                Uri uri = string.IsNullOrEmpty(queryString) ? authorizeUrl : new Uri(authorizeUrl.AbsoluteUri + "?" + queryString);
                View.OpenUrl(uri.AbsoluteUri);
            }
            else
            {
                View.OpenUrl(url);
            }
        }

        public void SetUrl(string url)
        {
            this.url = url;
        }

        public void CloseClicked()
        {
            View.Close();
            View.OnError();
        }

        public string GenerateOAuth2StateRandom(ulong number_of_characters = 16)
        {
            //
            // Generate a unique state string to check for forgeries
            //
            var chars = new char[number_of_characters];
            var rand = new Random();
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)rand.Next((int)'a', (int)'z' + 1);
            }
            string state_string = new string(chars);

            return state_string;
        }

        public void NavigateTo(string absoluteString)
        {
            if (redirect != null && absoluteString.StartsWith(redirect.AbsoluteUri))
            {
                if (absoluteString.IndexOf("#") > 0)
                {
                    var values = FormDecode(absoluteString.Substring(absoluteString.IndexOf("#")));
                    View.Close();
                    View.OnOk(values);
                }
            }
        }



        public bool IsUriEncodedDataString(string s)
        {
            if
                (
                    s.Equals(Uri.EscapeDataString(s))
                    &&
                    Uri.IsWellFormedUriString(s, UriKind.RelativeOrAbsolute)
                )
            {
                return true;
            }

            return false;
        }

        public Dictionary<string, string> CreateRequestQueryParameters(string clientId, Uri redirectUrl, string scope)
        {

            Dictionary<string, string> oauth_request_query_parameters = null;
            oauth_request_query_parameters = new Dictionary<string, string>();

            //--------------------------------------------------------------------------------------- 
            string cid = clientId;
            if (IsUriEncodedDataString(clientId) == false)
            {
                cid = Uri.EscapeDataString(clientId);
            }
            oauth_request_query_parameters.Add("client_id", cid);
            //--------------------------------------------------------------------------------------- 

            //--------------------------------------------------------------------------------------- 
            string oauth_redirect_uri_original = redirectUrl.OriginalString;
            string oauth_redirect_uri_absolute = redirectUrl.AbsoluteUri;
            if (redirectUrl != null)
            {
                oauth_request_query_parameters.Add("redirect_uri", Uri.EscapeDataString(oauth_redirect_uri_original));
            }


            if (IsUriEncodedDataString(scope) == false)
            {
                scope = Uri.EscapeDataString(scope);
            }
            oauth_request_query_parameters.Add("scope", scope);
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            string response_type = string.Join(" ", "token");
            oauth_request_query_parameters.Add("response_type", response_type);
            //--------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            string state = GenerateOAuth2StateRandom();
            if (IsUriEncodedDataString(state) == false)
            {
                state = Uri.EscapeDataString(state);
            }
            oauth_request_query_parameters.Add("state", state);
            //---------------------------------------------------------------------------------------


            //---------------------------------------------------------------------------------------
            return oauth_request_query_parameters;
        }

        static char[] AmpersandChars = new char[] { '&' };
        public static IDictionary<string, string> FormDecode(string encodedString)
        {
            #region
            ///-------------------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		bug fix in SslCertificateEqualityComparer #76
            ///		https://github.com/xamarin/Xamarin.Auth/pull/76
            /*
			var inputs = new Dictionary<string, string> ();

			if (encodedString.StartsWith ("?") || encodedString.StartsWith ("#")) {
				encodedString = encodedString.Substring (1);
			}

			var parts = encodedString.Split (AmpersandChars);
			foreach (var p in parts) {
				var kv = p.Split (EqualsChars);
				var k = Uri.UnescapeDataString (kv[0]);
				var v = kv.Length > 1 ? Uri.UnescapeDataString (kv[1]) : "";
				inputs[k] = v;
			}
			*/
            var inputs = new Dictionary<string, string>();

            if (encodedString.Length > 0)
            {
                char firstChar = encodedString[0];
                if (firstChar == '?' || firstChar == '#')
                {
                    encodedString = encodedString.Substring(1);
                }

                if (encodedString.Length > 0)
                {
                    var parts = encodedString.Split(AmpersandChars, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var part in parts)
                    {
                        var equalsIndex = part.IndexOf('=');

                        string key;
                        string value;
                        if (equalsIndex >= 0)
                        {
                            key = Uri.UnescapeDataString(part.Substring(0, equalsIndex));
                            value = Uri.UnescapeDataString(part.Substring(equalsIndex + 1));
                        }
                        else
                        {
                            key = Uri.UnescapeDataString(part);
                            value = string.Empty;
                        }

                        inputs[key] = value;
                    }
                }
            }
            ///-------------------------------------------------------------------------------------------------
            #endregion

            return inputs;
        }
    }
}


