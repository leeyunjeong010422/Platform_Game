using UnityEngine;

public class RandomMoveState_M : MonsterStateBase
{
    private float moveDelay = 3f;

    public override void Enter(MonsterMover monster)
    {
        monster.Invoke("RandomMove", moveDelay);
    }

    public override void Update(MonsterMover monster)
    {
        monster.rigid.velocity = new Vector2(monster.nextMove * monster.speed, monster.rigid.velocity.y);

        Vector2 frontVector = new Vector2(monster.rigid.position.x + monster.nextMove * 0.2f, monster.rigid.position.y);
        RaycastHit2D hit = Physics2D.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            monster.Turn();
        }
    }

    public override void Exit(MonsterMover monster)
    {
        monster.CancelInvoke();
    }
}
