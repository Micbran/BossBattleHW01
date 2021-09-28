using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class Player : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] visuals;
    private List<Color> originalColors = new List<Color>();
    private bool invincible = false;

    public bool Invincible
    {
        set
        {
            this.invincible = value;
        }

        get
        {
            return this.invincible;
        }
    }

    private ShipController shipController;

    private void Awake()
    {
        this.shipController = GetComponent<ShipController>();
        foreach (MeshRenderer mesh in this.visuals)
        {
            originalColors.Add(mesh.material.color);
        }
    }

    public void SetColor(Color newColor)
    {
        foreach (MeshRenderer mesh in this.visuals)
        {
            mesh.material.color = newColor;
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < this.visuals.Length; i++)
        {
            this.visuals[i].material.color = this.originalColors[i];
        }
    }
}
