using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private PlayerShooter charge;
    [SerializeField] private float maxChargeTime = 7f;

    private float currentChargeTime = 0f;
    private bool isCharging = false;

    [SerializeField] private Text MaxText;
    [SerializeField] private Text ReadyText;

    [SerializeField] private Image ChargeBarForeground;

    private void OnEnable()
    {
        charge.OnChargeChange += this.OnChargeChange;
        charge.OnChargeEnd += this.OnChargeEnd;
    }

    private void OnDisable()
    {
        charge.OnChargeChange -= this.OnChargeChange;
        charge.OnChargeEnd -= this.OnChargeEnd;
    }

    private void OnChargeChange(float newCharge)
    {
        this.ChargeBarForeground.fillAmount = (newCharge / this.maxChargeTime);
        if (newCharge >= 1)
        {
            this.ReadyText.enabled = true;
        }
        if (newCharge >= 6.9)
        {
            this.MaxText.enabled = true;
        }
    }

    private void OnChargeEnd()
    {
        this.ChargeBarForeground.fillAmount = 0;
        this.ReadyText.enabled = false;
        this.MaxText.enabled = false;
    }
}
