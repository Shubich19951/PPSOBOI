using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ImageMinerService
{
    public interface IImageMiner
    {
        Task<Images> GetImages(int count);
    }
}
