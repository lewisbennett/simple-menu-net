using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class WeightCalculatorViewModel : CalculatorBaseViewModel
    {
        #region Protected Methods
        protected override Metric GetInitialFromMetric()
        {
            return Metric.Kilogram;
        }

        protected override Metric GetInitialToMetric()
        {
            return Metric.Pound;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            Title = Resources.TitleWeightCalculator;
        }
        #endregion
    }
}
