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
    public abstract partial class CalculatorBaseViewModel : BaseViewModel
    {
        #region Fields
        private bool _canRecalculateFrom, _canRecalculateTo;
        private Metric _fromMetric, _toMetric;
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
                if (_fromMetric != value)
                {
                    _fromMetric = value;

                    RecalculateFrom();
                }
            }
        }

        /// <summary>
        /// Gets or sets the to metric.
        /// </summary>
        public Metric ToMetric
        {
            get => _toMetric;

            set
            {
                if (_toMetric != value)
                {
                    _toMetric = value;

                    RecalculateTo();
                }
            }
        }
        #endregion

        #region Event Handlers
        private void FromMetricButton_Click()
        {
            ShowChangeMetric(GetInitialFromMetric(), ToMetric, (metric) => FromMetric = metric);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(FromUnits):

                    if (_canRecalculateFrom)
                        RecalculateFrom();
                    else
                        _canRecalculateFrom = true;

                    return;

                case nameof(ToUnits):

                    if (_canRecalculateTo)
                        RecalculateTo();
                    else
                        _canRecalculateTo = true;

                    return;

                default:
                    return;
            }
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

            _fromUnits = _toUnits = "1";

            RecalculateFrom();
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            FromUnitsHint = Resources.HintFrom;
            ToUnitsHint = Resources.HintTo;
        }
        #endregion

        #region Private Methods
        private void RecalculateFrom()
        {
            FromMetricButtonText = FromMetric.GetTitle();

            var fromParsed = double.TryParse(FromUnits, out double from);

            _canRecalculateTo = false;

            ToUnits = (!fromParsed || from == 0 ? 0 : MetricHelper.Convert(FromMetric, from, ToMetric)).ToString();
        }

        private void RecalculateTo()
        {
            ToMetricButtonText = ToMetric.GetTitle();

            var toParsed = double.TryParse(ToUnits, out double to);

            _canRecalculateFrom = false;

            FromUnits = (!toParsed || to == 0 ? 0 : MetricHelper.Convert(ToMetric, to, FromMetric)).ToString();
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

            MessagingService.Instance.ActionSheetBottom(config);
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
