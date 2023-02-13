using UnityEngine;
[CreateAssetMenu(fileName = "BurstShot", menuName = "Weapons/Burst Fire", order = 2)]
public class BurstShotSO : WeaponSO
{
    public float minimumTimeBetweenBursts;
    public int burstsPerTriggerPull;
}
