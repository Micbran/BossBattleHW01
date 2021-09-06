using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet.asset", menuName = "Bullet")]
public class Bullet_SO : ScriptableObject
{
    public int bulletDamage;
    public float bulletSpeed;

    public Bullet bulletToFire;
}
