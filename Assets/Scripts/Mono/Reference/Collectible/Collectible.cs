using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Collectible : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    protected float MovementSpeed => this.movementSpeed;

    [SerializeField] private ParticleSystem collectParticles;
    [SerializeField] private AudioClip collectAudio;
    [SerializeField] private float clipVolume = 1f;

    private Rigidbody rb;
    protected Quaternion turnOffset;

    protected abstract void Collect(PlayerTank player);

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        turnOffset = Quaternion.Euler(0, this.movementSpeed, 0);
    }

    private void FixedUpdate()
    {
        Movement(this.rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        rb.MoveRotation(rb.rotation * this.turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerTank player = other.gameObject.GetComponent<PlayerTank>();
        if (player == null)
        {
            return;
        }

        this.Collect(player);
        this.Feedback();

        gameObject.SetActive(false);
    }

    private void Feedback()
    {
        if (this.collectParticles != null)
        {
            this.collectParticles = Instantiate(this.collectParticles, this.transform.position, Quaternion.identity);
        }

        if (this.collectAudio != null)
        {
            AudioHelper.PlayClip2D(this.collectAudio, this.clipVolume);
        }
    }
}
