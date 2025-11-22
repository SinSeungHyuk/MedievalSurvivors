using MS.Skill;
using UnityEngine;

namespace MS.Field
{
    public class ProjectileObject : SkillObject
    {
        private Vector3 moveDir;
        private float moveSpeed;


        public void InitProjectile(Vector3 _moveDir, float _moveSpeed)
        {
            GetComponent<Rigidbody>().linearVelocity = _moveDir * moveSpeed;

            moveDir = _moveDir;
            moveSpeed = _moveSpeed;
        }

        public void OnTriggerEnter(Collider _other)
        {
            if (IsValidTarget(_other, out SkillSystemComponent _ssc))
            {
                for (int i = 0; i < hitCountPerAttack; i++)
                {
                    onHitCallback?.Invoke(this, _ssc);
                }
                maxAttackCount--;
            }

            if (maxAttackCount <= 0)
                ObjectLifeState = FieldObjectLifeState.Death;
        }

        public override void OnUpdate(float _deltaTime)
        {
            base.OnUpdate(_deltaTime);

            //transform.position += (moveDir * moveSpeed * _deltaTime);
        }
    }
}