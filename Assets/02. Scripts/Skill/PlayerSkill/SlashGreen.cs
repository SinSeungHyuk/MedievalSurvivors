using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class SlashGreen : BaseSkill
    {
        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            float damage = ownerSSC.AttributeSet.AttackPower.Value * skillData.SkillValueDict[ESkillValueType.Damage] + skillData.SkillValueDict[ESkillValueType.Default];
            float speedDebuff = skillData.SkillValueDict[ESkillValueType.Buff];

            var skillObject = SkillObjectManager.Instance.SpawnSkillObject<AreaObject>("Area_Blizzard", owner, Settings.MonsterLayer);
            skillObject.InitArea(0.2f);
            skillObject.transform.position = MonsterManager.Instance.GetNearestMonster(owner.Position).Position;
            skillObject.SetDuration(4f);
            skillObject.SetMaxHitCount(17);
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

                _ssc.AttributeSet.MoveSpeed.AddBonusStat("Blizzard", EBonusType.Percentage, speedDebuff);
            });


            await UniTask.CompletedTask;
        }
    }
}

