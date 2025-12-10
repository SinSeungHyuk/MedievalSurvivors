using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public class FOBS : BaseSkill
    {
        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            var skillObject = SkillObjectManager.Instance.SpawnSkillObject<AreaObject>("Area_FOBS", owner, Settings.MonsterLayer);
            skillObject.InitArea(0.2f);
            skillObject.transform.position = MonsterManager.Instance.GetNearestMonster(owner.Position).Position;
            skillObject.SetDuration(4f);
            skillObject.SetMaxHitCount(int.MaxValue);
            skillObject.SetHitCountPerAttack(1);
            skillObject.SetHitCallback((_skillObject, _ssc) =>
            {
                DamageInfo damageInfo = new DamageInfo(
                        _attacker: owner,
                        _target: _ssc.Owner,
                        _attributeType: EDamageAttributeType.Electric,
                        _damage: 50f,
                        _isCritic: false,
                        _knockbackForce: 0f
                    );
                _ssc.TakeDamage(damageInfo);
            });


            await UniTask.CompletedTask;
        }
    }
}

