using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossStates;

[RequireComponent(typeof(BossMovement))]
public class BossShooter : MonoBehaviour
{
    private BossMovement parentMovement;
    private Transform baseTransform;

    [SerializeField] private Transform barrelCenterFront;   // 2 - Default, Rotation, Strafe
    [SerializeField] private Transform barrelRightFront;    // 3 - Default, Rotation, Strafe
    [SerializeField] private Transform barrelRightCenter;   // 6 - Rotation
    [SerializeField] private Transform barrelRightBack;     // 9 - Rotation
    [SerializeField] private Transform barrelCenterBack;    // 8 - Rotation
    [SerializeField] private Transform barrelLeftBack;      // 7 - Rotation
    [SerializeField] private Transform barrelLeftCenter;    // 4 - Rotation
    [SerializeField] private Transform barrelLeftFront;     // 1 - Default, Rotation, Strafe
    [Space(10)]
    [SerializeField] private Bullet_SO defaultBullet;
    [Space(5)]
    [SerializeField] private Bullet_SO rotationBullet;
    [Space(5)]
    [SerializeField] private Bullet_SO strafeBullet;
    [Space(10)]
    [SerializeField] private float defaultBurstMax;
    [SerializeField] private float defaultBurstMin;
    [SerializeField] private float rotationBurstMax;
    [SerializeField] private float rotationBurstMin;
    [SerializeField] private float strafeBurstMax;
    [SerializeField] private float strafeBurstMin;
    [Space(10)]
    [SerializeField] private float defaultVolleyMax;
    [SerializeField] private float defaultVolleyMin;
    [SerializeField] private float rotationVolleyMax;
    [SerializeField] private float rotationVolleyMin;
    [SerializeField] private float strafeVolleyMax;
    [SerializeField] private float strafeVolleyMin;
    [Space(10)]
    [SerializeField] private int defaultVolleySize;
    [SerializeField] private int rotationVolleySize;
    [SerializeField] private int strafeVolleySize;

    private float currentShotBurstTimer;
    private float currentShotVolleyTimer;
    private int currentVolleySize;

    private void Awake()
    {
        this.parentMovement = this.GetComponent<BossMovement>();
        this.baseTransform = this.transform;
        this.currentShotBurstTimer = this.defaultBurstMax;
        this.currentShotVolleyTimer = this.defaultVolleyMax;
    }

    private void FixedUpdate()
    {
        this.currentShotVolleyTimer -= Time.deltaTime;
        if (this.currentShotVolleyTimer <= 0)
        {
            this.currentShotBurstTimer -= Time.deltaTime;
            if (this.currentShotBurstTimer <= 0)
            {
                this.FireBurst();
                this.ResetBurst();
                this.currentVolleySize++;
            }

            if (this.VolleySizeMet())
            {
                this.ResetVolley();
            }
        }
    }

    private void FireBurst()
    {
        switch (this.parentMovement.CurrentState)
        {
            case BossState.MovingToPointRotationState:
            case BossState.MovingToPointStrafeState:
            case BossState.DefaultState:
                this.FireDefaultBurst();
                break;

            case BossState.RotationFireState:
                this.FireRotationBurst();
                break;

            case BossState.StrafeFireState:
                this.FireStrafeBurst();
                break;

            default:
                this.FireDefaultBurst();
                break;
        }
    }

    private void FireDefaultBurst()
    {
        Bullet bulletLeft = Instantiate(this.defaultBullet.bulletToFire, this.barrelLeftFront.position, Quaternion.identity);
        Bullet bulletCenter = Instantiate(this.defaultBullet.bulletToFire, this.barrelCenterFront.position, Quaternion.identity);
        Bullet bulletRight = Instantiate(this.defaultBullet.bulletToFire, this.barrelRightFront.position, Quaternion.identity);

        bulletLeft.Fire(this.gameObject, this.barrelLeftFront.forward, defaultBullet.bulletSpeed, defaultBullet.bulletDamage, defaultBullet.collisionParticles, defaultBullet.collisionSound);
        bulletCenter.Fire(this.gameObject, this.barrelCenterFront.forward, defaultBullet.bulletSpeed, defaultBullet.bulletDamage, defaultBullet.collisionParticles, defaultBullet.collisionSound);
        bulletRight.Fire(this.gameObject, this.barrelRightFront.forward, defaultBullet.bulletSpeed, defaultBullet.bulletDamage, defaultBullet.collisionParticles, defaultBullet.collisionSound);
    }

    private void FireRotationBurst()
    {
        // loops aren't real and I refuse to create a "transform" list for no reason at all
        // if I were firing more than 8 bullets or an unknown number I'd be motivated to refactor
        Bullet bulletLeftFront = Instantiate(this.rotationBullet.bulletToFire, this.barrelLeftFront.position, Quaternion.identity);
        Bullet bulletCenterFront = Instantiate(this.rotationBullet.bulletToFire, this.barrelCenterFront.position, Quaternion.identity);
        Bullet bulletRightFront = Instantiate(this.rotationBullet.bulletToFire, this.barrelRightFront.position, Quaternion.identity);
        Bullet bulletRightCenter = Instantiate(this.rotationBullet.bulletToFire, this.barrelRightCenter.position, Quaternion.identity);
        Bullet bulletRightBack = Instantiate(this.rotationBullet.bulletToFire, this.barrelRightBack.position, Quaternion.identity);
        Bullet bulletCenterBack = Instantiate(this.rotationBullet.bulletToFire, this.barrelCenterBack.position, Quaternion.identity);
        Bullet bulletLeftBack = Instantiate(this.rotationBullet.bulletToFire, this.barrelLeftBack.position, Quaternion.identity);
        Bullet bulletLeftCenter = Instantiate(this.rotationBullet.bulletToFire, this.barrelLeftCenter.position, Quaternion.identity);

        bulletLeftFront.Fire(this.gameObject, this.barrelLeftFront.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletCenterFront.Fire(this.gameObject, this.barrelCenterFront.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletRightFront.Fire(this.gameObject, this.barrelRightFront.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletRightCenter.Fire(this.gameObject, this.barrelRightCenter.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletRightBack.Fire(this.gameObject, this.barrelRightBack.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletCenterBack.Fire(this.gameObject, this.barrelCenterBack.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletLeftBack.Fire(this.gameObject, this.barrelLeftBack.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
        bulletLeftCenter.Fire(this.gameObject, this.barrelLeftCenter.forward, this.rotationBullet.bulletSpeed, this.rotationBullet.bulletDamage, this.rotationBullet.collisionParticles, this.rotationBullet.collisionSound);
    }

    private void FireStrafeBurst()
    {
        Bullet bulletLeft = Instantiate(this.strafeBullet.bulletToFire, this.barrelLeftFront.position, Quaternion.identity);
        Bullet bulletCenter = Instantiate(this.strafeBullet.bulletToFire, this.barrelCenterFront.position, Quaternion.identity);
        Bullet bulletRight = Instantiate(this.strafeBullet.bulletToFire, this.barrelRightFront.position, Quaternion.identity);

        bulletLeft.Fire(this.gameObject, this.barrelLeftFront.forward, strafeBullet.bulletSpeed, strafeBullet.bulletDamage, strafeBullet.collisionParticles, strafeBullet.collisionSound);
        bulletCenter.Fire(this.gameObject, this.barrelCenterFront.forward, strafeBullet.bulletSpeed, strafeBullet.bulletDamage, strafeBullet.collisionParticles, strafeBullet.collisionSound);
        bulletRight.Fire(this.gameObject, this.barrelRightFront.forward, strafeBullet.bulletSpeed, strafeBullet.bulletDamage, strafeBullet.collisionParticles, strafeBullet.collisionSound);
    }

    private void ResetBurst()
    {
        this.currentShotBurstTimer = this.GetRandomRange(this.DetermineCurrentBurstTime());
    }

    private (float, float) DetermineCurrentBurstTime()
    {
        return this.parentMovement.CurrentState switch
        {
            BossState.DefaultState => (this.defaultBurstMin, this.defaultBurstMax),
            BossState.MovingToPointStrafeState => (this.defaultBurstMin, this.defaultBurstMax),
            BossState.MovingToPointRotationState => (this.defaultBurstMin, this.defaultBurstMax),
            BossState.RotationFireState => (this.rotationBurstMin, this.rotationBurstMax),
            BossState.StrafeFireState => (this.strafeBurstMin, this.strafeBurstMax),
            _ => (this.defaultBurstMin, this.defaultBurstMax)
        };
    }

    private bool VolleySizeMet()
    {
        return this.currentVolleySize >= this.DetermineCurrentVolleySize();
    }

    private int DetermineCurrentVolleySize()
    {
        return this.parentMovement.CurrentState switch
        {
            BossState.DefaultState => this.defaultVolleySize,
            BossState.MovingToPointStrafeState => this.defaultVolleySize,
            BossState.MovingToPointRotationState => this.defaultVolleySize,
            BossState.RotationFireState => this.rotationVolleySize,
            BossState.StrafeFireState => this.strafeVolleySize,
            _ => this.defaultVolleySize
        };
    }

    private void ResetVolley()
    {
        this.currentVolleySize = 0;
        this.currentShotVolleyTimer = this.GetRandomRange(this.DetermineCurrentVolleyTimer());
    }

    private (float, float) DetermineCurrentVolleyTimer()
    {
        return this.parentMovement.CurrentState switch
        {
            BossState.DefaultState => (this.defaultVolleyMin, this.defaultVolleyMax),
            BossState.MovingToPointStrafeState => (this.defaultVolleyMin, this.defaultVolleyMax),
            BossState.MovingToPointRotationState => (this.defaultVolleyMin, this.defaultVolleyMax),
            BossState.RotationFireState => (this.rotationVolleyMin, this.rotationVolleyMax),
            BossState.StrafeFireState => (this.strafeVolleyMin, this.strafeVolleyMax),
            _ => (this.defaultVolleyMin, this.defaultVolleyMax)
        };
    }

    private float GetRandomRange((float, float) range)
    {
        return Random.Range(range.Item1, range.Item2);
    }
}
