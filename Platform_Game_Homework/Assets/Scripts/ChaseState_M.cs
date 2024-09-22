using UnityEngine;

//플레이어를 추적하는 상태
public class ChaseState_M : MonsterStateBase
{
    public override void Update(MonsterMover monster)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(monster.transform.position, player.transform.position);

        if (distance < 1f) //거리가 1미터 보다 짧으면
        {
            monster.ChangeState(new AttackState_M()); //공격 상태로 전환
        }
        else if (distance < 3f) //3미터 보다 짧으면
        {
            Vector2 direction = (player.transform.position - monster.transform.position).normalized;

            monster.spriteRenderer.flipX = direction.x > 0; //방향 전환

            monster.rigid.velocity = new Vector2(direction.x * monster.speed, monster.rigid.velocity.y); //플레이어 추적
        }
        else //둘다 아니면 다시 랜덤 이동 상태로 전환
        {
            monster.ChangeState(new RandomMoveState_M());
        }
    }
}
