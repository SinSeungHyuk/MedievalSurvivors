using MS.Core.StateMachine;
using MS.Data;
using MS.Manager;
using UnityEngine;
using UnityEngine.AI;
using static MS.Mode.SurvivalMode;

namespace MS.Field
{
    public class MonsterCharacter : FieldCharacter
    {
        private MSStateMachine<MonsterCharacter> monsterStateMachine;
        private NavMeshAgent navMeshAgent;

        public enum MonsterState
        {
            Trace,
            Attack,
            Dead,
        }

        protected override void Awake()
        {
            base.Awake();

            navMeshAgent = GetComponent<NavMeshAgent>();
            monsterStateMachine = new MSStateMachine<MonsterCharacter>(this);
            monsterStateMachine.RegisterState((int)MonsterState.Trace, OnTraceEnter, OnTraceUpdate, OnTraceExit);
        }

        public void InitMonster(string _monsterKey)
        {
            ObjectType = FieldObjectType.Monster;
            ObjectLifeState = FieldObjectLifeState.Live;

            if (!DataManager.Instance.MonsterSettingDataDict.TryGetValue(_monsterKey, out MonsterSettingData _monsterData))
            {
                Debug.LogError($"InitMonster Key Missing : {_monsterKey}");
                return;
            }

            MonsterAttributeSet monsterAttributeSet = new MonsterAttributeSet();
            monsterAttributeSet.InitAttributeSet(_monsterData.AttributeSetSettingData);
            SSC.InitSkillActorInfo(this, monsterAttributeSet);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(Position, out hit, 5.0f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
                navMeshAgent.enabled = true;
            }
            monsterStateMachine.TransitState((int)MonsterState.Trace);
        }

        public void OnUpdate(float _deltaTime)
        {
            monsterStateMachine.OnUpdate(_deltaTime);
        }


        #region Trace
        private void OnTraceEnter(int _prev, object[] _params)
        {
            navMeshAgent.destination = PlayerManager.Instance.Player.Position;
        }
        private void OnTraceUpdate(float _dt)
        {
            navMeshAgent.destination = PlayerManager.Instance.Player.Position;
        }
        private void OnTraceExit(int _next)
        {
            
        }
        #endregion

        #region Attack
        #endregion

        #region Dead
        #endregion
    }
}