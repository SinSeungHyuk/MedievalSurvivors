using MS.Data;
using MS.Skill;
using UnityEngine;

public class MonsterAttributeSet : BaseAttributeSet
{
    public EDamageAttributeType WeaknessAttributeType { get; protected set; }
    public Stat AttackRange { get; protected set; }
    

    public void InitAttributeSet(MonsterAttributeSetSettingData _monsterData)
    {
        MaxHealth = new Stat(_monsterData.MaxHealth);
        Health = _monsterData.MaxHealth;
        AttackPower = new Stat(_monsterData.AttackPower);
        Defense = new Stat(_monsterData.Defense);
        MoveSpeed = new Stat(_monsterData.MoveSpeed);
        AttackRange = new Stat(_monsterData.AttackRange);
        WeaknessAttributeType = _monsterData.WeaknessAttributeType;
    }
}
