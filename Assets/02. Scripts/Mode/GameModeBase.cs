using MS.Core.StateMachine;
using UnityEngine;

namespace MS.Mode
{
    public abstract class GameModeBase
    {
        protected MSStateMachine<GameModeBase> modeStateMachine;


        public GameModeBase() {
            modeStateMachine = new MSStateMachine<GameModeBase>(this);
        }

        public virtual void StartMode()
        {
            OnRegisterStates();
        }

        public virtual void OnUpdate(float _deltaTime)
        {
            modeStateMachine.OnUpdate(_deltaTime);
        }

        public virtual void EndMode() { }

        protected abstract void OnRegisterStates();
    }
}