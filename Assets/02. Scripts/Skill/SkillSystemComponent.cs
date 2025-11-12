using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Manager;
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
        private GameObject owner;

        public BaseAttributeSet AttributeSet => attributeSet;
        public GameObject Owner => owner;


        void Update()
        {
            foreach (var skill in ownedSkillDict.Values)
            {
                skill.UpdateCooltime(Time.deltaTime);
            }
        }

        public void InitSkillActorInfo(GameObject _owner, BaseAttributeSet _attributeSet)
        {
            owner = _owner;
            attributeSet = _attributeSet;
        }

        public void GiveSkill(string _skillKey)
        {
            if (DataManager.Instance.SkillSettingData.TryGetValue(_skillKey, out SkillSettingData _skillData))
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
                Debug.LogWarning($"ownedSkillDict에 {_skillKey}가 없습니다.");
                return;
            }

            if (skillToUse.IsCooltime) return;

            CancelSkill(_skillKey);

            CancellationTokenSource cts = new CancellationTokenSource();
            runningSkillDict[_skillKey] = cts;

            Debug.Log($"{_skillKey} 스킬 사용 시작...");

            try
            {
                await skillToUse.ActivateSkill(cts.Token);

                skillToUse.SetCooltime();
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