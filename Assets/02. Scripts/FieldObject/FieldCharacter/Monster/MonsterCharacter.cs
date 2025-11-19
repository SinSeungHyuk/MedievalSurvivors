using MS.Data;
using MS.Field;
using MS.Manager;
using UnityEngine;

namespace MS.Field
{
    public class MonsterCharacter : FieldCharacter
    {

        public void InitMonster(string _monsterKey)
        {
            ObjectType = FieldObjectType.Monster;
            ObjectLifeState = FieldObjectLifeState.Live;

            if (!DataManager.Instance.MonsterSettingDataDict.TryGetValue(_monsterKey, out MonsterSettingData _monsterData))
            {
                Debug.LogError($"InitMonster Key Missing : {_monsterKey}");
                return;
            }

            MonsterAttributeSet monsterAttributeSet = new MonsterAttributeSet();
            monsterAttributeSet.InitAttributeSet(_monsterData.AttributeSetSettingData);

            SSC.InitSkillActorInfo(this, monsterAttributeSet);
        }
    }
}