using MS.Skill;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86;


namespace MS.Field
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerCharacter player;
        private Rigidbody rb;
        private Vector2 moveInput;

        public Vector3 MoveDir { get; private set; }
        public Rigidbody Rb => rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            player = GetComponent<PlayerCharacter>();
        }

        private void FixedUpdate()
        {
            if (player.SSC.AttributeSet == null || player.IsMovementLocked)
            {
                rb.linearVelocity = Vector2.zero;
                moveInput = Vector2.zero;
                return;
            }
            if (rb != null)
            {
                MoveDir = new Vector3(moveInput.x, 0f, moveInput.y);
                Vector3 targetVelocity = MoveDir * player.SSC.AttributeSet.MoveSpeed.Value;
                rb.linearVelocity = targetVelocity;

                float moveValue = MoveDir.sqrMagnitude;
                if (moveValue > 0.01f)
                {
                    rb.MoveRotation(Quaternion.LookRotation(MoveDir, Vector3.up));
                }
                player.Animator.SetFloat("Speed", moveValue);
            }
        }

        public void OnMove(InputValue inputValue)
        {
            moveInput = inputValue.Get<Vector2>().normalized;
        }
    }
}

