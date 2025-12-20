using DG.Tweening;
using MS.Data;
using UnityEngine;


namespace MS.Field
{
    public class ResourceFieldItem : FieldItem
    {
        private float itemValue;
        private EItemType itemType;
        private Tween rotationTween;


        public override void InitFieldItem(string _key, ItemSettingData _data)
        {
            base.InitFieldItem(_key, _data);

            itemValue = _data.ItemValue;
            itemType = _data.ItemType;

            transform.position = new Vector3(Position.x, Position.y + 0.5f, Position.z);

            if (_data.ItemType == EItemType.Coin)
            {
                rotationTween = transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart);
            }
        }

        protected override void OnAcquire(PlayerCharacter _player)
        {
            if (_player == null)
            {
                Debug.Log("OnAcquire :: Player null");
                return;
            }

            switch (itemType)
            {
                case EItemType.Coin:
                    _player.LevelSystem.CurExp += itemValue;
                    break;
                case EItemType.RedCrystal:
                    // _player.Heal(itemValue);
                    break;
                case EItemType.GreenCrystal:
                    // _player.Heal(itemValue);
                    break;
                case EItemType.BlueCrystal:
                    // _player.Heal(itemValue);
                    break;
            }

            if (rotationTween != null)
            {
                rotationTween.Kill();
                rotationTween = null;
            }
        }
    }
}