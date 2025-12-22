using MS.Data;
using UnityEngine;
using UnityEngine.UIElements;



namespace MS.Field
{
    public class InteractionItem : FieldItem
    {
        protected override void OnAcquire(PlayerCharacter _player)
        {
            if (_player == null)
            {
                Debug.Log("OnAcquire :: Player null");
                return;
            }

            switch (itemType)
            {
                case EItemType.BossChest:
                    
                    break;
            }
        }
    }
}