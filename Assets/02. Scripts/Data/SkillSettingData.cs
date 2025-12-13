using MS.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Data
{
    [Serializable]
    public class SkillSettingData
    {
        public string SkillName { get; set; }
        public string Description { get; set; }
        public EDamageAttributeType AttributeType { get; set; }
        public float Cooltime { get; set; }
        public Dictionary<ESkillValueType, float> SkillValueDict { get; set; }
    }

    public enum ESkillValueType
    {
        Default,            // 기본 수치 (예: 기본데미지 n)
        Damage,             // 기본 공격력 대비 데미지 계수 (예: 1.5)
        Move,               // 이동 스킬의 이동 거리
        Buff,               // 버프 효과 배율 (예: 0.15 = 15% 증가)
        Duration,           // 지속 시간 (예: 버프 지속 시간)
        Casting,            // 캐스팅 시간
        Delay,              // 스킬 지연시간 (예: 1초 후 폭발)
    }
}
