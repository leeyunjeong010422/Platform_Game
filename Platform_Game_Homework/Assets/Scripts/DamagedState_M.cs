using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState_M : MonsterStateBase
{
    public override void Enter(MonsterMover monster)
    {
        monster.spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        monster.spriteRenderer.flipY = true;
        monster.collider2d.enabled = false;
        monster.rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        monster.Invoke("DeActive", 5);
    }
}

