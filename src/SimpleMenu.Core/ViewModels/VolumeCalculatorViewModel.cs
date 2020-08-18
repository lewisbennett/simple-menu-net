using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class VolumeCalculatorViewModel : CalculatorBaseViewModel
    {
        #region Protected Methods
        protected override Metric GetInitialFromMetric()
            => Metric.Liter;

        protected override Metric GetInitialToMetric()
            => Metric.Gallon;
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            Title = Resources.TitleVolumeCalculator;
        }
        #endregion
    }
}
