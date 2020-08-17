using MvvmCross;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Core.ViewModels.List;

namespace SimpleMenu.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets the ingredients list view model.
        /// </summary>
        public IngredientsListViewModel IngredientsListViewModel { get; private set; }

        /// <summary>
        /// Gets the meal list view model.
        /// </summary>
        public MealListViewModel MealListViewModel { get; private set; }

        /// <summary>
        /// Gets the menu list view model.
        /// </summary>
        public MenuListViewModel MenuListViewModel { get; private set; }

        /// <summary>
        /// Gets the temperature calculator view model.
        /// </summary>
        public TemperatureCalculatorViewModel TemperatureCalculatorViewModel { get; private set; }

        /// <summary>
        /// Gets the volume calculator view model.
        /// </summary>
        public VolumeCalculatorViewModel VolumeCalculatorViewModel { get; private set; }

        /// <summary>
        /// Gets the weight calculator view model.
        /// </summary>
        public WeightCalculatorViewModel WeightCalculatorViewModel { get; private set; }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            IngredientsListViewModel = Mvx.IoCProvider.IoCConstruct<IngredientsListViewModel>();
            MealListViewModel = Mvx.IoCProvider.IoCConstruct<MealListViewModel>();
            MenuListViewModel = Mvx.IoCProvider.IoCConstruct<MenuListViewModel>();
            TemperatureCalculatorViewModel = Mvx.IoCProvider.IoCConstruct<TemperatureCalculatorViewModel>();
            VolumeCalculatorViewModel = Mvx.IoCProvider.IoCConstruct<VolumeCalculatorViewModel>();
            WeightCalculatorViewModel = Mvx.IoCProvider.IoCConstruct<WeightCalculatorViewModel>();

            IngredientsListViewModel.Prepare();
            MealListViewModel.Prepare();
            MenuListViewModel.Prepare();
            TemperatureCalculatorViewModel.Prepare();
            VolumeCalculatorViewModel.Prepare();
            WeightCalculatorViewModel.Prepare();
        }
        #endregion
    }
}
