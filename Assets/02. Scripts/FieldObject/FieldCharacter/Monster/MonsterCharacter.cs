using Cysharp.Threading.Tasks;
using MS.Core.StateMachine;
using MS.Data;
using MS.Manager;
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
        private Animator animator;

        public enum MonsterState
        {
            Trace,
            Attack,
            Dead,
        }


        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            monsterStateMachine = new MSStateMachine<MonsterCharacter>(this);
            monsterStateMachine.RegisterState((int)MonsterState.Trace, OnTraceEnter, OnTraceUpdate, OnTraceExit);
            monsterStateMachine.RegisterState((int)MonsterState.Attack, OnAttackEnter, OnAttackUpdate, OnAttackExit);
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
            monsterStateMachine.TransitState((int)MonsterState.Trace);
        }

        public void OnUpdate(float _deltaTime)
        {
            monsterStateMachine.OnUpdate(_deltaTime);
        }


        #region Trace
        private float attackRange = 0f;
        private void OnTraceEnter(int _prev, object[] _params)
        {
            navMeshAgent.destination = PlayerManager.Instance.Player.Position;
            attackRange = ((MonsterAttributeSet)(SSC.AttributeSet)).AttackRange.Value;
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
            navMeshAgent.isStopped = true;
        }
        #endregion

        #region Attack
        private MonsterSkillSettingData currentSkillData;
        private void OnAttackEnter(int _prev, object[] _params)
        {
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
            animator.SetTrigger(skillData.AnimTriggerKey); // 공격 애니메이션 재생
        }
        private void OnAttackUpdate(float _dt)
        {
            if (currentSkillData == null) return;

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(currentSkillData.AnimTriggerKey) && stateInfo.normalizedTime >= 1.0f)
            {
                monsterStateMachine.TransitState((int)MonsterState.Trace);
            }
        }
        private void OnAttackExit(int _next)
        {
            navMeshAgent.isStopped = false;
        }
        #endregion

        #region Dead
        #endregion
    }
}