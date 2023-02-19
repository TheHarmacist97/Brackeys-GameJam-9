using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHealthUIOverlay : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    Character currentCharacter;

    public void UpdatePlayer()
    {
        currentCharacter = GameManager.Instance.player;
    }

    private void Awake()
    {
        GameManager.Instance.playerSet += UpdatePlayer;
        currentCharacter = GameManager.Instance.player;
        currentCharacter.OnDamage = UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthBar.fillAmount = (float)(currentCharacter.currentHealth / (float)currentCharacter.totalHealth);
    }


}
