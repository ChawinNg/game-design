using System;
using UnityEngine;

public class AugmentStore : MonoBehaviour
{
    public static AugmentStore Instance;
    public event Action<string, float> OnStatChanged;

    private float _damageModifier = 0;
    private float _healthModifier = 0;
    private float _moveSpeedModifier = 0;
    private float _dashCooldownModifier = 0;
    private int _armorModifier = 0;
    private float _attackSpeedModifier = 0;

    public float DamageModifier
    {
        get => _damageModifier;
        set { _damageModifier = value; OnStatChanged?.Invoke(nameof(DamageModifier), value); }
    }

    public float HealthModifier
    {
        get => _healthModifier;
        set { _healthModifier = value; OnStatChanged?.Invoke(nameof(HealthModifier), value); }
    }

    public float MoveSpeedModifier
    {
        get => _moveSpeedModifier;
        set { _moveSpeedModifier = value; OnStatChanged?.Invoke(nameof(MoveSpeedModifier), value); }
    }

    public float DashCooldownModifier
    {
        get => _dashCooldownModifier;
        set { _dashCooldownModifier = value; OnStatChanged?.Invoke(nameof(DashCooldownModifier), value); }
    }

    public int ArmorModifier
    {
        get => _armorModifier;
        set { _armorModifier = value; OnStatChanged?.Invoke(nameof(ArmorModifier), value); }
    }

    public float AttackSpeedModifier
    {
        get => _attackSpeedModifier;
        set { _attackSpeedModifier = value; OnStatChanged?.Invoke(nameof(AttackSpeedModifier), value); }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
