using MS.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Field
{
    public class AreaObject : SkillObject
    {
        private List<SkillSystemComponent> attackTargetList = new List<SkillSystemComponent>();
        private float attackInterval;
        private float elapsedAttackTime;


        public void InitArea(float _attackInterval)
        {
            attackInterval = _attackInterval;
            elapsedAttackTime = 0f;
        }

        public void OnTriggerEnter(Collider _other)
        {
            if (IsValidTarget(_other, out SkillSystemComponent _ssc))
            {
                attackTargetList.Add(_ssc);
            }
        }
        public void OnTriggerExit(Collider _other)
        {
            if (_other.TryGetComponent(out SkillSystemComponent _ssc))
                attackTargetList.Remove(_ssc);
        }

        public override void OnUpdate(float _deltaTime)
        {
            base.OnUpdate(_deltaTime);

            elapsedAttackTime += _deltaTime;
            if (elapsedAttackTime < attackInterval)
                return;

            for (int i = attackTargetList.Count - 1; i >= 0; i--)
            {
                var attackTarget = attackTargetList[i];
                if (!attackTarget || attackTarget.Owner.ObjectLifeState != FieldObjectLifeState.Live)
                {
                    attackTargetList.RemoveAt(i);
                    continue;
                }

                for (int hitCount = 0; hitCount < hitCountPerAttack; hitCount++)
                {
                    onHitCallback?.Invoke(this, attackTarget);
                }
            }
            elapsedAttackTime = 0f;
            
            maxAttackCount--;
            if (maxAttackCount <= 0)
                ObjectLifeState = FieldObjectLifeState.Death;
        }
    }
}