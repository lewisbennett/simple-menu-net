using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Events;
using SimpleMenu.Core.Interfaces;
using SimpleMenu.Core.ViewModels.Base;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    public abstract partial class ListBaseViewModel : BaseViewModel
    {
        #region Fields
        private bool _shouldShowDataEmptyAction;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether data is currently being loaded.
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets whether the data empty action should be shown.
        /// </summary>
        public bool ShouldShowDataEmptyAction
        {
            get => _shouldShowDataEmptyAction;

            set
            {
                if (_shouldShowDataEmptyAction != value)
                {
                    _shouldShowDataEmptyAction = value;

                    ShowDataEmptyAction = _shouldShowDataEmptyAction && IsDataEmpty && !IsLoading;
                }
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

        /// <summary>
        /// Loads the next page of data.
        /// </summary>
        public virtual void LoadNextPage()
        {
        }
        #endregion

        #region Protected Methods
        protected virtual Task LoadInitialPageAsync()
            => Task.FromResult(true);

        protected virtual Task LoadNextPageAsync()
            => Task.FromResult(true);
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

    public abstract partial class ListBaseViewModel<TModel> : ListBaseViewModel
        where TModel : class
    {
        #region Events
        public event EventHandler<ItemClickedEventArgs<TModel>> ItemClicked;
        public event EventHandler<ItemClickedEventArgs<TModel>> ItemLongClicked;
        #endregion

        #region Event Handlers
        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsDataEmpty = Data.Count < 1;
            ShowDataEmptyAction = ShouldShowDataEmptyAction && IsDataEmpty && !IsLoading;

            for (var i = 0; i < Data.Count; i++)
            {
                var item = Data[i];

                if (item is IIndexable indexableModel)
                    indexableModel.Index = i;
            }
        }

        protected virtual void OnItemClicked(TModel item)
            => ItemClicked?.Invoke(this, new ItemClickedEventArgs<TModel>(item));

        protected virtual void OnItemLongClicked(TModel item)
            => ItemLongClicked?.Invoke(this, new ItemClickedEventArgs<TModel>(item));

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(ShowLoading):

                    if (ShowLoading)
                    {
                        MiddleMessage = LoadingHint;
                        ShowDataEmptyAction = false;
                    }
                    else
                    {
                        MiddleMessage = DataEmptyHint;
                        ShowDataEmptyAction = ShouldShowDataEmptyAction;
                    }

                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the initial page of data.
        /// </summary>
        public override async void LoadInitialPage()
        {
            base.LoadInitialPage();

            if (IsLoading)
                return;

            IsLoading = true;

            ShowLoading = IsDataEmpty;

            await LoadInitialPageAsync().ConfigureAwait(false);

            IsLoading = ShowLoading = false;
        }

        /// <summary>
        /// Loads the next page of data.
        /// </summary>
        public override async void LoadNextPage()
        {
            base.LoadNextPage();

            if (!IsLoading)
            {
                IsLoading = true;

                await LoadNextPageAsync().ConfigureAwait(false);

                IsLoading = false;
            }
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            Data = new MvxObservableCollection<TModel>();

            ItemClickCommand = new MvxCommand<TModel>(OnItemClicked);
            ItemLongClickCommand = new MvxCommand<TModel>(OnItemLongClicked);

            IsDataEmpty = true;

            Data.CollectionChanged += Data_CollectionChanged;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            if (viewFinishing)
                Data.CollectionChanged -= Data_CollectionChanged;
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
