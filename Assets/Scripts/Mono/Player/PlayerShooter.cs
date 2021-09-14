using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet_SO bulletStats;
    [SerializeField] private Transform origin;
    [SerializeField] private ParticleSystem chargeParticles;
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private AudioClip chargeSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private ParticleSystem maxChargeParticles;
    [SerializeField] private AudioClip maxChargeSound;
    [SerializeField] private AudioClip badChargeSound;

    private bool isCharging = false;
    private float chargeTime = 0f;
    private bool fullCharge = false;
    private AudioSource chargeSource;

    private void Awake()
    {
        this.chargeParticles = Instantiate(this.chargeParticles, this.origin.transform);
        this.chargeParticles.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.StartCharging();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            this.ReleaseCharge();
        }

        if (this.isCharging)
        {
            this.KeepCharging();
        }
    }

    private void StartCharging()
    {
        this.isCharging = true;
        if (this.chargeParticles != null)
        {
            this.chargeParticles.Play();
        }
        if (this.chargeSound != null)
        {
            this.chargeSource = AudioHelper.PlayClipLoop(this.chargeSound, 0.3f);
        }
        return;
    }

    private void KeepCharging()
    {
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, 7f);
        if (chargeTime >= 6.9f && !this.fullCharge)
        {
            this.fullCharge = true;
            this.ChargeMaxFeedback();
        }
    }

    private void ReleaseCharge()
    {
        if (this.chargeParticles != null)
        {
            this.chargeParticles.Stop();
        }
        if (this.chargeSource != null)
        {
            Destroy(this.chargeSource.gameObject);
        }
        if (this.chargeTime > 1f)
        {
            Bullet bullet = Instantiate(bulletStats.bulletToFire, origin.position, Quaternion.identity);
            bullet.Fire(this.gameObject, this.origin.forward, this.CalculateBulletSpeed(bulletStats.bulletSpeed), this.CalculateBulletDamage(bulletStats.bulletDamage), bulletStats.collisionParticles, bulletStats.collisionSound);
            this.ReleaseFeedback();
        }
        else
        {
            this.BadChargeFeedback();
        }
        isCharging = false;
        this.fullCharge = false;
        this.chargeTime = 0f;
    }

    private float CalculateBulletSpeed(float originalSpeed)
    {
        if (this.chargeTime <= 1f)
        {
            return originalSpeed;
        }
        else if (this.chargeTime <= 3f)
        {
            return originalSpeed + 4;
        }
        else if (this.chargeTime <= 5f)
        {
            return originalSpeed + 8;
        }
        else if (this.chargeTime <= 6.8f)
        {
            return originalSpeed + 12;
        }
        else if (this.chargeTime <= 7f)
        {
            return originalSpeed + 20;
        }
        return originalSpeed;
    }

    private int CalculateBulletDamage(int originalDamage)
    {
        if (this.chargeTime <= 1f)
        {
            return originalDamage;
        }
        else if (this.chargeTime <= 3f)
        {
            return originalDamage + 2;
        }
        else if (this.chargeTime <= 5f)
        {
            return originalDamage + 4;
        }
        else if (this.chargeTime <= 6.8f)
        {
            return originalDamage + 6;
        }
        else if (this.chargeTime <= 7f)
        {
            return originalDamage + 10;
        }
        return originalDamage;
    }

    private void ReleaseFeedback()
    {
        if (this.fireParticles != null)
        {
            Instantiate(this.fireParticles, this.origin.position, Quaternion.identity);
        }
        if (this.fireSound != null)
        {
            AudioHelper.PlayClip2D(this.fireSound, 1f);
        }
        if (this.chargeParticles != null)
        {
            this.chargeParticles.Stop();
        }
    }

    private void BadChargeFeedback()
    {
        if (this.badChargeSound != null)
        {
            AudioHelper.PlayClip2D(this.badChargeSound, 1f);
        }
    }

    private void ChargeMaxFeedback()
    {
        if (this.maxChargeParticles != null)
        {
            Instantiate(this.maxChargeParticles, this.origin.position, Quaternion.identity);
        }
        if (this.maxChargeSound != null)
        {
            AudioHelper.PlayClip2D(this.maxChargeSound, 0.5f);
        }
        if (this.chargeParticles != null)
        {
            this.chargeParticles.Stop();
        }
        Destroy(this.chargeSource);
    }
}
