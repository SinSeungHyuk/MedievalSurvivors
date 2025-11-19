using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEditor.Experimental.GraphView;
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


        private float attackRadius = 5.0f;
        private float forwardOffset = 3.0f;
        private void CheckHit()
        {
            Vector3 center = ownerSSC.Owner.Position + (ownerSSC.Owner.transform.forward * forwardOffset);
            Collider[] hitColliders = Physics.OverlapSphere(center, attackRadius, Settings.MonsterLayer);
            foreach (var hit in hitColliders)
            {
                if (hit.gameObject == ownerSSC.Owner.gameObject) continue;

                if (hit.gameObject.TryGetComponent(out SkillSystemComponent targetSSC))
                {
                    // 데미지 정보 생성
                    DamageInfo damageInfo = new DamageInfo(
                        _attacker: ownerSSC.Owner,
                        _target: targetSSC.Owner,
                        _attributeType: EDamageAttributeType.Fire,
                        _damage: 30f,
                        _isCritic: false,
                        _knockbackForce: 5f
                    );

                    targetSSC.TakeDamage(damageInfo);
                }
            }
        }
    }
}

