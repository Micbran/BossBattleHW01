using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : IDamageable
{
    public event Action<int> OnTakeDamage = delegate { };

    public int CurrentHealth { get; private set; }

    private float invincibilityTimer = -1;
    private float invincibilityTimerMax = 2f;

    [SerializeField] private Color invincibleColor;

    [SerializeField] private int maxHealth;

    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;

    private Player ifPlayer = null;

    public int MaxHealth
    {
        get
        {
            return this.maxHealth;
        }
    }

    private void Awake()
    {
        this.CurrentHealth = this.maxHealth;
        this.ifPlayer = this.GetComponent<Player>();
    }

    private void Update()
    {
        if (this.ifPlayer != null && this.ifPlayer.Invincible)
        {
            this.invincibilityTimer -= Time.deltaTime;
            if (this.invincibilityTimer < 0)
            {
                ifPlayer.ResetColor();
                this.ifPlayer.Invincible = false;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (this.ifPlayer != null && this.ifPlayer.Invincible)
        {
            return;
        }
        this.CurrentHealth -= damage;
        this.DamageFeedback();
        this.OnTakeDamage?.Invoke(this.CurrentHealth);
        this.CheckIfDead();
        if (this.ifPlayer != null)
        {
            this.ifPlayer.Invincible = true;
            this.ifPlayer.SetColor(this.invincibleColor);
            this.invincibilityTimer = this.invincibilityTimerMax;
        } 
    }

    private void DamageFeedback()
    {
        if (this.damageParticles != null)
        {
            Instantiate(this.damageParticles, this.transform.position, Quaternion.identity);
        }
        if (this.damageSound != null)
        {
            AudioHelper.PlayClip2D(this.damageSound, 0.8f);
        }
    }

    private void CheckIfDead()
    {
        if (CurrentHealth <= 0)
        {
            this.Kill();
        }
    }

    private void Kill()
    {
        IDestroy[] destroys = this.GetComponents<IDestroy>();
        foreach (IDestroy destroy in destroys)
        {
            destroy.Destruction();
        }
    }
}
