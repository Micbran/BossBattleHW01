using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDestroyEmitSound : IDestroy
{
    [SerializeField] private AudioClip deathSound;

    public override void Destruction()
    {
        if (this.deathSound != null)
        {
            AudioHelper.PlayClip2D(this.deathSound, 0.8f);
        }
    }
}
