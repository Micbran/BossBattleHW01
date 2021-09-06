using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected float powerUpDuration = 5f;
    [SerializeField] protected float turnSpeed = 1f;

    [SerializeField] private ParticleSystem collectParticles;
    [SerializeField] private AudioClip collectAudio;
    [SerializeField] protected AudioClip expireAudio;

    [SerializeField] protected Collider colliderToDeactivate;
    [SerializeField] protected GameObject visualsToDeactivate;

    private Rigidbody rb;
    protected Quaternion turnOffset;

    protected abstract void ActivatePowerUp(Player player);
    protected abstract void DeactivatePowerUp(Player player);

    private void Awake()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.turnOffset = turnOffset = Quaternion.Euler(0, this.turnSpeed, 0);
        this.EnableRendering();
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
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        StartCoroutine(this.StartPowerUp(player));
        this.Feedback();
    }

    private IEnumerator StartPowerUp(Player player)
    {
        this.ActivatePowerUp(player);
        this.DisableRendering();

        yield return new WaitForSeconds(this.powerUpDuration);

        this.DeactivatePowerUp(player);
        Destroy(this.gameObject);
    }

    private void Feedback()
    {
        if (this.collectParticles != null)
        {
            Instantiate(this.collectParticles, this.transform.position, Quaternion.identity);
        }

        if (this.collectAudio != null)
        {
            AudioHelper.PlayClip2D(this.collectAudio, 1f);
        }
    }


    protected void EnableRendering()
    {
        this.colliderToDeactivate.enabled = true;
        this.visualsToDeactivate.SetActive(true);
    }

    protected void DisableRendering()
    {
        this.colliderToDeactivate.enabled = false;
        this.visualsToDeactivate.SetActive(false);
    }
}
