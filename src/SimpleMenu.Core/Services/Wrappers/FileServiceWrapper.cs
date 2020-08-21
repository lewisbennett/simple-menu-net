using MvvmCross;
using SimpleMenu.Core.Data.Entities.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace SimpleMenu.Core.Services.Wrappers
{
    public class FileServiceWrapper
    {
        #region Fields
        private readonly List<BaseEntity> _entities = new List<BaseEntity>();
        private readonly IFileService _fileService = Mvx.IoCProvider.Resolve<IFileService>();
        private bool _isSaving;
        private readonly Timer _saveTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current entities.
        /// </summary>
        public BaseEntity[] Entities
        {
            get
            {
                lock (_entities)
                    return _entities.ToArray();
            }
        }
        #endregion

        #region Event Handlers
        private void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_saveTimer.Enabled)
                _saveTimer.Enabled = true;
        }

        private async void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isSaving)
                return;

            _isSaving = true;

            await Task.Run(async () =>
            {
                foreach (var entity in Entities)
                {
                    if (entity.HasBeenChanged)
                    {
                        await entity.SaveAsync().ConfigureAwait(false);

                        entity.HasBeenChanged = false;
                    }
                }

            }).ConfigureAwait(false);

            _saveTimer.Enabled = _isSaving = false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an entity if it's not already present.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void AddEntity(BaseEntity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);

                entity.PropertyChanged += Entity_PropertyChanged;
            }
        }

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
        /// Deletes a file from a directory.
        /// </summary>
        /// <param name="directory">The directory from where to delete the file, or null.</param>
        /// <param name="fileName">The name of the file to delete, including file extension.</param>
        public void DeleteFile(string directory, string fileName)
            => File.Delete(ConstructPath(directory, fileName));

        /// <summary>
        /// Deletes a file from a directory.
        /// </summary>
        /// <param name="directory">The directory from where to delete the file, or null.</param>
        /// <param name="fileName">The name of the file to delete, including file extension.</param>
        public Task DeleteFileAsync(string directory, string fileName)
            => Task.Run(() => DeleteFile(directory, fileName));

        /// <summary>
        /// Gets whether a file exists.
        /// </summary>
        /// <param name="directory">The directory where the file is located, or null.</param>
        /// <param name="fileName">The name of the file, including file extension.</param>
        public bool DoesExist(string directory, string fileName)
        {
            var path = ConstructPath(directory, fileName);

            if (string.IsNullOrWhiteSpace(fileName))
                return Directory.Exists(path);
            else
                return File.Exists(path);
        }

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
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void RemoveEntity(BaseEntity entity)
        {
            _entities.Remove(entity);

            entity.PropertyChanged -= Entity_PropertyChanged;
        }

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

        #region Constructors
        public FileServiceWrapper()
        {
            _saveTimer.Elapsed += SaveTimer_Elapsed;
        }
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
