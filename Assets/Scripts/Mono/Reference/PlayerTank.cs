using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
[RequireComponent(typeof(Inventory))]
public class PlayerTank : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private MeshRenderer[] visuals;
    private List<Color> originalColors = new List<Color>();
    private int currentHealth;
    private bool invincible = false;
    
    public bool Invincible
    {
        set
        {
            this.invincible = value;
        }
    }

    private TankController tankController;
    private Inventory inventory;

    private void Awake()
    {
        this.tankController = GetComponent<TankController>();
        this.inventory = GetComponent<Inventory>();
        foreach(MeshRenderer mesh in this.visuals)
        {
            originalColors.Add(mesh.material.color);
        }
    }

    private void Start()
    {
        this.currentHealth = maxHealth;
    }

    private void Update()
    {

    }


    public void IncreaseHealth(int amount)
    {
        this.currentHealth = Mathf.Clamp(this.currentHealth + amount, 0, this.maxHealth);
        Debug.Log("Player's Health: " + this.currentHealth);
    }

    public void DecreaseHealth(int amount)
    {
        if (this.invincible)
        {
            return;
        }
        this.currentHealth = Mathf.Clamp(this.currentHealth - amount, 0, this.maxHealth);
        Debug.Log("Player's Health: " + this.currentHealth);
        if (this.currentHealth <= 0)
        {
            this.Kill();
        }
    }

    public void Kill()
    {
        if (this.invincible)
        {
            return;
        }
        this.gameObject.SetActive(false);
        // fire kill events
    }

    public void AddScore(int addedTreasure)
    {
        this.inventory.UpdateTreasure(addedTreasure);
    }

    public void SetColor(Color newColor)
    {
        foreach(MeshRenderer mesh in this.visuals)
        {
            mesh.material.color = newColor;
        }
    }

    public void ResetColor()
    {
        for(int i = 0; i < this.visuals.Length; i++)
        {
            this.visuals[i].material.color = this.originalColors[i];
        }
    }
}
