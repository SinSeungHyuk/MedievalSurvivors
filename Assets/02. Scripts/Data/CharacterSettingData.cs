using System;
using UnityEngine;


namespace MS.Data
{
    [Serializable]
    public class CharacterSettingData
    {
        public string CharacterName { get; set; }
        public AttributeSetSettingData AttributeSetSettingData { get; set; }
        public string DefaultSkillKey { get; set; } // 장착중인 기본스킬 키값
    }

    [Serializable]
    public class AttributeSetSettingData
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }

        public float AttackPower { get; set; }
        public float Defense { get; set; }

        public float CriticChance { get; set; }
        public float CriticMultiple { get; set; }
    }
}
