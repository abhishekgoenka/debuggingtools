using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ImageService.ServiceContracts
{
    /// <summary>
    /// Image server service contract.
    /// </summary>
    [ServiceContract]
    public interface IImageService
    {
        [OperationContract, WebGet(UriTemplate = "GetCustomerImageLarge/{customerId}")]
        Stream GetCustomerImageLarge(string customerId);

        [OperationContract, WebGet(UriTemplate = "GetCustomerImageSmall/{customerId}")]
        Stream GetCustomerImageSmall(string customerId);

        [OperationContract, WebGet(UriTemplate = "GetProductImage/{productId}")]
        Stream GetProductImage(string productId);
    }
}

