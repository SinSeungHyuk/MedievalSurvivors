using MS.Data;
using UnityEngine;


namespace MS.Utils
{
    public static class BattleUtils
    {
        // 방어력 계산
        public static float CalcDefenseStat(float _damage, float _defense)
        {
            float finalDamage = _damage;

            float defensePercent = MathUtils.BattleScaling(_defense);
            finalDamage = MathUtils.DecreaseByPercent(finalDamage, defensePercent);

            return finalDamage;
        }

        // 스킬 기본데미지 계산
        public static float CalcSkillBaseDamage(float _attackPower, SkillSettingData _skillData)
        {
            float damage = _skillData.GetValue(ESkillValueType.Damage);
            float baseDamage = _skillData.GetValue(ESkillValueType.Default);

            return (_attackPower * damage) + baseDamage;
        }

        // 스킬 치명타 계산 (플레이어만 호출할 함수. 몬스터는 치명타가 없음)
        public static bool CalcSkillCriticDamage(float _baseDamage, float _criticChance, float _criticMultiple, out float _resultDamage)
        {
            _resultDamage = _baseDamage;
            
            if (_criticChance > 0)
            {
                if (!MathUtils.IsSuccess(_criticChance))
                    return false;
            }

            _resultDamage = _baseDamage * _criticMultiple;
            return true;
        }
    }
}