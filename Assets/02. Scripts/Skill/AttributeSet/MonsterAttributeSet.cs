using MS.Data;
using UnityEngine;

public class MonsterAttributeSet : BaseAttributeSet
{
    public Stat DropEXP { get; protected set; }
    public Stat DropItem { get; protected set; }


    public void InitAttributeSet(MonsterAttributeSetSettingData _monsterData)
    {
        MaxHealth = new Stat(_monsterData.MaxHealth);
        Health = _monsterData.MaxHealth;
        AttackPower = new Stat(_monsterData.AttackPower);
        Defense = new Stat(_monsterData.Defense);
        DropEXP = new Stat(_monsterData.DropEXP);
        DropItem = new Stat(_monsterData.DropItem);
        MoveSpeed = new Stat(_monsterData.MoveSpeed);
    }
}
