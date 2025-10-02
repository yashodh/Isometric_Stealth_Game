using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health 
{
    private int _health;

    private int _maxHealth = 100;

    public bool IsDead => _health <= 0;

    public HealthContext Context;

    public Health()
    {
        _health = _maxHealth;
    }

    public void DealDamage(int dam, HealthContext context = HealthContext.None)
    {
        if (_health <= 0)
            return;

        Context = context;
        _health -= dam;
    }

    public int GetHealth()
    {
        return _health;
    }

    public void Heal(int val)
    {
        _health = val;
    }

    public void HealToMax()
    {
        Heal(_maxHealth);
    }
}

public enum HealthContext
{
    None,
    Melee,
    Ranged,
    FrontTakedown,
    BackTakedown,
    ExplosiveTakedown,
}
