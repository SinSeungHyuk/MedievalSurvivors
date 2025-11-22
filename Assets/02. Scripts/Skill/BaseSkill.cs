using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Skill;
using System.Threading;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


namespace MS.Skill
{
    public abstract class BaseSkill
    {
        protected SkillSystemComponent ownerSSC;
        protected FieldCharacter owner;
        protected SkillSettingData skillData;

        private float curCooltime;
        private int curSkillLevel;

        public bool IsCooltime => curCooltime > 0;
        public int CurSkillLevel => curSkillLevel;


        public virtual void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            ownerSSC = _owner;
            owner = ownerSSC.Owner;
            skillData = _skillData;
            curCooltime = 0;
            curSkillLevel = 1;
        }

        public abstract UniTask ActivateSkill(CancellationToken token);

        public void SetCooltime() => curCooltime = skillData.Cooltime;

        public void UpdateCooltime(float _deltaTime)
        {
            if (curCooltime > 0)
            {
                curCooltime -= _deltaTime;
            }
        }
    }
}