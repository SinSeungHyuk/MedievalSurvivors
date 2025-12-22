using MS.Data;
using MS.Field;
using MS.Manager;
using UnityEngine;


namespace MS.Field
{
    public abstract class FieldItem : FieldObject
    {
        private string fieldItemKey;
        private string gameplayCueKey;
        protected EItemType itemType;

        public string FieldItemKey => fieldItemKey;


        public virtual void InitFieldItem(string _itemKey, ItemSettingData _itemData)
        {
            fieldItemKey = _itemKey;
            gameplayCueKey = _itemData.GameplayCueKey;
            itemType = _itemData.ItemType;

            ObjectLifeState = FieldObjectLifeState.Live;
            ObjectType = FieldObjectType.FieldItem;
        }

        protected abstract void OnAcquire(PlayerCharacter _player);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCharacter>(out PlayerCharacter player))
            {
                // °øÅë È¹µæ ·ÎÁ÷À» ½ÇÇà
                GameplayCueManager.Instance.PlayCue(gameplayCueKey, player);
                OnAcquire(player);
                ObjectPoolManager.Instance.Return(fieldItemKey, this.gameObject);
            }
        }
    }
}