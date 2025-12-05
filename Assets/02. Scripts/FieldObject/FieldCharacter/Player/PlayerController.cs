using MS.Skill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MS.Field
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerCharacter player;
        private CharacterController characterController;
        private Vector2 moveInput;
        private float moveSpeed;
        private float verticalVelocity;

        public Vector3 MoveDir { get; private set; }
        public CharacterController CC => characterController;


        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            player = GetComponent<PlayerCharacter>();
        }

        private void Update()
        {
            if (player.SSC.AttributeSet == null || player.IsMovementLocked)
            {
                moveInput = Vector2.zero;
                return;
            }

            CalculateMovement();
        }

        private void CalculateMovement()
        {
            if (!CC.isGrounded) verticalVelocity += Physics.gravity.y * Time.deltaTime;
            else verticalVelocity = 0f;

            MoveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            moveSpeed = player.SSC.AttributeSet.MoveSpeed.Value;
            
            Vector3 finalMove = (MoveDir * moveSpeed) + (Vector3.up * verticalVelocity);
            characterController.Move(finalMove * Time.deltaTime);

            float moveValue = MoveDir.sqrMagnitude;
            if (moveValue > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(MoveDir);
            }
            player.Animator.SetFloat("Speed", moveValue);
        }

        public void OnMove(InputValue inputValue)
        {
            moveInput = inputValue.Get<Vector2>().normalized;
        }
    }
}