using MS.Core.StateMachine;
using MS.Field;
using MS.Manager;
using UnityEngine;

namespace MS.Mode
{
    public abstract class GameModeBase
    {
        protected MSStateMachine<GameModeBase> modeStateMachine;
        protected FieldMap curFieldMap;

        public FieldMap CurFieldMap => curFieldMap;



        public GameModeBase() 
        {
            modeStateMachine = new MSStateMachine<GameModeBase>(this);
        }

        public virtual void StartMode()
        {
            OnRegisterStates();
        }

        public virtual void OnUpdate(float _deltaTime)
        {
            modeStateMachine.OnUpdate(_deltaTime);

            SkillObjectManager.Instance.OnUpdate(Time.deltaTime);
            MonsterManager.Instance.OnUpdate(Time.deltaTime);
        }

        public virtual void OnFixedUpdate(float _fixedDeltaTime)
        {
            SkillObjectManager.Instance.OnFixedUpdate(Time.fixedDeltaTime);
        }

        public virtual void EndMode() { }

        protected abstract void OnRegisterStates();
    }
}