using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float projectileSpeed;
    private int damage;
    private GameObject shooter;
    private Vector3 travelDirection;
    private bool targetHit;
    private Transform baseTransform;

    public void Fire(GameObject shooter, Vector3 travelDirection, float projectileSpeed, int damage)
    {
        this.shooter = shooter;
        this.projectileSpeed = projectileSpeed;
        this.damage = damage;
        this.travelDirection = travelDirection;
        this.travelDirection.Normalize();

        this.baseTransform = this.transform;
        this.targetHit = false;
    }

    private void Update()
    {
        if (targetHit)
            return;

        float distanceToTravel = projectileSpeed * Time.deltaTime;

        this.baseTransform.Translate(travelDirection * distanceToTravel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;

        if (collision.gameObject == shooter)
            return;

        this.targetHit = true;
        IDamageable[] damageables = collision.gameObject.GetComponents<IDamageable>();
        foreach (IDamageable damageable in damageables)
        {
            damageable.TakeDamage(damage); // TODO put damage numbers somewhere
        }
        Destroy(gameObject);
    }
}
