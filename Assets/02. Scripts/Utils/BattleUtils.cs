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
    }
}