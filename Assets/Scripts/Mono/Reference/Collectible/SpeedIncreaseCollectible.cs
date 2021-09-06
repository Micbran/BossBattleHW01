using UnityEngine;

public class SpeedIncreaseCollectible : Collectible
{
    [SerializeField] float speedAmount = 3;

    protected override void Collect(PlayerTank player)
    {
        TankController playerController = player.GetComponent<TankController>();
        if (playerController == null)
        {
            return;
        }

        playerController.MoveSpeed = speedAmount;
    }

    protected override void Awake()
    {
        base.Awake();
        turnOffset = Quaternion.Euler(this.MovementSpeed, this.MovementSpeed, this.MovementSpeed);
    }
}
