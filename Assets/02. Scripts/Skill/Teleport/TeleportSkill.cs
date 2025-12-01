using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace MS.Skill
{
    public class Teleport : BaseSkill
    {
        private PlayerController playerController;
        private Rigidbody rb;


        public override void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            base.InitSkill(_owner, _skillData);

            playerController = owner.GetComponent<PlayerCharacter>().PlayerController;
            rb = playerController.Rb;
        }

        public override async UniTask ActivateSkill(CancellationToken token)
        {
            Vector3 moveDir = playerController.MoveDir;
            if (moveDir == Vector3.zero) moveDir = owner.transform.forward;

            float teleportRange = 15f;
            Vector3 currentPos = rb.position;
            Vector3 targetPos = currentPos + (moveDir.normalized * teleportRange);

            NavMeshHit hit;
            if (NavMesh.Raycast(currentPos, targetPos, out hit, NavMesh.AllAreas))
            {
                targetPos = hit.position;
            }

            rb.MovePosition(targetPos);

            EffectManager.Instance.PlayEffect("Eff_Teleport", owner.Position, owner.Rotation);

            await UniTask.CompletedTask;
        }
    }
}