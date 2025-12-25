using MS.Data;
using MS.Manager;
using System;
using UnityEngine;

public class PlayerAttributeSet : BaseAttributeSet
{
    public Stat CriticChance { get; protected set; }
    public Stat CriticMultiple { get; protected set; }
    public Stat Evasion { get; protected set; }
    public Stat LifeSteal { get; protected set; }
    public Stat CooltimeAccel { get; protected set; }
    public Stat ProjectileCount { get; protected set; }
    public Stat AreaRange { get; protected set; }
    public Stat Knockback { get; protected set; }
    public Stat CoinBonus { get; protected set; }

    public void InitAttributeSet(AttributeSetSettingData _characterData)
    {
        MaxHealth = new Stat(_characterData.MaxHealth);
        Health = _characterData.MaxHealth;
        AttackPower = new Stat(_characterData.AttackPower);
        Defense = new Stat(_characterData.Defense);
        MoveSpeed = new Stat(_characterData.MoveSpeed);
        CriticChance = new Stat(_characterData.CriticChance);
        CriticMultiple = new Stat(_characterData.CriticMultiple);
        Evasion = new Stat(_characterData.Evasion);
        LifeSteal = new Stat(_characterData.LifeSteal);
        CooltimeAccel = new Stat(_characterData.CooltimeAccel);
        ProjectileCount = new Stat(_characterData.ProjectileCount);
        AreaRange = new Stat(_characterData.AreaRange);
        Knockback = new Stat(_characterData.Knockback);
        CoinBonus = new Stat(_characterData.CoinBonus);

        statDict.Add(EStatType.MaxHealth, MaxHealth);
        statDict.Add(EStatType.AttackPower, AttackPower);
        statDict.Add(EStatType.Defense, Defense);
        statDict.Add(EStatType.MoveSpeed, MoveSpeed);
        statDict.Add(EStatType.CriticChance, CriticChance);
        statDict.Add(EStatType.CriticMultiple, CriticMultiple);
        statDict.Add(EStatType.Evasion, Evasion);
        statDict.Add(EStatType.LifeSteal, LifeSteal);
        statDict.Add(EStatType.CooltimeAccel, CooltimeAccel);
        statDict.Add(EStatType.ProjectileCount, ProjectileCount);
        statDict.Add(EStatType.AreaRange, AreaRange);
        statDict.Add(EStatType.Knockback, Knockback);
        statDict.Add(EStatType.CoinBonus, CoinBonus);
    }
}