using System;
using UnityEngine;

public class PHealth : PlayerManager
{
    [SerializeField] int maxHealth = 10;

    public int currentHealth { get; private set;  }

    public override void Init(PlayerSystems system)
    {
        base.Init(system);
        currentHealth = maxHealth;
    }

    public override void Tick()
    {
        Debug.Log(currentHealth);
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
    }
}
