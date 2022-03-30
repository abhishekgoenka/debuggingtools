using System;
using System.Web;
using System.Configuration;
using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Helps creating Request objects.
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// The Client Tag.
        /// </summary>
        public static string ClientTag { get; private set; }

        /// <summary>
        /// Static constructor. Reads from web.config and then stores it in memory.
        /// </summary>
        static RequestHelper()
        {
            ClientTag = ConfigurationManager.AppSettings.Get("ClientTag");
        }

        /// <summary>
        /// Gets AccessToken
        /// </summary>
        private static string AccessToken
        {
            get
            {
                if (HttpContext.Current.Session["AccessToken"] == null)
                {
                    // Request a unique accesstoken from the webservice. This token is
                    // that is valid for the duration of the session.
                    var repository = new AuthRepository();
                    HttpContext.Current.Session["AccessToken"] = repository.GetToken();
                }
                return (string)HttpContext.Current.Session["AccessToken"];
            }
        }

        /// <summary>
        /// Helper method that adds RequestId, ClientTag, and AccessToken to all request types.
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
        /// Generates unique request identifier (a Guid)
        /// </summary>
        public static string RequestId
        {
            get { return Guid.NewGuid().ToString(); }
        }
    }
}