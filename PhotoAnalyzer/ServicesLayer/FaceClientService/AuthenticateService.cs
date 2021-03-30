using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace ServicesLayer.FaceClientService
{
    public class AuthenticateService : IAuthenticateService
    {
        private string SubscriptionKeyFace = "121a034c34154abdbdce49d396325b8d";
        private string SubscriptionKeySearch = "670fd9da678c45bc863d686d402b32cb";
        private string FaceEndpoint = "https://photo-analyzer-face.cognitiveservices.azure.com/";

        public IFaceClient AuthenticateFaceClient()
        {
            return new FaceClient(new Microsoft.Azure.CognitiveServices.Vision.Face.ApiKeyServiceClientCredentials(SubscriptionKeyFace)) { Endpoint = FaceEndpoint };
        }

        public ImageSearchClient AuthenticateImageClient()
        {
            return new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(SubscriptionKeySearch));
        }
    }
}
