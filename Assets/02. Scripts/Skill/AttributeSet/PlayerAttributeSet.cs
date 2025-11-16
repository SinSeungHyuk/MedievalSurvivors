using MS.Data;
using UnityEngine;

public class PlayerAttributeSet : BaseAttributeSet
{
    public float CriticChance { get; protected set; }
    public float CriticMultiple { get; protected set; }

    
    public void InitAttributeSet(AttributeSetSettingData _characterData)
    {
        Health = _characterData.Health;
        MaxHealth = _characterData.MaxHealth;
        AttackPower = _characterData.AttackPower;
        Defense = _characterData.Defense;
        CriticChance = _characterData.CriticChance;
        CriticMultiple = _characterData.CriticMultiple;
        MoveSpeed = _characterData.MoveSpeed;
    }
}
