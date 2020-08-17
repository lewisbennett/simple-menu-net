using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SkiaSharp;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SixLabors_Image = SixLabors.ImageSharp.Image;
using SKExtended_Svg = SkiaSharp.Extended.Svg.SKSvg;

namespace SimpleMenu.Core.Data.Operations
{
    public class ImageOperations
    {
        #region Public Methods
        /// <summary>
        /// Gets an image as a byte array from a web address.
        /// </summary>
        /// <param name="uri">The web address where the image is hosted.</param>
        public async Task<byte[]> LoadPictureFromWebAsync(Uri uri)
        {
            var uriString = uri.ToString();

            byte[] image;

            using (var webClient = new WebClient())
                image = await webClient.DownloadDataTaskAsync(uri).ConfigureAwait(false);

            if (uriString.EndsWith(".svg"))
                image = ParseSvgImage(image);
            else
                image = ParseImage(uriString.Substring(uriString.Length - 4), image);

            return image;
        }
        #endregion

        #region Private Methods
        private (int, int, double) CalculateWidthAndHeight(int originalWidth, int originalHeight)
        {
            if (originalWidth < MaxImageDimension && originalHeight < MaxImageDimension)
                return (originalWidth, originalHeight, 1);

            var largestOriginalSide = (double)Math.Max(originalWidth, originalHeight);

            var scale = MaxImageDimension / largestOriginalSide;

            return ((int)(originalWidth * scale), (int)(originalHeight * scale), scale);
        }

        private byte[] ParseImage(string imageFormat, byte[] rawImage)
        {
            using var image = SixLabors_Image.Load(rawImage);

            var dimensions = CalculateWidthAndHeight(image.Width, image.Height);

            // If Item3 is 1 the image doesn't need to be altered, return it.
            if (dimensions.Item3 == 1)
                return rawImage;

            image.Mutate(i => i.Resize(dimensions.Item1, dimensions.Item2));

            using var stream = new MemoryStream();

            image.Save(stream, image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(imageFormat));

            return stream.ToArray();
        }

        private byte[] ParseSvgImage(byte[] rawImage)
        {
            SKPicture picture;

            using (var memoryStream = new MemoryStream(rawImage))
                picture = new SKExtended_Svg().Load(memoryStream);

            var dimensions = CalculateWidthAndHeight((int)Math.Ceiling(picture.CullRect.Width), (int)Math.Ceiling(picture.CullRect.Height));

            using var finalImageMemoryStream = new MemoryStream();

            SKImage.FromPicture(picture, new SKSizeI(dimensions.Item1, dimensions.Item2), SKMatrix.MakeScale((float)dimensions.Item3, (float)dimensions.Item3)).Encode(SKEncodedImageFormat.Png, 100).SaveTo(finalImageMemoryStream);

            return finalImageMemoryStream.ToArray();
        }
        #endregion

        #region Constant Values
        public const int MaxImageDimension = 1080;
        #endregion

        #region Static Properties
        public static ImageOperations Instance { get; } = new ImageOperations();
        #endregion
    }
}
