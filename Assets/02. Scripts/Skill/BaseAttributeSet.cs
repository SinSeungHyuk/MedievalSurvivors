using UnityEngine;

public class BaseAttributeSet
{
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }

    public float AttackPower { get; protected set; } 
    public float Defense { get; protected set; }

    // TODO :: 상속받아서 각자 InitAttributeSet(Data)로 초기 세팅하기
}
