using Cysharp.Threading.Tasks;
using MS.Core.StateMachine;
using MS.Data;
using MS.Manager;
using MS.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


namespace MS.Field
{
    public class MonsterCharacter : FieldCharacter
    {
        private MSStateMachine<MonsterCharacter> monsterStateMachine;
        private List<MonsterSkillSettingData> skillList = new List<MonsterSkillSettingData>();
        private NavMeshAgent navMeshAgent;

        public enum MonsterState
        {
            Idle,
            Trace,
            Attack,
            Dead,
        }


        protected override void Awake()
        {
            base.Awake();

            navMeshAgent = GetComponent<NavMeshAgent>();
            monsterStateMachine = new MSStateMachine<MonsterCharacter>(this);
            monsterStateMachine.RegisterState((int)MonsterState.Idle, OnIdleEnter, OnIdleUpdate, OnIdleExit);
            monsterStateMachine.RegisterState((int)MonsterState.Trace, OnTraceEnter, OnTraceUpdate, OnTraceExit);
            monsterStateMachine.RegisterState((int)MonsterState.Attack, OnAttackEnter, OnAttackUpdate, OnAttackExit);
            monsterStateMachine.RegisterState((int)MonsterState.Dead, OnDeadEnter, OnDeadUpdate, OnDeadExit);
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

            foreach (var skillInfo in _monsterData.SkillList)
            {
                SSC.GiveSkill(skillInfo.SkillKey);
            }
            skillList = _monsterData.SkillList;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(Position, out hit, 5.0f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
                navMeshAgent.enabled = true;
            }
            navMeshAgent.stoppingDistance = _monsterData.AttributeSetSettingData.AttackRange - 1f;
            navMeshAgent.speed = _monsterData.AttributeSetSettingData.MoveSpeed;
            monsterStateMachine.TransitState((int)MonsterState.Idle);
        }

        public void OnUpdate(float _deltaTime)
        {
            monsterStateMachine.OnUpdate(_deltaTime);
        }


        #region Idle
        private float elapsedIdleTime = 0f;
        private void OnIdleEnter(int _prev, object[] _params)
        {
            navMeshAgent.ResetPath();
            Animator.SetTrigger(Settings.AnimHashIdle);
            elapsedIdleTime = 0f;
        }
        private void OnIdleUpdate(float _dt)
        {
            elapsedIdleTime += _dt;
            if (elapsedIdleTime > 0.2f)
            {
                monsterStateMachine.TransitState((int)MonsterState.Trace);
            }
        }
        private void OnIdleExit(int _next)
        {
            
        }
        #endregion

        #region Trace
        private float attackRange = 0f;
        private void OnTraceEnter(int _prev, object[] _params)
        {
            navMeshAgent.isStopped = false;

            attackRange = ((MonsterAttributeSet)(SSC.AttributeSet)).AttackRange.Value;
            if ((PlayerManager.Instance.Player.Position - Position).sqrMagnitude < (attackRange * attackRange))
            {
                monsterStateMachine.TransitState((int)MonsterState.Attack);
            }
            else
            {
                navMeshAgent.destination = PlayerManager.Instance.Player.Position;
                Animator.SetTrigger(Settings.AnimHashRun);
            }
        }
        private void OnTraceUpdate(float _dt)
        {
            navMeshAgent.destination = PlayerManager.Instance.Player.Position;
            
            if ((PlayerManager.Instance.Player.Position - Position).sqrMagnitude < (attackRange* attackRange))
            {
                monsterStateMachine.TransitState((int)MonsterState.Attack);
            }
        }
        private void OnTraceExit(int _next)
        {
            
        }
        #endregion

        #region Attack
        private MonsterSkillSettingData currentSkillData;
        private void OnAttackEnter(int _prev, object[] _params)
        {
            navMeshAgent.isStopped = true;

            // 소유한 스킬리스트에서 랜덤으로 사용할 스킬 선택
            MonsterSkillSettingData skillData = null;
            int totalRatio = skillList.Sum(x => x.SkillActivateRate);
            int ratioSum = 0;
            int randomRate = Random.Range(0, totalRatio);
            foreach (var skillInfo in skillList)
            {
                ratioSum += skillInfo.SkillActivateRate;
                if (randomRate < ratioSum)
                {
                    skillData = skillInfo;
                    break;
                }
            }
            currentSkillData = skillData;

            SSC.UseSkill(skillData.SkillKey).Forget(); // 해당 공격이 쿨타임이면 내부적으로 사용을 안함
            Animator.SetTrigger(skillData.AnimTriggerKey); // 공격 애니메이션 재생
        }
        private void OnAttackUpdate(float _dt)
        {
            if (currentSkillData == null) return;

            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(currentSkillData.AnimTriggerKey) && stateInfo.normalizedTime >= 0.95f)
            {
                monsterStateMachine.TransitState((int)MonsterState.Idle);
            }
        }
        private void OnAttackExit(int _next)
        {
           
        }
        #endregion

        #region Dead
        private void OnDeadEnter(int _prev, object[] _params)
        {
            navMeshAgent.ResetPath();
            Animator.SetTrigger(Settings.AnimHashDead);
        }
        private void OnDeadUpdate(float _dt)
        {
            elapsedIdleTime += _dt;
            if (elapsedIdleTime > 0.2f)
            {
                
            }
        }
        private void OnDeadExit(int _next)
        {

        }
        #endregion
    }
}