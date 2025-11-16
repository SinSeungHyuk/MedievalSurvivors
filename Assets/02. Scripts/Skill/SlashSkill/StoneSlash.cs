using Cysharp.Threading.Tasks;
using MS.Core;
using MS.Data;
using MS.Manager;
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

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            //ownerSSC.Owner.Animator.SetTrigger("Attack01");
            //ownerSSC.Owner.Animator.speed = 0.2f;

            ownerSSC.Owner.Animator.SetBool("Attack02Casting", true);

            GameplayCueManager.Instance.PlayCue("GC_Test", ownerSSC.Owner);

            await UniTask.Delay(3000);

            ownerSSC.Owner.Animator.SetBool("Attack02Casting", false);
        }
    }
}

