using MS.Data;
using UnityEngine;

public class MonsterAttributeSet : BaseAttributeSet
{
    public Stat DropEXP { get; protected set; }
    public Stat AttackRange { get; protected set; }
    

    public void InitAttributeSet(MonsterAttributeSetSettingData _monsterData)
    {
        MaxHealth = new Stat(_monsterData.MaxHealth);
        Health = _monsterData.MaxHealth;
        AttackPower = new Stat(_monsterData.AttackPower);
        Defense = new Stat(_monsterData.Defense);
        MoveSpeed = new Stat(_monsterData.MoveSpeed);
        AttackRange = new Stat(_monsterData.AttackRange);
        DropEXP = new Stat(_monsterData.DropEXP);
    }
}
