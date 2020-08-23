using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.List.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class TimesOfDayListViewModel : ListBaseViewModel<TimeOfDayModel>
    {
        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            var settings = await SettingsOperations.Instance.GetSettingsAsync().ConfigureAwait(false);

            InvokeOnMainThread(() => UpdateCollection(settings.TimesOfDay));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            LoadingHint = Resources.MessagingLoadingTimesOfDay;
            Title = Resources.TitleTimesOfDay;
        }
        #endregion

        #region Private Methods
        private void UpdateCollection(IEnumerable<TimeOfDayEntity> timesOfDay)
        {
            if (Data.Count > 0)
                Data.Clear();

            Data.AddRange(timesOfDay.OrderBy(t => t.TimeOfDay).Select(t => new TimeOfDayModel { Entity = t }));
        }
        #endregion
    }
}
