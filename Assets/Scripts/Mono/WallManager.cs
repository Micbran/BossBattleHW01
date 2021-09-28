using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] Health bossHealth;
    [SerializeField] float wallSpawnTimerMax;
    [SerializeField] float wallSpawnTimerThreshold;
    [SerializeField] GameObject wallToSpawn;
    [SerializeField] List<GameObject> wallSpawnIndicators;
    [SerializeField] List<Transform> transformsToSpawnOn;
    [SerializeField] AudioClip wallSpawnClip;
    [SerializeField] AudioClip halfHealthClip;
    [SerializeField] AudioClip indicatorSoundClip;

    private float wallSpawnTimer = 0;
    private int bossMaxHealth;
    private bool isActive = false;
    private bool indicatorSound = true;

    private void Awake()
    {
        this.bossMaxHealth = this.bossHealth.MaxHealth;
        foreach (GameObject indicator in this.wallSpawnIndicators)
        {
            indicator.SetActive(false);
        }
    }

    private void OnEnable()
    {
        this.bossHealth.OnTakeDamage += this.CheckHealth;
    }

    private void OnDisable()
    {
        this.bossHealth.OnTakeDamage -= this.CheckHealth;
    }

    private void Update()
    {
        if (!this.isActive)
            return;

        this.wallSpawnTimer -= Time.deltaTime;
        if (this.wallSpawnTimer <= this.wallSpawnTimerThreshold && !indicatorSound)
        {
            indicatorSound = true;
            if (this.indicatorSoundClip != null)
            {
                AudioHelper.PlayClip2D(this.indicatorSoundClip, 0.8f);
            }
            foreach (GameObject indicator in this.wallSpawnIndicators)
            {
                indicator.SetActive(true);
            }
        }
        if (this.wallSpawnTimer <= 0)
        {
            foreach (Transform location in this.transformsToSpawnOn)
            {
                Collider[] check = Physics.OverlapSphere(location.position, 0.5f);
                bool noWall = true;
                foreach (Collider collider in check)
                {
                    if (collider.tag.Equals("Wall"))
                    {
                        noWall = false;
                    }
                }
                if (noWall)
                {
                    Instantiate(this.wallToSpawn, location.position, Quaternion.identity);
                }
            }
            if (this.wallSpawnClip != null)
            {
                AudioHelper.PlayClip2D(this.wallSpawnClip, 0.7f);
            }

            foreach (GameObject indicator in this.wallSpawnIndicators)
            {
                indicator.SetActive(false);
            }
            this.wallSpawnTimer = this.wallSpawnTimerMax;
            indicatorSound = false;
        }
    }

    private void CheckHealth(int currentHealth)
    {
        if (currentHealth <= (this.bossMaxHealth / 2) && !this.isActive)
        {
            this.isActive = true;
            if (halfHealthClip != null)
            {
                AudioHelper.PlayClip2D(this.halfHealthClip, 0.7f);
            }
        }
    }
}
