using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class ImageSearchListVewModelNavigationParams
    {
        #region Properties
        /// <summary>
        /// Gets or sets the action invoked when an image is selected.
        /// </summary>
        public Action<string> ImageSelectedCallback { get; set; }

        /// <summary>
        /// Gets or sets the image search criteria.
        /// </summary>
        public string SearchCriteria { get; set; }
        #endregion
    }

    public partial class ImageSearchListViewModel : ListBaseViewModel<ImageModel, ImageSearchListVewModelNavigationParams>
    {
        #region Fields
        private Action<string> _imageSelectedCallback;
        private readonly IMvxNavigationService _navigationService;
        private string _searchCriteria;
        #endregion

        #region Event Handlers
        private void HelpButton_Click()
        {
            MessagingService.Instance.Alert(new AlertConfig
            {
                Title = Resources.TitleHelp,
                Message = string.Format(Resources.MessageImageSearchResults, _searchCriteria),
                OkButtonText = Resources.ActionOk
            });
        }

        protected override void OnItemClicked(ImageModel item)
        {
            base.OnItemClicked(item);

            _navigationService.Close(this);

            _imageSelectedCallback?.Invoke(item.ImageUri);
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            if (Data.Count > 0)
                Data.Clear();

            var imageResults = await ImageOperations.Instance.SearchForImagesAsync(_searchCriteria, 10).ConfigureAwait(false);

            var results = await Task.WhenAll(imageResults.Select(i => LoadPictureFromWebAsync(i.ImageUri))).ConfigureAwait(false);

            InvokeOnMainThread(() => Data.AddRange(results.Where(r => r.Item2 != null).Select(r => new ImageModel { Image = r.Item2, ImageUri = r.Item1 })));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            HelpButtonClickCommand = new MvxCommand(HelpButton_Click);

            DataEmptyHint = Resources.HintNoImagesFound;
            Title = Resources.TitleImageSearch;
        }

        public override void Prepare(ImageSearchListVewModelNavigationParams parameter)
        {
            base.Prepare(parameter);

            _imageSelectedCallback = parameter.ImageSelectedCallback;
            _searchCriteria = parameter.SearchCriteria;

            LoadingHint = string.Format(Resources.MessagingImageSearching, _searchCriteria);
        }
        #endregion

        #region Constructors
        public ImageSearchListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private async Task<(string, byte[])> LoadPictureFromWebAsync(string imageUri)
        {
            try
            {
                var image = await ImageOperations.Instance.LoadImageFromWebAsync(new Uri(imageUri), 200, 200).ConfigureAwait(false);

                return (imageUri, image);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
        #endregion
    }
}
