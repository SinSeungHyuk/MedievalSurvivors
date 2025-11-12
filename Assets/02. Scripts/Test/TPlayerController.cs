using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Manager;
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

    public void Update()
    {
        if (SSC)
        {
            SSC.UseSkill("StoneSlash").Forget();
        }
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
        if (DataManager.Instance.CharacterSettingData.TryGetValue("TestCharacter", out CharacterSettingData _characterData))
        {
            PlayerAttributeSet playerAttributeSet = new PlayerAttributeSet();
            playerAttributeSet.InitAttributeSet(_characterData.AttributeSetSettingData);

            SSC.InitSkillActorInfo(this.gameObject, playerAttributeSet);
            SSC.GiveSkill(_characterData.DefaultSkillKey);

            Debug.Log("=================================================");
            Debug.Log($"[캐릭터 초기화 테스트] {_characterData.CharacterName}");

            // 1. AttributeSet 스탯 출력 (BaseAttributeSet 공통 속성)
            Debug.Log("--- 📊 스탯 정보 ---");
            Debug.Log($"MaxHealth: {SSC.AttributeSet.MaxHealth}, Health: {SSC.AttributeSet.Health}");
            Debug.Log($"AttackPower: {SSC.AttributeSet.AttackPower}, Defense: {SSC.AttributeSet.Defense}");
        }
    }

    public void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>().normalized;
    }
}
