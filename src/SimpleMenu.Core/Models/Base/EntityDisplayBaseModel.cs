using SimpleMenu.Core.Data.Entities.Base;
using System.ComponentModel;

namespace SimpleMenu.Core.Models.Base
{
    public abstract class EntityDisplayBaseModel<TEntity> : BaseModel
        where TEntity : BaseEntity
    {
        #region Fields
        private TEntity _entity;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the entity being displayed.
        /// </summary>
        public TEntity Entity
        {
            get => _entity;

            set
            {
                if (_entity == value)
                    return;

                if (_entity != null)
                    _entity.PropertyChanged -= OnEntityPropertyChanged;

                _entity = value;

                if (_entity != null)
                    _entity.PropertyChanged += OnEntityPropertyChanged;

                OnEntityChanged();
            }
        }
        #endregion

        #region Event Handlers
        protected virtual void OnEntityChanged()
        {
        }

        protected virtual void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
        #endregion
    }
}
