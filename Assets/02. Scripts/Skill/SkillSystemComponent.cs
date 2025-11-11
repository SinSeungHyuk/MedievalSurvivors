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

            Debug.Log(ownedSkillDict["StoneSlash"] + " : " + ownedSkillDict["StoneSlash"].SkillLevel);
        }
    }
}