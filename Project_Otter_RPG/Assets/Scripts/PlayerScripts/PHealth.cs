using System;
using UnityEngine;

public class PHealth : PlayerManager
{
    [SerializeField] int maxHealth = 10;

    public int currentHealth { get; private set;  }

    public override void Init(PlayerSystems system)
    {
        base.Init(system);
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
    }
}
