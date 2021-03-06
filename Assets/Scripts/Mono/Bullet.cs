using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float projectileSpeed;
    private int damage;
    private GameObject shooter;
    private Vector3 travelDirection;
    private bool targetHit;
    private Transform baseTransform;
    private bool isPlayerBullet;

    private ParticleSystem collisionParticles;
    private AudioClip collisionSound;

    public void Fire(GameObject shooter, Vector3 travelDirection, float projectileSpeed, int damage, 
        ParticleSystem collisionParticles = null, AudioClip collisionSound = null, bool isPlayerBullet = false)
    {
        this.shooter = shooter;
        this.projectileSpeed = projectileSpeed;
        this.damage = damage;
        this.travelDirection = travelDirection;
        this.travelDirection.Normalize();

        this.collisionParticles = collisionParticles;
        this.collisionSound = collisionSound;

        this.baseTransform = this.transform;
        this.targetHit = false;
        this.isPlayerBullet = isPlayerBullet;
    }

    private void Update()
    {
        if (targetHit)
            return;

        float distanceToTravel = projectileSpeed * Time.deltaTime;

        this.baseTransform.Translate(travelDirection * distanceToTravel);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (targetHit)
            return;

        if (collision.gameObject == shooter)
            return;

        if (collision.gameObject.GetComponent<Bullet>())
            return;

        // dirty + accident-prone solution, but irrelevant for the scope of this game
        if (collision.gameObject.tag.Equals("Wall") && !this.isPlayerBullet)
            return;

        this.targetHit = true;
        IDamageable[] damageables = collision.gameObject.GetComponents<IDamageable>();
        foreach (IDamageable damageable in damageables)
        {
            damageable.TakeDamage(this.damage);
        }
        this.Feedback();
        Destroy(gameObject);
    }

    private void Feedback()
    {
        if (this.collisionParticles != null)
        {
            Instantiate(this.collisionParticles, this.transform.position, Quaternion.identity);
        }
        if (this.collisionSound != null)
        {
            AudioHelper.PlayClip2D(this.collisionSound, 1f);
        }
    }
}
