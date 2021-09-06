using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private ParticleSystem collisionParticles = null;
    [SerializeField] private AudioClip collisionSound = null;

    private Rigidbody rb;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        return;
    }

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }
        this.PlayerImpact(player);
        this.ImpactFeedback();
    }

    protected virtual void PlayerImpact(Player player)
    {
        player.DecreaseHealth(this.damageAmount);
    }

    protected void ImpactFeedback()
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
