using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace ServicesLayer.FaceClientService
{
    public interface IAuthenticateService
    {
        IFaceClient AuthenticateFaceClient();

        ImageSearchClient AuthenticateImageClient();
    }
}
