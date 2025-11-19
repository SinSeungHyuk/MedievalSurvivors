using MS.Skill;
using System;
using UnityEngine;


namespace MS.Data
{
    [Serializable]
    public class MonsterSettingData
    {
        public string MonsterName { get; set; }
        public MonsterAttributeSetSettingData AttributeSetSettingData { get; set; }
        public EDamageAttributeType WeaknessAttributeType { get; set; } // 약점 속성
        public string DefaultSkillKey { get; set; } // 장착중인 기본스킬 키값
    }

    [Serializable]
    public class MonsterAttributeSetSettingData
    {
        public float MaxHealth { get; set; }
        public float AttackPower { get; set; }
        public float Defense { get; set; }
        public float MoveSpeed { get; set; }
        public float DropEXP { get; set; }
        public float DropItem { get; set; }
    }
}
