using System.ComponentModel;

namespace SimpleMenu.Core.Models.Base
{
    public abstract class EntityDisplayBaseModel<TEntity> : BaseModel
        where TEntity : class, INotifyPropertyChanged
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
                if (_entity != value)
                {
                    OnEntityChanging(_entity, value);

                    _entity = value;

                    OnEntityChanged();
                }
            }
        }
        #endregion

        #region Event Handlers
        private void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnEntityPropertyChanged(e.PropertyName);
        }

        protected virtual void OnEntityChanged()
        {
        }

        protected virtual void OnEntityChanging(TEntity oldEntity, TEntity newEntity)
        {
            if (oldEntity != null)
                oldEntity.PropertyChanged -= Entity_PropertyChanged;

            if (newEntity != null)
                newEntity.PropertyChanged += Entity_PropertyChanged;
        }

        protected virtual void OnEntityPropertyChanged(string propertyName)
        {
        }
        #endregion
    }
}
