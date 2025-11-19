using UnityEngine;


namespace MS.Utils
{
    public static class Settings
    {
        #region LAYERMASK SETTING
        public static int BattleScalingConstant = 100;
        #endregion

        #region LAYERMASK SETTING
        public static LayerMask MonsterLayer = LayerMask.GetMask("Monster"); // 몬스터 레이어
        public static LayerMask PlayerLayer = LayerMask.GetMask("Player"); // 플레이어 레이어
        #endregion
    }
}