using Cysharp.Threading.Tasks;
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
            MonsterManager.Instance.ClearMonster();
        }

        public override void OnUpdate(float _dt)
        {
            base.OnUpdate(_dt);

            SkillObjectManager.Instance.OnUpdate(_dt);
            MonsterManager.Instance.OnUpdate(_dt);
            EffectManager.Instance.OnUpdate(_dt);
        }


        #region Mode Callback
        private void OnMonsterDead()
        {
            if (MathUtils.IsSuccess(stageSettingData.WaveSpawnInfoList[curWaveCount - 1].FieldItemSpawnChance))
            {
                Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, curWaveCount);
                FieldItemManager.Instance.SpawnRandomFieldItem(spawnPos);
            }
            // TODO :: UI 업데이트 (킬카운트)
        }

        private void OnBossMonsterDead()
        {
            // 다음 웨이브로 넘어가는 연출 호출하기 + UI 업데이트
            ActivateNextWaveAsync().Forget();
        }
        #endregion
    }
}