using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class BigCrystal : BaseSkill
    {
        private PlayerAttributeSet playerAttriSet;

        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);

            playerAttriSet = (_owner.AttributeSet) as PlayerAttributeSet;
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            await SetSkillCasting(token);

            var skillObject = SkillObjectManager.Instance.SpawnSkillObject<AreaObject>("Area_BigCrystal", owner, Settings.MonsterLayer);
            skillObject.InitArea();
            skillObject.transform.position = MonsterManager.Instance.GetNearestMonster(owner.Position).Position;
            skillObject.SetDuration(1.5f);
            skillObject.SetDelay(0.5f);
            skillObject.SetMaxHitCount(1);
            skillObject.SetHitCountPerAttack(1);
            skillObject.SetHitCallback((_skillObject, _ssc) =>
            {
                float damage = BattleUtils.CalcSkillBaseDamage(ownerSSC.AttributeSet.AttackPower.Value, skillData);
                bool isCritic = BattleUtils.CalcSkillCriticDamage(damage, playerAttriSet.CriticChance.Value, playerAttriSet.CriticMultiple.Value, out float finalDamage);

                DamageInfo damageInfo = new DamageInfo(
                        _attacker: owner,
                        _target: _ssc.Owner,
                        _attributeType: skillData.AttributeType,
                        _damage: finalDamage,
                        _isCritic: isCritic,
                        _knockbackForce: skillData.GetValue(ESkillValueType.Knockback)
                    );
                _ssc.TakeDamage(damageInfo);
            });

            await UniTask.CompletedTask;
        }
    }
}

