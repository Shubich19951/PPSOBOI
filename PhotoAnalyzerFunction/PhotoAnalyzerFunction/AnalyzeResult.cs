using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAnalyzerFunction
{
    public class AnalyzeResult
    {
        public string id { get; set; }

        public string PhotoId { get; set; }

        public string Accessory { get; set; }

        public string EmotionType { get; set; }

        public string FacialHair { get; set; }

        public string HairColor { get; set; }

        public int FaceNumber { get; set; }

        public string Url { get; set; }
    }
}
