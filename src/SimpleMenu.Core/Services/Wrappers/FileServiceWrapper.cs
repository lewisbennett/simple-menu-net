using MvvmCross;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Services.Wrappers
{
    public class FileServiceWrapper
    {
        #region Fields
        private readonly IFileService _fileService = Mvx.IoCProvider.Resolve<IFileService>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a directory, if it doesn't already exist.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public bool CreateDirectory(string directory)
        {
            var path = ConstructPath(directory);

            if (Directory.Exists(path))
                return true;

            var directoryInfo = Directory.CreateDirectory(path);

            return directoryInfo != null && Directory.Exists(path);
        }

        /// <summary>
        /// Create a directory, if it doesn't already exist.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public Task<bool> CreateDirectoryAsync(string directory)
            => Task.Run(() => CreateDirectory(directory));

        /// <summary>
        /// Gets whether a file exists.
        /// </summary>
        /// <param name="directory">The directory where the file is located, or null.</param>
        /// <param name="fileName">The name of the file, including file extension.</param>
        public bool DoesExist(string directory, string fileName)
            => Directory.Exists(ConstructPath(directory, fileName));

        /// <summary>
        /// Gets whether a file exists.
        /// </summary>
        /// <param name="directory">The directory where the file is located, or null.</param>
        /// <param name="fileName">The name of the file, including file extension.</param>
        public Task<bool> DoesExistAsync(string directory, string fileName)
            => Task.Run(() => DoesExist(directory, fileName));

        /// <summary>
        /// List all files in a directory.
        /// </summary>
        /// <param name="directory">The directory to list files from, or null.</param>
        public string[] ListFiles(string directory)
            => Directory.EnumerateFiles(ConstructPath(directory)).ToArray();

        /// <summary>
        /// List all files in a directory.
        /// </summary>
        /// <param name="directory">The directory to list files from, or null.</param>
        public Task<string[]> ListFilesAsync(string directory)
            => Task.Run(() => ListFiles(directory));

        /// <summary>
        /// Reads an image.
        /// </summary>
        /// <param name="directory">The directory where the image is stored, or null.</param>
        /// <param name="fileName">The name of the file to read.</param>
        public byte[] ReadImage(string directory, string fileName)
        {
            if (!fileName.EndsWith(".jpg"))
                fileName = $"{fileName}.jpg";

            var image = Image.Load(ConstructPath(directory, fileName));

            using var stream = new MemoryStream();

            image.Save(stream, image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(".jpg"));

            return stream.ToArray();
        }

        /// <summary>
        /// Reads an image.
        /// </summary>
        /// <param name="directory">The directory where the image is stored, or null.</param>
        /// <param name="fileName">The name of the file to read.</param>
        public async Task<byte[]> ReadImageAsync(string directory, string fileName)
        {
            if (!fileName.EndsWith(".jpg"))
                fileName = $"{fileName}.jpg";

            var image = await Image.LoadAsync(ConstructPath(directory, fileName)).ConfigureAwait(false);

            using var stream = new MemoryStream();

            image.Save(stream, image.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(".jpg"));

            return stream.ToArray();
        }

        /// <summary>
        /// Reads a JSON file.
        /// </summary>
        /// <param name="directory">The directory to read the file from, or null.</param>
        /// <param name="fileName">The name of the file to read.</param>
        public T ReadJson<T>(string directory, string fileName)
        {
            if (!fileName.EndsWith(".json"))
                fileName = $"{fileName}.json";

            var path = ConstructPath(directory, fileName);

            using var streamReaer = new StreamReader(path);

            return JsonSerializer.Deserialize<T>(streamReaer.ReadToEnd());
        }

        /// <summary>
        /// Reads a JSON file.
        /// </summary>
        /// <param name="directory">The directory to read the file from, or null.</param>
        /// <param name="fileName">The name of the file to read, not including file extension.</param>
        public Task<T> ReadJsonAsync<T>(string directory, string fileName)
            => Task.Run(() => ReadJson<T>(directory, fileName));

        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="directory">The durectory to save the image to, or null.</param>
        /// <param name="fileName">The name of the image to save.</param>
        /// <param name="image">The image to save.</param>
        public void SaveImage(string directory, string fileName, byte[] image)
        {
            if (!fileName.EndsWith(".jpg"))
                fileName = $"{fileName}.jpg";

            Image.Load(image).SaveAsJpeg(ConstructPath(directory, fileName));
        }

        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="directory">The durectory to save the image to, or null.</param>
        /// <param name="fileName">The name of the image to save.</param>
        /// <param name="image">The image to save.</param>
        public Task SaveImageAsync(string directory, string fileName, byte[] image)
        {
            if (!fileName.EndsWith(".jpg"))
                fileName = $"{fileName}.jpg";

            return Image.Load(image).SaveAsJpegAsync(ConstructPath(directory, fileName));
        }

        /// <summary>
        /// Saves a JSON file.
        /// </summary>
        /// <param name="directory">The directory to save the file to, or null.</param>
        /// <param name="fileName">The name of the file to save.</param>
        /// <param name="content">The content to serialize and save.</param>
        public void SaveJson<T>(string directory, string fileName, T content)
        {
            if (!fileName.EndsWith(".json"))
                fileName = $"{fileName}.json";

            var path = ConstructPath(directory, fileName);

            using var streamWriter = new StreamWriter(path);

            streamWriter.WriteLine(JsonSerializer.Serialize(content));
        }

        /// <summary>
        /// Saves a JSON file.
        /// </summary>
        /// <param name="directory">The directory to save the file to, or null.</param>
        /// <param name="fileName">The name of the file to save.</param>
        /// <param name="content">The content to serialize and save.</param>
        public Task SaveJsonAsync<T>(string directory, string fileName, T content)
            => Task.Run(() => SaveJson(directory, fileName, content));
        #endregion

        #region Private Methods
        private string ConstructPath(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return _fileService.StoragePath;
            else
                return Path.Combine(_fileService.StoragePath, directory);
        }

        private string ConstructPath(string directory, string fileName)
            => Path.Combine(ConstructPath(directory), fileName);
        #endregion

        #region Constant Values
        public const string ImagesDirectory = "images";
        public const string MealsDirectory = "meals";
        #endregion

        #region Static Properties
        public static FileServiceWrapper Instance { get; } = new FileServiceWrapper();
        #endregion
    }
}
