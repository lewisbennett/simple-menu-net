using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Events;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.Base;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    public abstract class ListBaseViewModel : BaseViewModel
    {
        #region Fields
        private string _dataEmptyActionButtonText = string.Empty, _dataEmptyHint = Resources.HintNothingToDisplay, _loadingHint = Resources.MessagingLoading;
        private bool _shouldShowDataEmptyAction, _showLoading;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the command triggered when the data empty action button is clicked.
        /// </summary>
        public IMvxCommand DataEmptyActionButtonClickCommand { get; private set; }

        /// <summary>
        /// Gets or sets the text displayed on the data empty action button.
        /// </summary>
        public string DataEmptyActionButtonText
        {
            get => _dataEmptyActionButtonText;

            set
            {
                value ??= string.Empty;

                if (_dataEmptyActionButtonText.Equals(value))
                    return;

                _dataEmptyActionButtonText = value;
                RaisePropertyChanged(() => DataEmptyActionButtonText);
            }
        }

        /// <summary>
        /// Gets or sets the hint displayed when the data collection is empty.
        /// </summary>
        public string DataEmptyHint
        {
            get => _dataEmptyHint;

            set
            {
                value ??= string.Empty;

                if (_dataEmptyHint.Equals(value))
                    return;

                _dataEmptyHint = value;
                RaisePropertyChanged(() => MiddleMessage);
            }
        }

        /// <summary>
        /// Gets whether or not the data collection currently contains any items.
        /// </summary>
        public abstract bool IsDataEmpty { get; }

        /// <summary>
        /// Gets or sets whether data is currently being loaded.
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the text displayed when data is being loaded and the data collection is empty.
        /// </summary>
        public string LoadingHint
        {
            get => _loadingHint;

            set
            {
                value ??= string.Empty;

                if (_loadingHint.Equals(value))
                    return;

                _loadingHint = value;
                RaisePropertyChanged(() => MiddleMessage);
            }
        }

        /// <summary>
        /// Gets the message displayed in the middle of the screen.
        /// </summary>
        public string MiddleMessage => ShowLoading ? LoadingHint : DataEmptyHint;

        /// <summary>
        /// Gets or sets whether to show the action when the data is empty.
        /// </summary>
        public bool ShouldShowDataEmptyAction
        {
            get => _shouldShowDataEmptyAction;

            set
            {
                if (_shouldShowDataEmptyAction == value)
                    return;

                _shouldShowDataEmptyAction = value;
                RaisePropertyChanged(() => ShowDataEmptyAction);
            }
        }

        /// <summary>
        /// Gets whether the data empty action should be visible.
        /// </summary>
        public bool ShowDataEmptyAction => IsDataEmpty && !IsLoading;

        /// <summary>
        /// Gets or sets whether data is currently being loaded.
        /// </summary>
        public bool ShowLoading
        {
            get => _showLoading;

            set
            {
                if (_showLoading == value)
                    return;

                _showLoading = value;

                RaisePropertyChanged(() => ShowLoading);
                RaisePropertyChanged(() => MiddleMessage);
                RaisePropertyChanged(() => ShowDataEmptyAction);
            }
        }
        #endregion

        #region Event Handlers
        protected virtual void OnDataEmptyActionButtonClick()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the initial page of data.
        /// </summary>
        public virtual void LoadInitialPage()
        {
        }

        public virtual Task LoadInitialPageAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Loads the next page of data.
        /// </summary>
        public virtual void LoadNextPage()
        {
        }

        public virtual Task LoadNextPageAsync()
        {
            return Task.FromResult(true);
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            DataEmptyActionButtonClickCommand = new MvxCommand(OnDataEmptyActionButtonClick);
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            LoadInitialPage();
        }
        #endregion
    }

    public abstract class ListBaseViewModel<TModel> : ListBaseViewModel
        where TModel : class
    {
        #region Events
        public event EventHandler<ItemClickedEventArgs<TModel>> ItemClicked;
        public event EventHandler<ItemClickedEventArgs<TModel>> ItemLongClicked;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the display data.
        /// </summary>
        public MvxObservableCollection<TModel> Data { get; } = new MvxObservableCollection<TModel>();

        /// <summary>
        /// Gets whether or not the data collection currently contains any items.
        /// </summary>
        public override bool IsDataEmpty => Data.Count < 1;

        /// <summary>
        /// Gets the command triggered when an item in the collection is clicked.
        /// </summary>
        public IMvxCommand ItemClickCommand { get; private set; }

        /// <summary>
        /// Gets the command triggered when an item in the collection is long clicked.
        /// </summary>
        public IMvxCommand ItemLongClickCommand { get; private set; }
        #endregion

        #region Event Handlers
        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => IsDataEmpty);
            RaisePropertyChanged(() => ShowDataEmptyAction);
        }

        protected virtual void OnItemClicked(TModel item)
        {
            ItemClicked?.Invoke(this, new ItemClickedEventArgs<TModel>(item));
        }

        protected virtual void OnItemLongClicked(TModel item)
        {
            ItemLongClicked?.Invoke(this, new ItemClickedEventArgs<TModel>(item));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the event handlers for the ViewModel.
        /// </summary>
        public override void AddEventHandlers()
        {
            base.AddEventHandlers();

            Data.CollectionChanged += Data_CollectionChanged;
        }

        /// <summary>
        /// Loads the initial page of data.
        /// </summary>
        public override async void LoadInitialPage()
        {
            base.LoadInitialPage();

            if (IsLoading)
                return;

            IsLoading = true;

            if (IsDataEmpty)
                ShowLoading = true;

            await LoadInitialPageAsync().ConfigureAwait(false);

            IsLoading = false;
            ShowLoading = false;
        }

        /// <summary>
        /// Loads the next page of data.
        /// </summary>
        public override async void LoadNextPage()
        {
            base.LoadNextPage();

            if (!IsLoading)
                await LoadNextPageAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the event handlers for the ViewModel.
        /// </summary>
        public override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            Data.CollectionChanged -= Data_CollectionChanged;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            ItemClickCommand = new MvxCommand<TModel>(OnItemClicked);
            ItemLongClickCommand = new MvxCommand<TModel>(OnItemLongClicked);
        }
        #endregion
    }

    public abstract class ListBaseViewModel<TModel, TNavigationParams> : ListBaseViewModel<TModel>, IMvxViewModel<TNavigationParams>
        where TModel : class
        where TNavigationParams : class
    {
        #region Lifecycle
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
