using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Skill;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public abstract class BaseSkill
    {
        protected SkillSystemComponent owner;
        protected SkillSettingData skillData;

        public float CurrentCooldown { get; protected set; }
        public int SkillLevel { get; protected set; }


        public void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            owner = _owner;
            skillData = _skillData;
            SkillLevel = 1;
        }

        public abstract UniTask ActivateSkill(CancellationToken token);

        public virtual bool CanActivateSkill()
        {
            

            return true;
        }
    }
}