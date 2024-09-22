using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DamagedState : IPlayerState
{
    private Vector2 targetPosition;

    public DamagedState(Vector2 targetPos)
    {
        targetPosition = targetPos;
    }

    public override void Enter(PlayerController player)
    {
        player.gameManager.HealthDown();
        player.gameObject.layer = 8;
        player.spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int direction = player.transform.position.x - targetPosition.x > 0 ? 1 : -1;
        player.rigid.AddForce(new Vector2(direction, 1) * 7, ForceMode2D.Impulse);

        player.animator.SetTrigger("doDamaged");
        player.Invoke("OffDamaged", 3);
    }

    public override void Exit(PlayerController player)
    {
        player.OffDamaged();
    }
}


