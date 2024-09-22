using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IPlayerState
{
    public override void Enter(PlayerController player)
    {
        player.rigid.velocity = Vector2.zero;
        player.spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        player.spriteRenderer.flipY = true;
        player.collider2d.enabled = false;
        player.rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}
