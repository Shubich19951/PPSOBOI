using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace ServicesLayer.PhotoAnalyzerService
{
    public interface IPhotoAnalyzerService
    {
        Task AnalyzePhoto(Images images);
    }
}
