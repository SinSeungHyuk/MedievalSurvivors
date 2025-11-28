using MS.Data;
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
    }
}