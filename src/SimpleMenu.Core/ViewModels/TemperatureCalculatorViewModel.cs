using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class TemperatureCalculatorViewModel : CalculatorBaseViewModel
    {
        #region Protected Methods
        protected override Metric GetInitialFromMetric()
        {
            return Metric.Celsius;
        }

        protected override Metric GetInitialToMetric()
        {
            return Metric.Fahrenheit;
        }
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
