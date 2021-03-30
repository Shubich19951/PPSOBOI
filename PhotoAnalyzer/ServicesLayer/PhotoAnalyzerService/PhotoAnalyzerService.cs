using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using ServicesLayer.FaceClientService;
using ServicesLayer.ServiceBusService;
using ServicesLayer.Models;

namespace ServicesLayer.PhotoAnalyzerService
{
    public class PhotoAnalyzerService : IPhotoAnalyzerService
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IServiceBusService _serviceBusService;

        public PhotoAnalyzerService(IAuthenticateService authenticateService, IServiceBusService serviceBusService)
        {
            _authenticateService = authenticateService;
            _serviceBusService = serviceBusService;
        }
        public async Task AnalyzePhoto(Images images)
        {
            var client = _authenticateService.AuthenticateFaceClient();

            foreach (var image in images.Value)
            {
                try
                {
                    IList<DetectedFace> detectedFaces;

                    detectedFaces = await client.Face.DetectWithUrlAsync(image.ContentUrl,
                        returnFaceAttributes: new List<FaceAttributeType>
                        {
                                FaceAttributeType.Accessories, FaceAttributeType.Age,
                                FaceAttributeType.Emotion, FaceAttributeType.FacialHair,
                                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair,
                                FaceAttributeType.Makeup,  FaceAttributeType.Smile
                        });

                    if (detectedFaces.Count > 0)
                    {
                        var analyzeResult = new AnalyzeResult();

                        var faceNumber = 1;
                        foreach (var face in detectedFaces)
                        {
                            List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
                            int count = face.FaceAttributes.Accessories.Count;
                            string accessory;
                            string[] accessoryArray = new string[count];
                            if (count == 0) { accessory = "NoAccessories"; }
                            else
                            {
                                for (int i = 0; i < count; ++i)
                                {
                                    accessoryArray[i] = accessoriesList[i].Type.ToString();
                                }

                                accessory = string.Join(",", accessoryArray);                                
                            }

                            analyzeResult.Accessory = accessory;

                            string emotionType = string.Empty;
                            double emotionValue = 0.0;
                            Emotion emotion = face.FaceAttributes.Emotion;
                            if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                            if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                            if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                            if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                            if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                            if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                            if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                            if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }

                            analyzeResult.EmotionType = emotionType;

                            analyzeResult.FacialHair = string.Format("{0}", face.FaceAttributes.FacialHair.Moustache + face.FaceAttributes.FacialHair.Beard + face.FaceAttributes.FacialHair.Sideburns > 0 ? "Yes" : "No");

                            Hair hair = face.FaceAttributes.Hair;

                            HairColorType returnColor = HairColorType.Unknown;
                            double maxConfidence = 0.0f;
                            foreach (HairColor hairColor in hair.HairColor)
                            {
                                if (hairColor.Confidence <= maxConfidence) { continue; }
                                maxConfidence = hairColor.Confidence;
                                returnColor = hairColor.Color;
                                analyzeResult.HairColor = returnColor.ToString();
                            }

                            analyzeResult.FaceNumber = faceNumber;
                            analyzeResult.Url = image.ContentUrl;

                            faceNumber++;

                            await _serviceBusService.SendMessage(analyzeResult);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
