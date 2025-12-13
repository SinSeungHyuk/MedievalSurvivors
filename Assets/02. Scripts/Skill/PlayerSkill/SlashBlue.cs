using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class SlashBlue : BaseSkill
    {
        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            float damage = ownerSSC.AttributeSet.AttackPower.Value * skillData.SkillValueDict[ESkillValueType.Damage] + skillData.SkillValueDict[ESkillValueType.Default];

            var skillObject = SkillObjectManager.Instance.SpawnSkillObject<AreaObject>("Area_SlashBlue", owner, Settings.MonsterLayer);
            skillObject.InitArea();
            skillObject.SetDuration(2.5f);
            skillObject.SetDelay(1f);
            skillObject.SetTraceTarget(owner, new Vector3(0f,0.5f,0f));
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

