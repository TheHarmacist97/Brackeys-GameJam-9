using System;
public interface IDamageable
{
    public int currentHealth { get; set; }
    public int totalHealth { get; set; }
    public void TakeDamage(int value);
    public void TakeHeal(int value);
    public void Die();
}
