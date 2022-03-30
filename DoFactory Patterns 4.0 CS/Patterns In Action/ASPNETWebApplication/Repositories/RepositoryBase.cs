using System;
using System.Web;
using System.ServiceModel;

using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Base class for all Repositories. Manages Service client.
    /// Provides common request-response correlation check.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Lazy loads ActionServiceClient and stores it in Session object.
        /// </summary>
        protected ActionServiceClient Client
        {
            get
            {
                // Check if not initialized yet
                if (HttpContext.Current.Session["ActionServiceClient"] == null)
                    HttpContext.Current.Session["ActionServiceClient"] = new ActionServiceClient();

                // If current client is 'faulted' (due to some error), create a new instance.
                var client = HttpContext.Current.Session["ActionServiceClient"] as ActionServiceClient;
                if (client.State == CommunicationState.Faulted)
                {
                    try { client.Abort(); }
                    catch { /* no action */ }

                    client = new ActionServiceClient();
                    HttpContext.Current.Session["ActionServiceClient"] = client;
                }

                return client;
            }
        }

        protected void Correlate(RequestBase request, ResponseBase response)
        {
            if (request.RequestId != response.CorrelationId)
                throw new ApplicationException("RequestId and CorrelationId do not match.");
        }
    }
}
