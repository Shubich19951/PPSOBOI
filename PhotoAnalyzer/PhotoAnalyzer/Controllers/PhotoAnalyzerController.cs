using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesLayer.ImageMinerService;
using ServicesLayer.PhotoAnalyzerService;
using ServicesLayer.ServiceBusService;

namespace PhotoAnalyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoAnalyzerController : ControllerBase
    {
        private readonly IImageMiner _imageMiner;
        private readonly IPhotoAnalyzerService _photoAnalyzerService;

        public PhotoAnalyzerController(IImageMiner imageMiner, IPhotoAnalyzerService photoAnalyzerService)
        {
            _imageMiner = imageMiner;
            _photoAnalyzerService = photoAnalyzerService;
        }

        // GET: api/<PhotoAnalyzerController>
        [HttpGet]
        public async Task<string> Get(int count)
        {
            var images = await _imageMiner.GetImages(count);

            await _photoAnalyzerService.AnalyzePhoto(images);

            return "Ok";
        }
    }
}
