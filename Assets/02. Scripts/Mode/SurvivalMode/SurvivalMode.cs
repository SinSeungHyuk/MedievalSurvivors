using Cysharp.Threading.Tasks;
using MS.Core;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.UI;
using MS.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode : GameModeBase
    {
        public event Action OnBossSpawned;

        private StageSettingData stageSettingData;
        private PlayerCharacter player;
        private BattlePanel battlePanel;

        public MSReactProp<int> KillCount { get; private set; } = new MSReactProp<int>(0);
        public MSReactProp<int> CurWaveCount { get; private set; } = new MSReactProp<int>(1);
        public MSReactProp<float> CurWaveTimer { get; private set; } = new MSReactProp<float>(1f);


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
            if (battlePanel != null)
            {
                battlePanel.OnUpdate(_dt);
            }
        }


        #region Mode Callback
        private void OnMonsterDead()
        {
            if (MathUtils.IsSuccess(stageSettingData.WaveSpawnInfoList[CurWaveCount.Value - 1].FieldItemSpawnChance))
            {
                Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, CurWaveCount.Value);
                FieldItemManager.Instance.SpawnRandomFieldItem(spawnPos);
            }

            KillCount.Value++;
        }

        private void OnBossMonsterDead()
        {
            // 다음 웨이브로 넘어가는 연출 호출하기 + UI 업데이트
            KillCount.Value++;
            ActivateNextWaveAsync().Forget();
        }

        private void OnPlayerLevelUpCallback(int _prevLv, int _curLv)
        {
            EffectManager.Instance.PlayEffect("Eff_Firework", player.Position, Quaternion.identity);


            List<StatRewardSettingData> selectedRewards = GetRandomStatRewards(4);

            var popup = UIManager.Instance.ShowPopup<StatRewardPopup>("StatRewardPopup");
            popup.InitStatRewardPopup(selectedRewards, player);
        }
        #endregion


        private List<StatRewardSettingData> GetRandomStatRewards(int _count)
        {
            List<StatRewardSettingData> results = new List<StatRewardSettingData>();
            var statRewardDict = DataManager.Instance.StatRewardSettingDataDict;
            var statTypes = (EStatType[])Enum.GetValues(typeof(EStatType));

            while (results.Count < _count)
            {
                EGrade rndGrade = MathUtils.GetRandomGrade();
                EStatType rndStat = statTypes[UnityEngine.Random.Range(0, statTypes.Length)];

                string key = rndStat.ToString() + rndGrade.ToString(); // 데이터 검색할 키값 조합

                if (statRewardDict.TryGetValue(key, out StatRewardSettingData data))
                {
                    if (!results.Contains(data))
                    {
                        results.Add(data);
                    }
                }
            }

            return results;
        }
    }
}