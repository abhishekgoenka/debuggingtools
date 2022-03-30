namespace Silverlight_Patterns_in_Action.Web
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Class containing information about the authenticated user.
    /// </summary>
    public partial class User : UserBase
    {
        /// <summary>
        /// Gets and sets the friendly name of the user.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
