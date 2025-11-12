using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Skill;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class StoneSlash : BaseSkill
    {
        private PlayerAttributeSet playerAttributeSet;


        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);

            playerAttributeSet = _owner.AttributeSet as PlayerAttributeSet;
        }

        public override UniTask ActivateSkill(CancellationToken token)
        {
            Debug.Log($"{skillData.skillName} »ç¿ë!!");

            return UniTask.CompletedTask;
        }
    }
}

