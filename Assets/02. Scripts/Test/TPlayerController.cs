using MS.Skill;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86;


public class TPlayerController : MonoBehaviour
{
    public SkillSystemComponent SSC;

    public Vector2 MoveInput;
    public Vector2 MoveDir;
    public float MoveSpeed = 5;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SSC = gameObject.AddComponent<SkillSystemComponent>();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 moveDirection = new Vector3(MoveInput.x, 0f, MoveInput.y);
            Vector3 targetVelocity = moveDirection * MoveSpeed;
            rb.linearVelocity = targetVelocity;

            if (moveDirection.sqrMagnitude > 0.0001f)
            {
                rb.MoveRotation(Quaternion.LookRotation(moveDirection, Vector3.up));
                MoveDir = MoveInput; // MoveInput 자체가 이미 정규화된 2D 방향임
            }
        }
    }

    public void Test()
    {
        SSC.InitSkillActorInfo(this.gameObject, null);
        SSC.GiveSkill("StoneSlash");
    }

    public void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>().normalized;
    }
}
