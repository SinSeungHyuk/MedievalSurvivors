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
                Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
                Vector3 targetVelocity = moveDirection * player.SSC.AttributeSet.MoveSpeed.Value;
                rb.linearVelocity = targetVelocity;

                float moveValue = moveDirection.sqrMagnitude;
                if (moveValue > 0.01f)
                {
                    rb.MoveRotation(Quaternion.LookRotation(moveDirection, Vector3.up));
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

