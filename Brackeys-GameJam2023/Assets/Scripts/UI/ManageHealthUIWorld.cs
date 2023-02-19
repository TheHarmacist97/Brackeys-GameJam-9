using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManageHealthUIWorld : MonoBehaviour
{
    [SerializeField] private CanvasGroup cGroup;
    [SerializeField] private Image healthBar;
    private float rate = .75f;
    Character currentCharacter;
    Camera mainCamera;

    private void OnCharacterChange()
    {
        if (currentCharacter.Equals(GameManager.Instance.Player))
        {
            currentCharacter.OnDamage -= UpdateHealth;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        cGroup.alpha = 0f;
        mainCamera = Camera.main;
        currentCharacter = transform.parent.parent.GetComponent<Character>();
        currentCharacter.OnDamage = UpdateHealth;
        GameManager.Instance.playerSet += OnCharacterChange;
    }

    private void Update()
    {
        //transform.LookAt(mainCamera.transform.position);
    }

    private void UpdateHealth()
    {
        transform.LookAt(mainCamera.transform);
        healthBar.fillAmount = (float)((float)currentCharacter.currentHealth / (float)currentCharacter.totalHealth);
        cGroup.alpha = 1f;
        StartCoroutine(FadeHealth());
    }

    public IEnumerator FadeHealth()
    {
        yield return new WaitForSeconds(1f);
        while (cGroup.alpha>=0f)
        {
            yield return null;
            cGroup.alpha -= Time.deltaTime / rate;
        }
    }
}
