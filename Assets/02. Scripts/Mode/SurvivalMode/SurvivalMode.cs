using UnityEngine;

namespace MS.Mode
{
    public class SurvivalMode : GameModeBase
    {
        public enum SurvivalModeState
        {
            LoadStart,
            BattleStart,
            BattleEnd,
        }

        protected override void OnRegisterStates()
        {
            modeStateMachine.RegisterState((int)SurvivalModeState.LoadStart, OnEnterPlay, OnUpdatePlay, OnExitPlay);

            modeStateMachine.TransitState((int)SurvivalModeState.LoadStart);
            Debug.Log("OnRegisterStates");
        }

        // TODO :: test
        private void OnEnterPlay(int _prev, object[] _params)
        {
            Debug.Log("Play State Enter");
        }

        private void OnUpdatePlay(float _dt)
        {
            
        }

        private void OnExitPlay(int _next)
        {
            
        }
    }
}