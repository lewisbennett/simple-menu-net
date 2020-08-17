using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Helper;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using System;

namespace SimpleMenu.Core.ViewModels.Base
{
    public abstract class CalculatorBaseViewModel : BaseViewModel
    {
        #region Fields
        private Metric _fromMetric, _toMetric;
        private string _fromUnits = string.Empty, _toUnits = string.Empty;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the from metric.
        /// </summary>
        public Metric FromMetric
        {
            get => _fromMetric;

            set
            {
                if (_fromMetric == value)
                    return;

                _fromMetric = value;

                RaisePropertyChanged(() => FromMetricButtonText);
                RecalculateFrom();
            }
        }

        /// <summary>
        /// Gets the command invoked when the from metric button is clicked.
        /// </summary>
        public IMvxCommand FromMetricButtonClickCommand { get; private set; }

        /// <summary>
        /// Gets the text displayed on the from metric button.
        /// </summary>
        public string FromMetricButtonText => FromMetric.GetTitle();

        /// <summary>
        /// Gets or sets the units being converted from.
        /// </summary>
        public string FromUnits
        {
            get => _fromUnits;

            set
            {
                value ??= string.Empty;

                if (_fromUnits.Equals(value))
                    return;

                _fromUnits = value;

                RaisePropertyChanged(() => FromUnits);
                RecalculateFrom();
            }
        }

        /// <summary>
        /// Gets the placeholder text for the from units field.
        /// </summary>
        public string FromUnitsHint => Resources.HintFrom;

        /// <summary>
        /// Gets the messaging service.
        /// </summary>
        public IMessagingService MessagingService => DialogMessaging.MessagingService.Instance;

        /// <summary>
        /// Gets the command triggered when the swap button is clicked.
        /// </summary>
        public IMvxCommand SwapButtonClickCommand { get; private set; }

        /// <summary>
        /// Gets or sets the to metric.
        /// </summary>
        public Metric ToMetric
        {
            get => _toMetric;

            set
            {
                if (_toMetric == value)
                    return;

                _toMetric = value;

                RaisePropertyChanged(() => ToMetricButtonText);
                RecalculateFrom();
            }
        }

        /// <summary>
        /// Gets the command invoked when the to metric button is clicked.
        /// </summary>
        public IMvxCommand ToMetricButtonClickCommand { get; private set; }

        /// <summary>
        /// Gets the text displayed on the to metric button.
        /// </summary>
        public string ToMetricButtonText => ToMetric.GetTitle();

        /// <summary>
        /// Gets or sets the units being converted to.
        /// </summary>
        public string ToUnits
        {
            get => _toUnits;

            set
            {
                value ??= string.Empty;

                if (_toUnits.Equals(value))
                    return;

                _toUnits = value;

                RaisePropertyChanged(() => ToUnits);
                RecalculateTo();
            }
        }

        /// <summary>
        /// Gets the placeholder text for the to units field.
        /// </summary>
        public string ToUnitsHint => Resources.HintTo;
        #endregion

        #region Event Handlers
        private void FromMetricButton_Click()
        {
            ShowChangeMetric(GetInitialFromMetric(), ToMetric, (metric) => FromMetric = metric);
        }

        private void SwapButton_Click()
        {
            SwapMetrics();
        }

        private void ToMetricButton_Click()
        {
            ShowChangeMetric(GetInitialToMetric(), FromMetric, (metric) => ToMetric = metric);
        }
        #endregion

        #region Protected Methods
        protected abstract Metric GetInitialFromMetric();

        protected abstract Metric GetInitialToMetric();
        #endregion

        #region Public Methods
        /// <summary>
        /// Swaps the metrics.
        /// </summary>
        public void SwapMetrics()
        {
            (FromMetric, ToMetric) = (ToMetric, FromMetric);
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            FromMetricButtonClickCommand = new MvxCommand(FromMetricButton_Click);
            SwapButtonClickCommand = new MvxCommand(SwapButton_Click);
            ToMetricButtonClickCommand = new MvxCommand(ToMetricButton_Click);

            FromMetric = GetInitialFromMetric();
            ToMetric = GetInitialToMetric();

            _fromUnits = "1";
            _toUnits = "1";

            RecalculateFrom();
        }
        #endregion

        #region Private Methods
        private void RecalculateFrom()
        {
            var fromParsed = double.TryParse(FromUnits, out double from);

            SetToWithoutRecalculating((!fromParsed || from == 0 ? 0 : MetricHelper.Convert(FromMetric, from, ToMetric)).ToString());
        }

        private void RecalculateTo()
        {
            var toParsed = double.TryParse(ToUnits, out double to);

            SetFromWithoutRecalculating((!toParsed || to == 0 ? 0 : MetricHelper.Convert(ToMetric, to, FromMetric)).ToString());
        }
        
        private void SetFromWithoutRecalculating(string value)
        {
            _fromUnits = value;
            RaisePropertyChanged(() => FromUnits);
        }

        private void SetToWithoutRecalculating(string value)
        {
            _toUnits = value;
            RaisePropertyChanged(() => ToUnits);
        }

        private void ShowChangeMetric(Metric initialMetric, Metric oppositeMetric, Action<Metric> metricSelectedAction)
        {
            var config = new ActionSheetBottomConfig
            {
                Title = Resources.TitleChooseMetric,
                CancelButtonText = Resources.ActionCancel
            };

            foreach (var m in Enum.GetValues(typeof(Metric)))
            {
                var metric = (Metric)m;

                if (initialMetric.IsCompatible(metric))
                    config.Items.Add(new ActionSheetItemConfig { Text = metric.GetTitle(), Data = metric });
            }

            config.ItemClickAction = (item) =>
            {
                if (item.Data is Metric metric)
                {
                    if (metric == oppositeMetric)
                        SwapMetrics();
                    else
                        metricSelectedAction?.Invoke(metric);
                }
            };

            MessagingService.ActionSheetBottom(config);
        }
        #endregion
    }

    public abstract class CalculatorBaseViewModel<TNavigationParams> : CalculatorBaseViewModel, IMvxViewModel<TNavigationParams>
        where TNavigationParams : class
    {
        #region Lifecycle
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
