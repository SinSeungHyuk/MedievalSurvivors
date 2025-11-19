using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class SkillSystemComponent : MonoBehaviour
    {
        private Dictionary<string, BaseSkill> ownedSkillDict = new Dictionary<string, BaseSkill>();
        private Dictionary<string, CancellationTokenSource> runningSkillDict = new Dictionary<string, CancellationTokenSource>();

        private BaseAttributeSet attributeSet;
        private FieldCharacter owner;

        public BaseAttributeSet AttributeSet => attributeSet;
        public FieldCharacter Owner => owner;


        // TODO :: GIZMO
        [Header("Gizmo Test Settings")]
        [SerializeField] private float testAttackRadius = 5.0f;
        [SerializeField] private float testForwardOffset = 3.0f;
        [SerializeField] private bool showGizmos = true;
        private void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;

            Gizmos.color = Color.red;

            Vector3 center = transform.position + (transform.forward * testForwardOffset);
            Gizmos.DrawWireSphere(center, testAttackRadius);
        }


        void Update()
        {
            foreach (var skill in ownedSkillDict.Values)
            {
                skill.UpdateCooltime(Time.deltaTime);
            }
        }

        public void InitSkillActorInfo(FieldCharacter _owner, BaseAttributeSet _attributeSet)
        {
            owner = _owner;
            attributeSet = _attributeSet;
        }

        public void GiveSkill(string _skillKey)
        {
            if (DataManager.Instance.SkillSettingDataDict.TryGetValue(_skillKey, out SkillSettingData _skillData))
            {
                // namespace 규칙이 반드시 보장되어야 합니다.
                var skillType = Type.GetType("MS.Skill." +  _skillKey);
                try
                {
                    BaseSkill skillInstance = (BaseSkill)Activator.CreateInstance(skillType);
                    skillInstance.InitSkill(this, _skillData);
                    ownedSkillDict.Add(_skillKey, skillInstance);
                }
                catch (Exception ex)
                {
                    Debug.LogError("SkillSystemComponent::GiveSkill : " + ex.Message);
                }
            }
        }

        public async UniTask UseSkill(string _skillKey)
        {
            if (!ownedSkillDict.TryGetValue(_skillKey, out BaseSkill skillToUse))
            {
                return;
            }

            if (skillToUse.IsCooltime) return;

            CancelSkill(_skillKey);

            CancellationTokenSource cts = new CancellationTokenSource();
            runningSkillDict[_skillKey] = cts;

            try
            {
                skillToUse.SetCooltime();
                await skillToUse.ActivateSkill(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"{_skillKey} 스킬 캔슬");
            }
            catch (Exception e)
            {
                Debug.LogError($"{_skillKey} 스킬 실행 중 에러: {e.Message}");
            }
            finally
            {
                // 스킬이 완료되었든, 캔슬되었든, 오류가 났든 실행 목록에서 제거
                if (runningSkillDict.ContainsKey(_skillKey))
                {
                    runningSkillDict.Remove(_skillKey);
                    cts.Dispose();
                }
            }
        }

        public void TakeDamage(DamageInfo _damageInfo)
        {
            if (attributeSet.Health <= 0) return;

            float finalDamage = _damageInfo.Damage;

            // TODO :: 방어력, 약점속성 계산으로 최종 데미지 결정
            finalDamage = BattleUtils.CalcDefenseStat(finalDamage, attributeSet.Defense.Value);
            attributeSet.Health -= finalDamage;

            // TODO :: 넉백,데미지 텍스트
            //ShowDamageText(finalDamage, damageInfo.IsCritical, damageInfo.AttributeType);
            //ApplyKnockback(damageInfo);

            Debug.Log($"[피격] {owner.name}가 {finalDamage}의 피해를 입음 (남은 체력: {attributeSet.Health})");
            if (attributeSet.Health <= 0)
            {
                //Die();
            }
        }

        public void CancelSkill(string _skillKey)
        {
            if (runningSkillDict.TryGetValue(_skillKey, out CancellationTokenSource cts))
            {
                cts.Cancel();
                cts.Dispose();
                runningSkillDict.Remove(_skillKey);
            }
        }

        public void CancelAllSkills()
        {
            foreach (var cts in runningSkillDict.Values)
            {
                cts.Cancel();
                cts.Dispose();
            }
            runningSkillDict.Clear();
        }
    }
}