using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : IDamageable
{
    public event Action<int> OnTakeDamage = delegate { };

    public int CurrentHealth { get; private set; }

    [SerializeField] private int maxHealth;

    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;

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
    }

    public override void TakeDamage(int damage)
    {
        this.CurrentHealth -= damage;
        this.DamageFeedback();
        this.OnTakeDamage?.Invoke(this.CurrentHealth);
        this.CheckIfDead();
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
