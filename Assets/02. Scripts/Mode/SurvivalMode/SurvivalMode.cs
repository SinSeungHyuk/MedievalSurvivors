using MS.Data;
using MS.Manager;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode : GameModeBase
    {
        private StageSettingData stageSettingData;


        public SurvivalMode(StageSettingData _stageSettingData) : base()
        {
            stageSettingData = _stageSettingData;
        }

        public enum SurvivalModeState
        {
            LoadStart,
            BattleStart,
            BattleEnd,
        }

        protected override void OnRegisterStates()
        {
            modeStateMachine.RegisterState((int)SurvivalModeState.LoadStart, OnLoadEnter, OnLoadUpdate, OnLoadExit);
            modeStateMachine.TransitState((int)SurvivalModeState.LoadStart);
        }

        public override void EndMode() 
        { 
            SkillObjectManager.Instance.ClearSkillObject();
            EffectManager.Instance.ClearEffect();
            ObjectPoolManager.Instance.ClearAllPools();
        }
    }
}