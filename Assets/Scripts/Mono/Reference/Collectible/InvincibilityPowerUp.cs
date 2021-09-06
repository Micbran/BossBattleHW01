using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerUp : PowerUp
{
    protected override void ActivatePowerUp(Player player)
    {
        player.Invincible = true;
        player.SetColor(new Color(58, 208, 239));
    }

    protected override void DeactivatePowerUp(Player player)
    {
        if (player != null)
        {
            player.Invincible = false;
            player.ResetColor();
            if (this.expireAudio != null)
            {
                AudioHelper.PlayClip2D(this.expireAudio, 1f);
            }
        }
    }
}
