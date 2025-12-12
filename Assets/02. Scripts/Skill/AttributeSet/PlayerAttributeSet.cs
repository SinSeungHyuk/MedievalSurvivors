using MS.Data;
using MS.Manager;
using System;
using UnityEngine;

public class PlayerAttributeSet : BaseAttributeSet
{
    public event Action<float, float> OnExpChanged;
    public event Action OnLevelUp;

    private float curExp;
    private float maxExp;
    private int curLevel;

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

        curLevel = 1;
        curExp = 0;
        maxExp = DataManager.Instance.CharacterSettingData.LevelSettingData.BaseExp;
    }

    public float CurExp
    {
        get => curExp;
        set
        {
            curExp += value;
            while (curExp >= maxExp)
            {
                curExp -= maxExp;
                LevelUp();
            }
            OnExpChanged?.Invoke(curExp, maxExp);
        }
    }

    private void LevelUp()
    {
        float increaseValue = DataManager.Instance.CharacterSettingData.LevelSettingData.IncreaseExpPerLevel;
        curLevel++;
        maxExp += (maxExp * (increaseValue * 0.01f));

        OnLevelUp?.Invoke();
    }
}
