using MS.Manager;
using MS.UI;
using System;
using UnityEngine;


namespace MS.Mode
{
    public class LobbyMode : GameModeBase
    {

        public enum LobbyModeState
        {
            MainMenu,
        }
        protected override void OnRegisterStates()
        {
            modeStateMachine.RegisterState((int)LobbyModeState.MainMenu, OnMainMenuEnter, OnMainMenuUpdate, OnMainMenuExit);
            modeStateMachine.TransitState((int)LobbyModeState.MainMenu);
        }

        public override void EndMode()
        {
            // todo :: bgm ²ô±â Á¤µµ?
        }


        #region MainMenu State
        private void OnMainMenuEnter(int arg1, object[] arg2)
        {
            UIManager.Instance.ShowView<BaseUI>("MainPanel");
        }
        private void OnMainMenuUpdate(float obj)
        {
            
        }
        private void OnMainMenuExit(int obj)
        {
            
        }
        #endregion
    }
}