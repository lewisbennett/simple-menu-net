using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class TemperatureCalculatorViewModel : CalculatorBaseViewModel
    {
        #region Protected Methods
        protected override Metric GetInitialFromMetric()
            => Metric.Celsius;

        protected override Metric GetInitialToMetric()
            => Metric.Fahrenheit;
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            Title = Resources.TitleTemperatureCalculator;
        }
        #endregion
    }
}
