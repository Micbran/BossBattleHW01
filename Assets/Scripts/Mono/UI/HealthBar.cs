using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health stats = null;

    [SerializeField] private Text healthBarValue = null;
    [SerializeField] private Image healthBarForeground = null;
    [SerializeField] private Image healthBarMiddleground = null;
    [SerializeField] private float secondaryHealthBarDrainRate = 0.0002f;

    private float maxHealth = 1;

    private void Awake()
    {
        maxHealth = stats.MaxHealth;
    }

    private void Start()
    {
        healthBarValue.text = Mathf.RoundToInt(stats.CurrentHealth).ToString();
    }

    private void OnEnable()
    {
        stats.OnTakeDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= UpdateHealthBar;
    }

    private void Update()
    {
        if (healthBarMiddleground.fillAmount <= healthBarForeground.fillAmount)
            return;
        healthBarMiddleground.fillAmount -= secondaryHealthBarDrainRate;

    }

    private void UpdateHealthBar(int currentHealth)
    {
        healthBarValue.text = Mathf.RoundToInt(Mathf.Max(currentHealth, 0)).ToString();
        healthBarForeground.fillAmount = (float)(Mathf.Max(currentHealth, 0)) / maxHealth;
    }
}
