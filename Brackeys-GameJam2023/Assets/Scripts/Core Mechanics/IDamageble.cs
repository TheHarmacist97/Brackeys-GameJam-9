using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public int currentHealth { get; set; }
    public int totalHealth { get; set; }
    public void TakeDamage(int value);
    public void TakeHeal(int value);
    public void Die();
}
