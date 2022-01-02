using ImageWizard.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.OpenCvSharp.Filters
{
    public class DetecFacesFilter : OpenCvSharpFilter
    {
        [Filter]
        public void DetectFaces()
        {
            //using var haarCascade = new CascadeClassifier(TextPath.HaarCascade);
            //using var lbpCascade = new CascadeClassifier(TextPath.LbpCascade);

            //// Detect faces
            //Mat haarResult = InternalDetectFace(haarCascade);
            //Mat lbpResult = InternalDetectFace(lbpCascade);

        }

        private Mat InternalDetectFace(CascadeClassifier cascade)
        {
            Mat result;

            using var src = Mat.FromStream(Context.Result.Data, ImreadModes.Unchanged);
            using var gray = new Mat();
            
            result = src.Clone();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Detect faces
            Rect[] faces = cascade.DetectMultiScale(
                gray, 1.08, 2, HaarDetectionTypes.ScaleImage, new Size(30, 30));

            // Render all detected faces
            foreach (Rect face in faces)
            {
                var center = new Point
                {
                    X = (int)(face.X + face.Width * 0.5),
                    Y = (int)(face.Y + face.Height * 0.5)
                };
                var axes = new Size
                {
                    Width = (int)(face.Width * 0.5),
                    Height = (int)(face.Height * 0.5)
                };
                Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
            }
            
            return result;
        }
    }
}
