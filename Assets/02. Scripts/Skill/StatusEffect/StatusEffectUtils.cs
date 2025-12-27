using MS.Field;
using UnityEngine;


namespace MS.Skill
{
    public static class StatusEffectUtils
    {
        public static void ApplyStatEffect(this FieldCharacter _target, string _key, EStatType _statType, float _value, EBonusType _bonusType, float _duration = -1)
        {
            StatusEffect effect = new StatusEffect();
            effect.InitStatusEffect(_duration);

            Stat stat = _target.SSC.AttributeSet.GetStatByType(_statType);
            if (stat == null) return;

            effect.OnStatusStartCallback += () =>
            {
                stat.AddBonusStat(_key, _bonusType, _value);
            };

            effect.OnStatusEndCallback += () =>
            {
                stat.RemoveBonusStat(_key);
            };

            _target.SSC.ApplyStatusEffect(_key, effect);
        }
    }
}