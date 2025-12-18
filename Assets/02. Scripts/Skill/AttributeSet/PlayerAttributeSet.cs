using MS.Data;
using MS.Manager;
using System;
using UnityEngine;

public class PlayerAttributeSet : BaseAttributeSet
{
    public Stat CriticChance { get; protected set; }
    public Stat CriticMultiple { get; protected set; }

    
    public void InitAttributeSet(AttributeSetSettingData _characterData)
    {
        MaxHealth = new Stat(_characterData.MaxHealth);
        Health = _characterData.MaxHealth;
        AttackPower = new Stat(_characterData.AttackPower);
        Defense = new Stat(_characterData.Defense);
        CriticChance = new Stat(_characterData.CriticChance);
        CriticMultiple = new Stat(_characterData.CriticMultiple);
        MoveSpeed = new Stat(_characterData.MoveSpeed);
    }

    public Stat GetStatByType(EStatType _type)
    {
        return _type switch
        {
            EStatType.MaxHealth => MaxHealth,
            EStatType.AttackPower => AttackPower,
            EStatType.Defense => Defense,
            EStatType.MoveSpeed => MoveSpeed,
            EStatType.CriticChance => CriticChance,
            EStatType.CriticMultiple => CriticMultiple,
            _ => null
        };
    }
}
