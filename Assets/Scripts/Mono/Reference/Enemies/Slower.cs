using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] private float speedAmount = 0.15f;

    protected override void PlayerImpact(Player player)
    {
        TankController playerController = player.GetComponent<TankController>();
        if (playerController == null)
        {
            return;
        }

        playerController.MoveSpeed = speedAmount;
    }
}
