
using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Repository class for user authentication.
    /// </summary>
    /// <remarks>
    /// Repository Pattern.
    /// </remarks>
    public class AuthRepository : RepositoryBase
    {
        /// <summary>
        /// GetToken must be the first call into web service. 
        /// This is irrespective of whether user is logging in or not.
        /// </summary>
        /// <returns>Unique access token that is valid for the duration of the session.</returns>
        public string GetToken()
        {
            var request = new TokenRequest();
            request.RequestId = RequestHelper.RequestId;
            request.ClientTag = RequestHelper.ClientTag;

            var response = Client.GetToken(request);

            Correlate(request, response);

            return response.AccessToken;
        }

        /// <summary>
        /// Login to the system.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>Success or failure flag.</returns>
        public bool Login(string username, string password)
        {
            var request = new LoginRequest().Prepare(); ;
            request.UserName = username;
            request.Password = password;

            var response = Client.Login(request);

            Correlate(request, response);

            return (response.Acknowledge == AcknowledgeType.Success);
        }

        /// <summary>
        /// Logout from from the system.
        /// </summary>
        /// <returns>Success or failure flag.</returns>
        public bool Logout()
        {
            var request = new LogoutRequest().Prepare();

            var response = Client.Logout(request);

            Correlate(request, response);

            return (response.Acknowledge == AcknowledgeType.Success);
        }
    }
}