using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode : GameModeBase
    {
        private StageSettingData stageSettingData;
        private PlayerCharacter player;


        public SurvivalMode(StageSettingData _stageSettingData) : base()
        {
            stageSettingData = _stageSettingData;
        }

        public enum SurvivalModeState
        {
            Load,
            BattleStart,
            BattleEnd,
        }

        protected override void OnRegisterStates()
        {
            modeStateMachine.RegisterState((int)SurvivalModeState.Load, OnLoadEnter, OnLoadUpdate, OnLoadExit);
            modeStateMachine.RegisterState((int)SurvivalModeState.BattleStart, OnBattleStartEnter, OnBattleStartUpdate, OnBattleStartExit);
            modeStateMachine.TransitState((int)SurvivalModeState.Load);
        }

        public override void EndMode() 
        { 
            SkillObjectManager.Instance.ClearSkillObject();
            EffectManager.Instance.ClearEffect();
            ObjectPoolManager.Instance.ClearAllPools();
        }


        private void OnMonsterDead()
        {
            if (MathUtils.IsSuccess(stageSettingData.WaveSpawnInfoList[curWaveCount - 1].FieldItemSpawnChance))
            {
                Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position);
                FieldItemManager.Instance.SpawnRandomFieldItem(spawnPos);
            }
        }
    }
}