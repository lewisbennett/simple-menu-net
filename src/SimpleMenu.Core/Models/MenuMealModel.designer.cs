﻿namespace SimpleMenu.Core.Models
{
    partial class MenuMealModel
    {
        #region Fields
        private string _dateTitle, _description, _title;
        private byte[] _image;
        private bool _showDescription, _showLoading;
        #endregion

        #region Properties
        public string DateTitle
        {
            get => _dateTitle;

            set => SetProperty(ref _dateTitle, value);
        }

        public string Description
        {
            get => _description;

            set => SetProperty(ref _description, value);
        }

        public byte[] Image
        {
            get => _image;

            set => SetProperty(ref _image, value);
        }

        public bool ShowDescription
        {
            get => _showDescription;

            set => SetProperty(ref _showDescription, value);
        }

        public bool ShowLoading
        {
            get => _showLoading;

            set => SetProperty(ref _showLoading, value);
        }

        public string Title
        {
            get => _title;

            set => SetProperty(ref _title, value);
        }
        #endregion
    }
}
