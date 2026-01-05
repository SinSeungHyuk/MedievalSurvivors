using MS.Data;
using MS.Manager;
using MS.UI;
using System.Collections.Generic;
using System.Linq;
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
                    var SkillDict = DataManager.Instance.SkillSettingDataDict;
                    var rewards = new List<string>();

                    var playerSkillKeys = SkillDict
                        .Where(x => x.Value.OwnerType == FieldObjectType.Player)
                        .Select(x => x.Key)
                        .ToList();

                    while (rewards.Count < 4 && playerSkillKeys.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, playerSkillKeys.Count);
                        rewards.Add(playerSkillKeys[randomIndex]);
                        playerSkillKeys.RemoveAt(randomIndex);
                    }

                    var popup = UIManager.Instance.ShowPopup<SkillRewardPopup>("SkillRewardPopup");
                    popup.InitSkillRewardPopup(rewards, _player);

                    break;
            }
        }
    }
}