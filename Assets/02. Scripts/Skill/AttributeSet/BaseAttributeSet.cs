using System;
using UnityEngine;

public class BaseAttributeSet
{
    // 현재체력, 최대체력
    public event Action<float, float> OnHealthChanged;

    private float health;

    public Stat MaxHealth { get; protected set; }
    public Stat AttackPower { get; protected set; } 
    public Stat Defense { get; protected set; }
    public Stat MoveSpeed { get; protected set; }


    public float Health
    {
        get => health;
        set
        {
            float clampedValue = Mathf.Clamp(value, 0, MaxHealth.Value);
            if (health != clampedValue)
            {
                health = clampedValue;
                OnHealthChanged?.Invoke(health, MaxHealth.Value);
            }
        }
    }
}
