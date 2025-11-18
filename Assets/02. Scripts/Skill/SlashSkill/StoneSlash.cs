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


            await UniTask.Delay(3000);

            GameplayCueManager.Instance.PlayCue("GC_Test", ownerSSC.Owner);
            ownerSSC.Owner.Animator.SetBool("Attack02Casting", false);

            CheckHit();
        }


        private float attackRadius = 2.0f;
        private float forwardOffset = 2.0f;
        private void CheckHit()
        {
            Vector3 center = ownerSSC.Owner.transform.position + (ownerSSC.Owner.transform.forward * forwardOffset);
            Collider[] hitColliders = Physics.OverlapSphere(center, attackRadius);

            foreach (var hit in hitColliders)
            {
                if (hit.gameObject == ownerSSC.Owner.gameObject) continue;
                Debug.Log($"[StoneSlash] Hit : {hit.name}");
            }
        }
    }
}

