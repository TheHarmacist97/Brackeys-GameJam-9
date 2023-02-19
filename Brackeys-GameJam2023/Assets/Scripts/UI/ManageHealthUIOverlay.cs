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
        currentCharacter.OnDamage -= UpdateHealth;
        currentCharacter = GameManager.Instance.player;
        currentCharacter.OnDamage += UpdateHealth;
        UpdateHealth();
    }

    private void Awake()
    {
        GameManager.Instance.playerSet += UpdatePlayer;
        currentCharacter = GameManager.Instance.Player;
        currentCharacter.OnDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        Debug.Log(currentCharacter.currentHealth / (float)currentCharacter.totalHealth);
        healthBar.fillAmount = currentCharacter.currentHealth / (float)currentCharacter.totalHealth;
    }


}
