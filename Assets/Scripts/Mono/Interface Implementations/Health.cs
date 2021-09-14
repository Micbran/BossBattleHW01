using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : IDamageable
{
    private int currentHealth;
    [SerializeField] private int maxHealth;

    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;

    private void Awake()
    {
        this.currentHealth = this.maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        this.currentHealth -= damage;
        this.DamageFeedback();
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
        if (currentHealth <= 0)
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
