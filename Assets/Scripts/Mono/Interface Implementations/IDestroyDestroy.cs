using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDestroyDestroy : IDestroy
{
    public override void Destruction()
    {
        Destroy(this.gameObject);
    }
}
