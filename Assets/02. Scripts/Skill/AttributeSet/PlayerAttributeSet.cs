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

        statDict.Add(EStatType.MaxHealth, MaxHealth);
        statDict.Add(EStatType.AttackPower, AttackPower);
        statDict.Add(EStatType.Defense, Defense);
        statDict.Add(EStatType.CriticChance, CriticChance);
        statDict.Add(EStatType.CriticMultiple, CriticMultiple);
        statDict.Add(EStatType.MoveSpeed, MoveSpeed);
    }
}
