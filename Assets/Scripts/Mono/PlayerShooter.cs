using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet_SO bulletStats;
    [SerializeField] private Transform origin;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCharging();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseCharge();
        }
    }

    private void StartCharging()
    {
        return;
    }

    private void ReleaseCharge()
    {
        Bullet bullet = Instantiate(bulletStats.bulletToFire, origin.position, Quaternion.identity);
        bullet.Fire(this.gameObject, Vector3.left, bulletStats.bulletSpeed, this.CalculateProjectileDamage(bulletStats.bulletDamage));
    }

    private int CalculateProjectileDamage(int originalDamage)
    {
        return originalDamage;
    }
}
