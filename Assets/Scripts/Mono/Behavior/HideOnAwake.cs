using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnAwake : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
