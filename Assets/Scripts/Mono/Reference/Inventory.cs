using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Text treasureDisplayField;
    private int currentTreasure;

    public void UpdateTreasure(int amountToAdd)
    {
        this.currentTreasure += amountToAdd;
        this.treasureDisplayField.text = this.currentTreasure.ToString();
    }
}
