using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class WeightCalculatorViewModel : CalculatorBaseViewModel
    {
        #region Protected Methods
        protected override Metric GetInitialFromMetric()
            => Metric.Kilogram;

        protected override Metric GetInitialToMetric()
            => Metric.Pound;
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
