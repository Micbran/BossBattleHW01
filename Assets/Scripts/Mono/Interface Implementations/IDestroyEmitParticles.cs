using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDestroyEmitParticles : IDestroy
{
    [SerializeField] private ParticleSystem deathParticles;

    public override void Destruction()
    {
        if (this.deathParticles != null)
        {
            Instantiate(this.deathParticles, this.transform.position, Quaternion.identity);
        }
    }
}
