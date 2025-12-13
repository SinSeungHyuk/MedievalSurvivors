using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class FastCrystal : BaseSkill
    {
        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            owner.Animator.SetTrigger(Settings.AnimHashAttack);

            float damage = ownerSSC.AttributeSet.AttackPower.Value * skillData.SkillValueDict[ESkillValueType.Damage] + skillData.SkillValueDict[ESkillValueType.Default];

            var skillObject = SkillObjectManager.Instance.SpawnSkillObject<AreaObject>("Area_FastCrystal", owner, Settings.MonsterLayer);
            skillObject.InitArea();
            skillObject.transform.position = MonsterManager.Instance.GetNearestMonster(owner.Position).Position;
            skillObject.SetDuration(1.5f);
            skillObject.SetDelay(0.5f);
            skillObject.SetMaxHitCount(1);
            skillObject.SetHitCountPerAttack(1);
            skillObject.SetHitCallback((_skillObject, _ssc) =>
            {
                DamageInfo damageInfo = new DamageInfo(
                        _attacker: owner,
                        _target: _ssc.Owner,
                        _attributeType: skillData.AttributeType,
                        _damage: damage,
                        _isCritic: false,
                        _knockbackForce: 0f
                    );
                _ssc.TakeDamage(damageInfo);
            });

            await UniTask.CompletedTask;
        }
    }
}

