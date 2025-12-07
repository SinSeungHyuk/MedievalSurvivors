using UnityEngine;


namespace MS.Utils
{
    public static class Settings
    {
        #region BATTLE SETTING
        public static int MaxWaveCount = 5;
        public static int BattleScalingConstant = 100;

        public static float DefaultMinSpawnDistance = 5f; // 몬스터 스폰 플레이어 최소거리
        public static float DefaultMaxSpawnDistance = 10f;

        public static float WaveTimer = 30f; // 웨이브 당 시간
        #endregion

        #region LAYERMASK SETTING
        public static LayerMask MonsterLayer = LayerMask.GetMask("Monster"); // 몬스터 레이어
        public static LayerMask PlayerLayer = LayerMask.GetMask("Player"); // 플레이어 레이어
        #endregion

        #region ANIMATION
        public static int AnimHashSpeed = Animator.StringToHash("Speed");
        #endregion
    }
}