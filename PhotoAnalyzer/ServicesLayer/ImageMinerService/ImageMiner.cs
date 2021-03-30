using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using ServicesLayer.FaceClientService;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace ServicesLayer.ImageMinerService
{
    public class ImageMiner : IImageMiner
    {
        static string searchTerm = "human faces";

        private readonly IAuthenticateService _authenticateService;

        public ImageMiner(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        public async Task<Images> GetImages(int count)
        {
            var client = _authenticateService.AuthenticateImageClient();

            var imageResults = await client.Images.SearchAsync(query: searchTerm, count: count);
            return imageResults;
        }
    }
}
