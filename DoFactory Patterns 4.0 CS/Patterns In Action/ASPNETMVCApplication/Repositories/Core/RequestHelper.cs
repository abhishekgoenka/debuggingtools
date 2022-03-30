using System;
using System.Web;
using System.Configuration;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories.Core
{
    /// <summary>
    /// Static Request Helper class. 
    /// Provides common functionalities that apply to all Request types.
    /// </summary>
    public static class RequestHelper
    {
        // The application ClientTag 
        public static string ClientTag { get; private set; }

        /// <summary>
        /// Static constructor. Sets the ClientTag (read from web.config).
        /// </summary>
        static RequestHelper()
        {
            ClientTag = ConfigurationManager.AppSettings.Get("ClientTag");
        }

        /// <summary>
        /// Gets or sets the Access Token value (provided by Server and stored in Session).
        /// </summary>
        public static string AccessToken
        {
            get
            {
                if (HttpContext.Current.Session["AccessToken"] == null)
                {
                    // Request a unique accesstoken from the webservice. This token is
                    // valid for the duration of the session.
                    var repository = new AuthRepository();
                    HttpContext.Current.Session["AccessToken"] = repository.GetToken();
                }
                return (string)HttpContext.Current.Session["AccessToken"];
            }
        }

        /// <summary>
        /// Helper extension method that adds RequestId, ClientTag, and AccessToken to all request types.
        /// </summary>
        /// <typeparam name="T">The request type.</typeparam>
        /// <param name="request">The request</param>
        /// <returns>Fully prepared request, ready to use.</returns>
        public static T Prepare<T>(this T request) where T : RequestBase
        {
            request.RequestId = RequestId;
            request.ClientTag = ClientTag;
            request.AccessToken = AccessToken;

            return request;
        }

        /// <summary>
        /// Generates unique request GUID identifier. 
        /// </summary>
        public static string RequestId
        {
            get { return Guid.NewGuid().ToString(); }
        }
    }
}