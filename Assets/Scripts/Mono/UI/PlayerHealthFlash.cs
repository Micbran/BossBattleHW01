using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthFlash : MonoBehaviour
{
    [SerializeField] private GameObject HealthFeedbackImage;
    [SerializeField] private Health stats;

    private void OnEnable()
    {
        stats.OnTakeDamage += FlashDamage;
    }

    private void OnDisable()
    {
        stats.OnTakeDamage -= FlashDamage;
    }

    private void FlashDamage(int damage)
    {
        this.HealthFeedbackImage.SetActive(true);
    }
}
