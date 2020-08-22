using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Services.Wrappers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SixLabors_Image = SixLabors.ImageSharp.Image;
using SKExtended_Svg = SkiaSharp.Extended.Svg.SKSvg;

namespace SimpleMenu.Core.Data.Operations
{
    public class ImageOperations
    {
        #region Fields
        private readonly Dictionary<string, byte[]> _imageCache = new Dictionary<string, byte[]>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets an image.
        /// </summary>
        /// <param name="imageUuid">The UUID of the image to get.</param>
        public async ValueTask<byte[]> GetImageAsync(Guid imageUuid)
        {
            var imageCacheName = $"{imageUuid}";

            if (_imageCache.TryGetValue(imageCacheName, out byte[] image))
                return image;

            image = await FileServiceWrapper.Instance.ReadImageAsync(FileServiceWrapper.ImagesDirectory, imageUuid.ToString()).ConfigureAwait(false);

            _imageCache[imageCacheName] = image;

            return image;
        }

        /// <summary>
        /// Gets an image.
        /// </summary>
        /// <param name="imageUuid">The UUID of the image to get.</param>
        /// <param name="maxWidth">The maximum width of the image.</param>
        /// <param name="maxHeight">The maximum height of the image.</param>
        public async ValueTask<byte[]> GetImageAsync(Guid imageUuid, int maxWidth, int maxHeight)
        {
            var imageCacheName = $"{imageUuid}_{maxWidth}_{maxHeight}";

            if (_imageCache.TryGetValue(imageCacheName, out byte[] image))
                return image;

            image = await GetImageAsync(imageUuid).ConfigureAwait(false);

            image = ParseImage(".jpg", image, maxWidth, maxHeight);

            _imageCache[imageCacheName] = image;

            return image;
        }

        /// <summary>
        /// Gets an image as a byte array from a web address.
        /// </summary>
        /// <param name="uri">The web address where the image is hosted.</param>
        public async Task<byte[]> LoadImageFromWebAsync(Uri uri, int? maxWidth = null, int? maxHeight = null)
        {
            var actualWidth = maxWidth ?? MaxImageDimension;
            var actualHeight = maxHeight ?? MaxImageDimension;

            var uriString = uri.ToString();

            byte[] image;

            using (var webClient = new WebClient())
                image = await webClient.DownloadDataTaskAsync(uri).ConfigureAwait(false);

            if (uriString.EndsWith(".svg"))
                image = ParseSvgImage(image, actualWidth, actualHeight);
            else
                image = ParseImage(uriString.Substring(uriString.Length - 4), image, actualWidth, actualHeight);

            return image;
        }

        /// <summary>
        /// Searches for images matching a criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        public async Task<WebImageEntity[]> SearchForImagesAsync(string searchCriteria, int imageCount = 50)
        {
            using var httpClient = new HttpClient();

            var locale = "en_US";
            var safeSearch = 1;

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.qwant.com/api/search/images?count={imageCount}&q={searchCriteria}&t=images&safesearch={safeSearch}&locale={locale}&uiv=4");

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new WebException($"Invalid response: {response.StatusCode}");

            var responseStr = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var responseJson = JsonDocument.Parse(responseStr);

            var dataJson = responseJson.RootElement.GetProperty("data");

            var resultJson = dataJson.GetProperty("result");

            return resultJson.GetProperty("items").EnumerateArray().Select(r => new WebImageEntity
            {
                ImageUri = r.GetProperty("media").GetString()

            }).ToArray();
        }
        #endregion

        #region Private Methods
        private (int, int, double) CalculateWidthAndHeight(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            if (originalWidth < maxWidth && originalHeight < maxHeight)
                return (originalWidth, originalHeight, 1);

            var largestOriginalSide = (double)Math.Max(originalWidth, originalHeight);
            var largestNewSide = (double)Math.Max(maxWidth, maxHeight);

            var scale = largestNewSide / largestOriginalSide;

            return ((int)(originalWidth * scale), (int)(originalHeight * scale), scale);
        }

        private byte[] ParseImage(string imageFormat, byte[] rawImage, int maxWidth, int maxHeight)
        {
            using var image = SixLabors_Image.Load(rawImage);

            if (image.Width <= maxWidth && image.Height <= maxHeight)
                return rawImage;

            var dimensions = CalculateWidthAndHeight(image.Width, image.Height, maxWidth, maxHeight);

            image.Mutate(i => i.Resize(dimensions.Item1, dimensions.Item2));

            using var stream = new MemoryStream();

            image.Save(stream, image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(imageFormat));

            return stream.ToArray();
        }

        private byte[] ParseSvgImage(byte[] rawImage, int maxWidth, int maxHeight)
        {
            SKPicture picture;

            using (var memoryStream = new MemoryStream(rawImage))
                picture = new SKExtended_Svg().Load(memoryStream);

            var dimensions = CalculateWidthAndHeight((int)Math.Ceiling(picture.CullRect.Width), (int)Math.Ceiling(picture.CullRect.Height), maxWidth, maxHeight);

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
